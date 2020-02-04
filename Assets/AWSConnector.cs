using System;
using System.IO;
using System.Collections;
using UnityEngine;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3.Model;
using UnityEngine.UI;
using TMPro;

public class AWSConnector
{
#region variable

    //S3バケット名
    const string BUCKET_NAME = "co-test-aws";
    static RegionEndpoint COGNITO_REGION = RegionEndpoint.APNortheast1; // リージョン。適宜変更
    static RegionEndpoint S3_REGION = RegionEndpoint.APNortheast1; // リージョン。適宜変更
    static RegionEndpoint DYNAMO_REGION = RegionEndpoint.APNortheast1; // リージョン。適宜変更

    CognitoAWSCredentials credentials;
    AmazonS3Client S3Client;
    AmazonDynamoDBClient DynamoDBClient;
    DynamoDBContext context;
    int iddate = 01;
#endregion

#region constructor
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AWSConnector()
    {
        // Amazon Cognito 認証情報プロバイダーの初期化
        credentials = new CognitoAWSCredentials (
            "", // ID プールの ID
            COGNITO_REGION // リージョン
        );
        //S3Clientの初期化
        S3Client = new AmazonS3Client(credentials, S3_REGION);

        //DynamoDBClientの初期化
        DynamoDBClient = new AmazonDynamoDBClient(credentials, DYNAMO_REGION);
        
        //DynamoDBContextの初期化
        context = new DynamoDBContext(DynamoDBClient);
    }
#endregion


#region S3
    #region upload File To S3
    /// <summary>
    /// 指定ファイルをS3バケットにアップロード
    /// </summary>
    /// <param name="resulttext">結果のテキスト表示用</param>
    /// <param name="inputFileFullPath">アップロードするローカルファイルパス</param>
    /// <param name="uploadS3path">S3パス。fol/filenameと指定するとfolフォルダ以下にアップロードする</param>
    public void uploadFileToS3(TextMeshProUGUI resulttext, string inputFileFullPath, string uploadS3path)
    {
        //ファイル読み込み
        var stream = new FileStream(inputFileFullPath,
            FileMode.Open, FileAccess.Read, FileShare.Read);

        //リクエスト作成
        var request = new PostObjectRequest()
        {
            Bucket = BUCKET_NAME,
            Key = uploadS3path,
            InputStream = stream,
            CannedACL = S3CannedACL.PublicRead,
            Region = S3_REGION
        };

        //アップロード
        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null){
                //Success
                Debug.Log(uploadS3path + "   :Upload successed");
                resulttext.text += string.Format("UPLoad S3 successed\n");
            }
            else{
                Debug.LogError(string.Format("receieved error {0}", responseObj.Response.HttpStatusCode.ToString()));
            }
        });
    }
    #endregion

    #region Get Bucket List
    /// <summary>
    /// Example method to Demostrate GetBucketList
    /// </summary>
    public void GetBucketList( Text ResultText)
    {
        ResultText.text += "Fetching all the Buckets";
        S3Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        {
            Debug.Log(responseObject.Exception);
            ResultText.text += "\n";
            if (responseObject.Exception == null)
            {
                ResultText.text += "\n Got Response \nPrinting now \n";
                responseObject.Response.Buckets.ForEach((s3b) =>
                {
                    ResultText.text += string.Format("\n bucket = {0}, created date = {1} \n", s3b.BucketName, s3b.CreationDate);
                });
            }
            else
            {
                ResultText.text += " \nGot Exception";
            }
        });
    }
    #endregion

    #region download File To S3
    /// <summary>
    /// アップロードしたファイルを今度はダウンロードする
    /// </summary>
    /// <param name="resulttext">結果のテキスト表示用</param>
    /// <param name="imagebox">　ダウンロードした画像を表示させるためのImage</param>
    public void downloadFileToS3(TextMeshProUGUI resulttext)
    {   
        S3Client.GetObjectAsync(BUCKET_NAME, "nyanko.jpg", (responseObj) =>
        {
            //リクエスト作成
            var response = responseObj.Response;
            if (response.ResponseStream != null)
            {
                MemoryStream stream = new MemoryStream();
                responseObj.Response.ResponseStream.CopyTo(stream);
                Texture2D tex = new Texture2D(4, 4);
                byte[] results = stream.ToArray();
                tex.LoadImage(results);
                resulttext.text += "\n";

            }
        });
        resulttext.text += string.Format("DownLoad S3 successed\n");
    }
    #endregion
#endregion


