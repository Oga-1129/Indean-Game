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

    Pass _pass = new Pass();
#endregion

#region constructor
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AWSConnector()
    {
        // Amazon Cognito 認証情報プロバイダーの初期化
        credentials = new CognitoAWSCredentials (
           "ap-northeast-1:6f472862-912a-42cc-867d-dbd1c34e2fcc", // ID プールの ID
            COGNITO_REGION // リージョン
        );
        //S3Clientの初期化
        S3Client = new AmazonS3Client(credentials, S3_REGION);

        //DynamoDBClientの初期化
        DynamoDBClient = new AmazonDynamoDBClient(credentials, DYNAMO_REGION);
        
        //DynamoDBContextの初期化
        context = new DynamoDBContext(DynamoDBClient);
        Debug.Log(_pass.idpool);
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
    #region  Create DynamoDB
    /// <summary>
    /// DynamoDBにデータを作成
    /// <summary>
    public IEnumerator CreateDynamoDB(int roomid)
    {
        Debug.Log("値をDynamoDBに生成");
        //PlayLogの初期化
        PlayLog mylog = new PlayLog
        {
            RoomID = roomid,
            P1N = " ",
            P2N = " ",
            P3N = " ",
            P4N = " ",
            P1Q = " ",
            P2Q = " ",
            P3Q = " ",
            P4Q = " ",
            P1Pre = false,
            P2Pre = false,
            P3Pre = false,
            P4Pre = false,
            Remarks = " ",
            OpenRoom = DateTime.Now,
            StPlayer = " ",
            StUpdate = false,
            UpNum = 0,
            PNum = 0
        };

        //DBに保存
        context.SaveAsync(mylog,(result)=>{
            //問題なし
            if(result.Exception == null){
                Debug.Log("DB saved\n");
            }
            //問題あり
            else{
                Debug.Log(result.Exception);
            }
        });
        yield return 0;
    }
    #endregion

    #region Get DynamoDB
    /// <summary>
    /// DynamoDBのデータを取得
    /// <summary>
    public IEnumerator GetDynamoDB()
    {
        //PlayLogの初期化
        PlayLog mylog = null;

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
                Debug.Log("LoadAsync error" +result.Exception.Message + "\n");
                Debug.LogException(result.Exception);
                return;
            }

            Debug.Log("Id = " + mylog.RoomID);
            Debug.Log("Name = " + mylog.P1N);
            Debug.Log("Date = " + mylog.P1Q);
        }, null);
        yield return 0;
    }

    #endregion

    #region Update DynamoDB
    // /// <summary>
    // /// DynamoDBのデータの更新
    // /// <summary>
    public IEnumerator UpdateDynamoDB(string updatename, string username,string question, bool Pre, string talk, string stplayer, bool stupdate)
    {
        //PlayLogの初期化
        PlayLog mylog = null;

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
                switch(updatename){
                    case "P1N": mylog.P1N = username; break;
                    case "P2N": mylog.P2N = username; break;
                    case "P3N": mylog.P3N = username; break;
                    case "P4N": mylog.P4N = username; break;

                    case "P1Q": mylog.P1Q = question; break;
                    case "P2Q": mylog.P2Q = question; break;
                    case "P3Q": mylog.P3Q = question; break;
                    case "P4Q": mylog.P4Q = question; break;

                    case "P1Pre": mylog.P1Pre = Pre; break;
                    case "P2Pre": mylog.P2Pre = Pre; break;
                    case "P3Pre": mylog.P3Pre = Pre; break;
                    case "P4Pre": mylog.P4Pre = Pre; break;

                    case "Remarks" : mylog.Remarks = talk; break;
                    case "StPlayer": mylog.StPlayer = stplayer; break;
                    case "StUpdate": mylog.StUpdate = stupdate; break;
                    case "UpNum"   : mylog.UpNum = 0;break;
                    case "PNum"    : mylog.PNum = 0;break;
                }


                //DBに保存
                context.SaveAsync<PlayLog>(mylog,(res)=>
                {
                    //問題なし
                    if(res.Exception == null){
                        Debug.Log("DB updated\n");
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
    public IEnumerator DeleteDynamoDB()
    {
        PlayLog mylog = null;

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
                    if(mylog==null)　Debug.Log("DB is deleted\n");
                });
            }
        });
        yield return 0;
    }
    #endregion
#endregion
}