using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using TMPro;
using UnityEngine.SceneManagement;

public class Matching : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;
    AWSConnector _AWS;
    int num;

    public GameObject StartButton;

    bool update;
    public TextMeshProUGUI text;
    
    public TextMeshProUGUI[] playertext = new TextMeshProUGUI[4];
    string Pname;

    // Start is called before the first frame update
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        //AWS通信
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        //AWSConnectorのオブジェクト化
        _AWS = new AWSConnector();

        //SQLite操作用
        DBSrc = DB.GetComponent<SampleDataBase>();  
        DBSrc.SelectDB();

        //SQLiteに保存してあるユーザー名
        Pname = DBSrc.PlayerName;

        //テキスト操作用
        text = text.GetComponent<TextMeshProUGUI> ();
        text.text = "<size=60%>データ登録中・・・</size>";
        for(int i = 0; i< 4; i++)
        {
            playertext[i] = playertext[i].GetComponent<TextMeshProUGUI>();
        }
        
        StartButton.SetActive(false);
        StartCoroutine(_AWS.GetDynamoDBPlayer(0));
    }


    public void GetPName()
    {
        text.text = "<size=60%>データ取得中・・・</size>";
        StartCoroutine(_AWS.GetDynamoDBPlayer(1));
        StartCoroutine(_AWS.GetDynamoDBState());
    }


    //取得したプレイヤー名の表示
    public void SetPName(int number)
    {
        Debug.Log("Name");
        string Pre;
        if(_AWS.PlayerPre[number] == "true")
        {
            Pre = "  OK ";
        }else{
            Pre = "準備中";
        }
        playertext[number].text = "Player" + (number+1) + "    " + Pre +  "   " + _AWS.Playername[number] + "\n";
    }


    //SQLiteのアップデート
    public void updatesqldb()
    {
        if(_AWS.Playername[DBSrc.num-1] != Pname){
            num = _AWS.membernum;
            text.text = "<size=60%>プレイヤーのデータ更新中・・・</size>";
            DBSrc.UpdateDB(Pname, "true" , num);
        }
    }


    public void Register()
    {
        if(_AWS.Playername[DBSrc.num-1] != Pname){
            Debug.Log("Register");
            num++;
            //新規ユーザーの登録
            StartCoroutine(_AWS.UpdateState("P"+ num + "N" , "" , "", true));
            StartCoroutine(_AWS.UpdatePlayer("P"+ num + "N" , Pname , true));
            
        }else{
            //名前表示
            for(int i = 0; i < 4; i ++){
                if(_AWS.Playername[i] != null)
                {
                    SetPName(i);
                }
            }
        }
    }

    void PlayerUpdate()
    {
        for(int i = 0 ; i < 4 ; i++)
        {
            SetPName(i);
        }
    }

    private void Update()
    {
        Debug.Log(_AWS.Game_State);
        if(_AWS.PlayerPre[0] == "true" && _AWS.PlayerPre[0] == "true" && _AWS.PlayerPre[1] == "true" && _AWS.PlayerPre[2] == "true" && _AWS.PlayerPre[3] == "true"){
            StartButton.SetActive(true);
        }
        if(_AWS.Game_State == "true")
        {
            SceneManager.LoadScene("Main"); 
        }
    }
}
