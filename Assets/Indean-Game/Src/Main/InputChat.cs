using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Amazon;


public class InputChat : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;


    //オブジェクトと結びつける
    public TMP_InputField inputField;
    public TextMeshProUGUI text;
    public TextMeshProUGUI startup_text;
    public TextMeshProUGUI[] _playername_list = new TextMeshProUGUI[4];

    int HH;
    int  MM;
    int SS;

    string Name;
    string PlayerName;

    int savetalkid;
    string namecolor = "black";
    AWSConnector _AWS; 

    void Start () {
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        //Componentを扱えるようにする
        DBSrc = DB.GetComponent<SampleDataBase>();   
        inputField = inputField.GetComponent<TMP_InputField> ();
        text = text.GetComponent<TextMeshProUGUI> ();
        DBSrc.SelectDB();
        PlayerName = DBSrc.PlayerName;
        DBSrc.UpdateDB(PlayerName, 1 , DBSrc.num);
        _AWS = new AWSConnector();
        StartCoroutine(_AWS.GetDynamoDB(0));
        startup_text.text = "準備中・・・";
        for(int i = 0; i < 4; i++)
        {
            _playername_list[i] = _playername_list[i].GetComponent<TextMeshProUGUI>();
        }
    }

    public void SetText()
    {
        StartCoroutine(_AWS.UpdateDynamoDB("Remarks", inputField.text, true, 0, PlayerName));
    }

    public void startup()
    {
        startup_text.text = "";
        for(int i = 0 ; i < 4 ; i++)
        {
            if(i != DBSrc.num-1){
                _playername_list[i].text += "\n" + _AWS.Playername[i] + "\n" +　"<size=60%>" + _AWS.PlayerThema[i] + "</size>";
            }else{
                _playername_list[i].text += "\n" + _AWS.Playername[i] + "\n" +　"******";
            }
        }
        GetTalkID();
    }

    public void InputText(){
        DateTime dt = DateTime.Now;
        HH = dt.Hour;
        MM = dt.Minute;
        SS = dt.Second;
        string[] testtext = new string[2];
        string Thema;

        if(inputField.text.Contains(_AWS.PlayerThema[DBSrc.num-1]))
        {
            text.text += "\n<align=center>！NGワード！</align>";
            DBSrc.UpdateDB(PlayerName, 0 , DBSrc.num);
            Thema = _AWS.PlayerThema[DBSrc.num-1];
        }else{
            Thema = "******";
        }

        if (DBSrc.state == 0){
            namecolor = "red";
        }
        Name = "<color=" + namecolor + "><size=150%><margin=0.5em>" + _AWS.talkname + "</size></color>";

        testtext[0] = "<color=" + namecolor + "><indent=25%>" + _AWS.talk + "</indent></color>";

        testtext[1] = "<color=red><size=70%><margin=1em>" + string.Format( "{0:D2}:{1:D2}:{2:D2}", HH , MM , SS) + "</size></color>";
        
        _playername_list[DBSrc.num-1].text = "Player" + DBSrc.num + "\n" + _AWS.Playername[DBSrc.num-1] + "\n" +　Thema;

        //テキストにinputFieldの内容を反映
        text.text += "<size=150%>\n</size>" + Name  + testtext[0] + "\n" +  testtext[1];
        inputField.text = "";
    }

    public void GetTalkID(){
        StartCoroutine(_AWS.GetDynamoDB(1));
    }

    void Update()
    {
        if(_AWS.talkid != savetalkid)
        {
            savetalkid = _AWS.talkid;
            InputText();
        }
    }
}
