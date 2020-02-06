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

    public TextMeshProUGUI text;
    string Pname;

    // Start is called before the first frame update
    void Start()
    {
        //AWS通信
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        //AWSConnectorのオブジェクト化
        _AWS = new AWSConnector();

        //SQLite操作用
        DBSrc = DB.GetComponent<SampleDataBase>();  
        DBSrc.SelectDB();

        //SQLiteに保存してあるユーザー名
        Pname = DBSrc.PlayerName;

        //AWSのデータベースに保存されてある名前データの取得
        Debug.Log("num:" + num + "\n");
        StartCoroutine(_AWS.GetDynamoDB());
        num++;

        //今回登録するユーザー名
        Debug.Log("Pname : " + Pname + "\n");

        //新規ユーザーの登録
        StartCoroutine(_AWS.UpdateDynamoDB("P"+ num + "N" , Pname , true , num));


        //テキスト操作用
        text = text.GetComponent<TextMeshProUGUI> ();
    }

    // Update is called once per frame
    public void GetPName()
    {
        StartCoroutine(_AWS.GetDynamoDB());
    }

    //取得したプレイヤー名の表示
    public void SetPName(int number)
    {
        Debug.Log(_AWS.Playername[number]);
        text.text += "Player" + (number+1) + "    " + Pname + "\n";
    }

    //SQLiteのアップデート
    public void updatesqldb(){
        num = _AWS.membernum;
        DBSrc.UpdateDB(Pname, 0 , num);
    }

    //既に登録されてあるユーザーの表示
    public void ReName()
    {
        StartCoroutine(_AWS.GetDynamoDB());
    }

    public void setReName()
    {
        for(int i = 0; i < 4; i ++){
            Pname = _AWS.Playername[i];
            Debug.Log("Pname :" + Pname + "\n");
            if(Pname != ""){
                Debug.Log("num :" + i);
                SetPName(i);
            }
        }
    }
}
