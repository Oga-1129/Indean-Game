using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Amazon;
using WebSocketSharp;


public class InputChat : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;


    //オブジェクトと結びつける
    public TMP_InputField inputField;
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI startup_text;
    public TextMeshProUGUI[] _playername_list = new TextMeshProUGUI[4];

    int HH;
    int  MM;
    int SS;

    string Name;
    string PlayerName;

    int saveStatementID;
    string namecolor = "black";
    AWSConnector _AWS; 

    public WebSocket ws;
    public Button sendButton;

    void Start () {
        //接続処理。接続先サーバと、ポート番号を指定する
        ws = new WebSocket("ws://localhost:3000/");
        ws.Connect();

        //送信ボタンが押されたときに実行する処理「SendText」を登録する
        sendButton.onClick.AddListener(SendText);
        //サーバからメッセージを受信したときに実行する処理「RecvText」を登録する
        ws.OnMessage += (sender, e) => RecvText(e.Data);
        //サーバとの接続が切れたときに実行する処理「RecvClose」を登録する
        ws.OnClose += (sender, e) => RecvClose();


        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        //Componentを扱えるようにする
        DBSrc = DB.GetComponent<SampleDataBase>();   
        // inputField = inputField.GetComponent<TMP_InputField> ();
        // ChatText = text.GetComponent<TextMeshProUGUI> ();
        DBSrc.SelectDB();
        PlayerName = DBSrc.PlayerName;
        DBSrc.UpdateDB(PlayerName, "true" , DBSrc.num);
        _AWS = new AWSConnector();
        StartCoroutine(_AWS.GetDynamoDBPlayer(0));
        StartCoroutine(_AWS.GetDynamoDBState());
        startup_text.text = "準備中・・・";
        for(int i = 0; i < 4; i++)
        {
            _playername_list[i] = _playername_list[i].GetComponent<TextMeshProUGUI>();
        }
    }

    //サーバーへ、メッセージを送信する
    public void SendText()
    {
        ws.Send(inputField.text);
    }

    //サーバーから受け取ったメッセージを、ChatTextに表示する
    public void RecvText(string text)
    {
        DateTime dt = DateTime.Now;
        HH = dt.Hour;
        MM = dt.Minute;
        SS = dt.Second;
        string[] testtext = new string[2];
        string Thema;
        if(inputField.text.Contains(_AWS.PlayerThema[DBSrc.num-1]))
        {
            ChatText.text += "\n<align=center>！NGワード！</align>";
            DBSrc.UpdateDB(PlayerName, "false" , DBSrc.num);
            StartCoroutine(_AWS.UpdatePlayer("P" + (DBSrc.num-1) + "St", "false",  false));
            Thema = _AWS.PlayerThema[DBSrc.num-1];
        }else{
            Thema = "******";
        }
        if (DBSrc.state == "false"){
            namecolor = "red";
        }
        Name = "<color=" + namecolor + "><size=150%><margin=0.5em>" + _AWS.talkname + "</size></color>";
        testtext[0] = "<color=" + namecolor + "><indent=25%>" + text + "</indent></color>";
        testtext[1] = "<color=red><size=70%><margin=1em>" + string.Format( "{0:D2}:{1:D2}:{2:D2}", HH , MM , SS) + "</size></color>";
        ChatText.text += (text + "\n");
        inputField.text = "";
    }

    //サーバーとの接続が切れた時のメッセージを、ChatTextに表示する
    public void RecvClose()
    {
        ChatText.text = ("Close.");
    }

    // public void SetText()
    // {
    //     StartCoroutine(_AWS.UpdateState("Remarks", inputField.text, PlayerName,false));
    // }

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
        GetStatementID();
    }

    public void GetStatementID(){
        StartCoroutine(_AWS.GetDynamoDBState());
    }

    void Update()
    {
        if(_AWS.StatementID != saveStatementID)
        {
            saveStatementID = _AWS.StatementID;
        }
    }
}
