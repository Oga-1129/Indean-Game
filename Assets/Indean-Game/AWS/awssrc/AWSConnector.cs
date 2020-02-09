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
using UnityEngine.SceneManagement;

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
    public static int iddate = 01;
    public string[] Playername = new string[4];
    public string[] PlayerThema = new string[4];
    Pass _pass = new Pass();
    Matching _matching;
    InputChat _inputchat;
    public int membernum;
    public string talk;
    public int roomID;
    public string talkname;
    
    public int talkid;
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
            OpenRoom = System.DateTime.Now,
            StPlayer = " ",
            StUpdate = false,
            UpNum = 0,
            PNum = 0,
            TalkID = 0
        };

        iddate = roomid;
        //DBに保存
        context.SaveAsync(mylog,(result)=>{
            //問題なし
            if(result.Exception == null){
                //Debug.Log("DB saved\n");
                SceneManager.LoadScene("Matching"); 
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
    public IEnumerator GetDynamoDB(int cheack)
    {
        //PlayLogの初期化
        PlayLog mylog = null;


        //検索
        string query = "SELECT Indean-GameDB WHERE RoomID = " + 0;

        

        //リクエスト作成
        context.LoadAsync<PlayLog>(iddate,
                    (AmazonDynamoDBResult<PlayLog> result) =>                       
        {
            //取ってきたデータをmylog(PlayLog)に保存
            mylog = result.Result as PlayLog;
            

            //プレイヤー１の名前の取得
            Playername[0] = (string)mylog.P1N;

            //プレイヤー2の名前の取得
            Playername[1] = (string)mylog.P2N;


            //プレイヤー3の名前の取得
            Playername[2] = (string)mylog.P3N;
                
            //プレイヤー4の名前の取得
            Playername[3] = (string)mylog.P4N;

            //プレイヤー1のお題の取得
            PlayerThema[0] = (string)mylog.P1Q;

            //プレイヤー2のお題の取得
            PlayerThema[1] = (string)mylog.P2Q;

            //プレイヤー3のお題の取得
            PlayerThema[2] = (string)mylog.P3Q;

            //プレイヤー4のお題の取得
            PlayerThema[3] = (string)mylog.P4Q;

            //トークID取得
            talkid = mylog.TalkID;

            //話している人
            talkname = mylog.StPlayer;


            //問題あり
            if (result.Exception != null)
            {
                //エラー表示
                Debug.Log("LoadAsync error" +result.Exception.Message + "\n");
                Debug.LogException(result.Exception);
                return;
            }else{              
                if(SceneManager.GetActiveScene().name == "Matching"){
                    _matching = GameObject.Find("Matching").GetComponent<Matching>();
                    if(cheack == 0){
                        _matching.Register();
                    }else if(cheack == 1){
                        //名前表示
                        _matching.setReName();
                    }
                }
            }

        }, null);


        yield return new WaitForSeconds(3);
        if(SceneManager.GetActiveScene().name == "Matching")
        {
            _matching = GameObject.Find("Matching").GetComponent<Matching>();
            _matching.GetPName();
        }else if(SceneManager.GetActiveScene().name == "Main")
        {
            _inputchat = GameObject.Find("InputChat").GetComponent<InputChat>();
            if(cheack == 0){
                _inputchat.startup();
            }else if(cheack == 1){
                _inputchat.GetTalkID();
            }
        }
    }

    #endregion

    #region Update DynamoDB
    // /// <summary>
    // /// DynamoDBのデータの更新
    // /// <summary>
    //string username,string question, bool Pre, string talk, string stplayer, bool stupdate

    public IEnumerator UpdateDynamoDB(string updatename, string state, bool hoge, int num, string talkname)
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
                    case "P1N": mylog.P1N = state; 
                                mylog.PNum++; break;

                    case "P2N": mylog.P2N = state; 
                                mylog.PNum++; break;

                    case "P3N": mylog.P3N = state; 
                                mylog.PNum++; break;

                    case "P4N": mylog.P4N = state;
                                mylog.PNum++; break;

                    case "P1Q": mylog.P1Q = state; break;
                    case "P2Q": mylog.P2Q = state; break;
                    case "P3Q": mylog.P3Q = state; break;
                    case "P4Q": mylog.P4Q = state; break;

                    case "P1Pre": mylog.P1Pre = hoge; break;
                    case "P2Pre": mylog.P2Pre = hoge; break;
                    case "P3Pre": mylog.P3Pre = hoge; break;
                    case "P4Pre": mylog.P4Pre = hoge; break;

                    case "Remarks" : mylog.Remarks = state; 
                                    mylog.TalkID = Random.Range(0, 999 + 1);
                                    mylog.StPlayer = talkname; break;

                    case "StUpdate": mylog.StUpdate = hoge; break;

                    case "UpNum"   : mylog.UpNum = num; break;
                    
                }
                membernum = mylog.PNum;

                //DBに保存
                context.SaveAsync<PlayLog>(mylog,(res)=>
                {
                    //問題なし
                    if(res.Exception == null){
                        //Debug.Log("DB updated\n");
                        if(SceneManager.GetActiveScene().name == "Matching")
                        {
                            _matching = GameObject.Find("Matching").GetComponent<Matching>();
                            //SQLiteのアップデート
                            _matching.updatesqldb();
                            //AWSに登録したユーザー名の取得
                            _matching.GetPName();
                        }else if(SceneManager.GetActiveScene().name == "Main"){
                            talkname = mylog.StPlayer;
                            talk = mylog.Remarks;
                        }else if(SceneManager.GetActiveScene().name == "Thema"){
                            SceneManager.LoadScene("Matching"); 
                        }
                    }
                });
            }else{
                Debug.Log(result.Exception);
            }
        });
        yield return  0;
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