#region DynamoDB
    #region Create DynamoDB
    /// <summary>
    /// DynamoDBにデータを作成
    /// <summary>
    /// <param name="resulttext">結果のテキスト表示用</param>
    /// <param name="inputid">主キー</param>
    /// <param name="inputusername">変更内容</param>
    public IEnumerator CreateDynamoDB(TextMeshProUGUI resulttext, TMP_InputField inputid, TMP_InputField inputusername)
    {
        //idの設定
        idset(inputid, inputusername);

        //PlayLogの初期化
        PlayLog mylog = new PlayLog
        {
            id = iddate,
            username = inputusername.text,
            date = DateTime.Now
        };

        //DBに保存
        context.SaveAsync(mylog,(result)=>{
            //問題なし
            if(result.Exception == null){
                resulttext.text += string.Format("DB saved\n");
            }
            //問題あり
            else{
                resulttext.text += string.Format("no saved\n");
            }
        });
        yield return 0;
    }
    #endregion

    #region Get DynamoDB
    /// <summary>
    /// DynamoDBのデータを取得
    /// <summary>
    /// <param name="resulttext">結果のテキスト表示用</param>
    /// <param name="inputid">主キー</param>
    /// <param name="inputusername">変更内容</param>
    public IEnumerator GetDynamoDB(TextMeshProUGUI resulttext , TMP_InputField inputid, TMP_InputField inputusername)
    {
        //PlayLogの初期化
        PlayLog mylog = null;

        //idの設定
        idset(inputid, inputusername);
        resulttext.text += "*** Load DB***\n";

        //リクエスト作成
        context.LoadAsync<PlayLog>(iddate,
                                    (AmazonDynamoDBResult<PlayLog> result) =>                       
        {
            //取ってきたデータをmylog(PlayLog)に保存
            mylog = result.Result as PlayLog;

            //問題あり
            if (result.Exception != null)
            {
                //エラー表示
                resulttext.text += string.Format("LoadAsync error" +result.Exception.Message + "\n");
                Debug.LogException(result.Exception);
                return;
            }

            Debug.Log("Id = " + mylog.id);
            Debug.Log("Name = " + mylog.username);
            Debug.Log("Date = " + mylog.date);

            //テキスト表示
            resulttext.text += string.Format("Id   = " + mylog.id + "\n" +
                                            "Name = " + mylog.username + "\n" + 
                                            "Date = " + mylog.date + "\n");
        }, null);
        yield return 0;
    }

    #endregion

    #region Update DynamoDB
    /// <summary>
    /// DynamoDBのデータの更新
    /// <summary>
    /// <param name="resulttext">結果のテキスト表示用</param>
    /// <param name="inputid">主キー</param>
    /// <param name="inputusername">変更内容</param>
    public IEnumerator UpdateDynamoDB(TextMeshProUGUI resulttext, TMP_InputField inputid, TMP_InputField inputusername)
    {
        //PlayLogの初期化
        PlayLog mylog = null;
        //idの設定
        idset(inputid, inputusername);

        //リクエスト作成
        context.LoadAsync<PlayLog>(iddate,(result)=>
        {
            //問題なし
            if(result.Exception == null )
            {
                //アップデートする対象をmylog(PlayLog)に決める
                mylog = result.Result as PlayLog;

                // Update few properties.
                //アップデート内容
                mylog.username = inputusername.text;
                mylog.date = DateTime.Now;

                //DBに保存
                context.SaveAsync<PlayLog>(mylog,(res)=>
                {
                    //問題なし
                    if(res.Exception == null){
                        resulttext.text += string.Format("DB updated\n");
                    }
                    Debug.Log(res.Exception);
                });
                
            }
        });
        yield return 0;
    }
    #endregion

    #region Delete DynamoDB
    /// <summary>
    /// DynamoDBのデータを削除
    /// <summary>
    /// <param name="resulttext">結果のテキスト表示用</param>
    /// <param name="inputid">主キー</param>
    /// <param name="inputusername">変更内容</param>
    public IEnumerator DeleteDynamoDB(TextMeshProUGUI resulttext, TMP_InputField inputid, TMP_InputField inputusername)
    {
        PlayLog mylog = null;
        //idの設定
        idset(inputid, inputusername);

        //消去リクエスト作成
        context.DeleteAsync<PlayLog>(iddate,(res)=>
        {
            //問題なし
            if(res.Exception == null)
            {
                //リクエスト作成
                context.LoadAsync<PlayLog>(iddate,(result)=>
                {
                    mylog = result.Result;
                    if(mylog==null)　resulttext.text += string.Format("DB is deleted\n");
                });
            }
        });
        yield return 0;
    }
    #endregion

    #region idset
    /// <summary>
    /// idの設定
    /// </summary>
    /// <param name="inputid"></param> 主キー
    /// <param name="inputusername"></param> 変更内容
    /// <returns></returns>
    int idset(TMP_InputField inputid, TMP_InputField inputusername)
    {
        Debug.Log(inputid.text);
        Debug.Log(inputusername.text);
        //受け取ったstring型をint型に変換
        Int32.TryParse(inputid.text, out iddate);
        return iddate;
    }
    #endregion
#endregion
}
