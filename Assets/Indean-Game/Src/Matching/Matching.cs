using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using TMPro;

public class Matching : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;
    AWSConnector _AWS;
    int num;

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
        _AWS.GetDynamoDB(0);

        //SQLite操作用
        DBSrc = DB.GetComponent<SampleDataBase>();  
        DBSrc.SelectDB();

        //SQLiteに保存してあるユーザー名
        Pname = DBSrc.PlayerName;

        //AWSのデータベースに保存されてある名前データの取得
        StartCoroutine(_AWS.GetDynamoDB(0));

        //テキスト操作用
        text = text.GetComponent<TextMeshProUGUI> ();
        text.text = "<size=60%>データ登録中・・・</size>";
        for(int i = 0; i< 4; i++)
        {
            playertext[i] = playertext[i].GetComponent<TextMeshProUGUI>();
        }
    }


    public void GetPName()
    {
        //text.text = "<size=60%>データ取得中・・・</size>";
        StartCoroutine(_AWS.GetDynamoDB(1));
    }


    //取得したプレイヤー名の表示
    public void SetPName(int number)
    {
        playertext[number].text = "Player" + (number+1) + "    " + Pname + "\n";
    }


    //SQLiteのアップデート
    public void updatesqldb()
    {
        if(_AWS.Playername[DBSrc.num-1] != Pname){
            num = _AWS.membernum;
            text.text = "<size=60%>プレイヤーのデータ更新中・・・</size>";
            DBSrc.UpdateDB(Pname, 0 , num);
        }
    }

    //既に登録されてあるユーザーの表示
    public void setReName()
    {
        text.text = "";
        for(int i = 0; i < 4; i ++){
            Pname = _AWS.Playername[i];
            if(Pname != null){
                SetPName(i);
            }
        }
    }


    public void Register()
    {
        Debug.Log("DBSrc.num :" + DBSrc.num);
        Debug.Log("_AWS.Playername[DBSrc.num] :" + _AWS.Playername[DBSrc.num-1]);
        Debug.Log("Pname:" + Pname);


        if(_AWS.Playername[DBSrc.num-1] != Pname){
            num++;
            //新規ユーザーの登録
            StartCoroutine(_AWS.UpdateDynamoDB("P"+ num + "N" , Pname , true , num, ""));
        }else{
            setReName();
        }
    }

    void PlayerUpdate()
    {
        for(int i = 0 ; i < 4 ; i++)
        {
            SetPName(i);
        }
    }
}
