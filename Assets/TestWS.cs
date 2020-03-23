using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using TMPro;

public class TestWS : MonoBehaviour
{
    public WebSocket ws;
    public TextMeshProUGUI chatText;
    public Button sendButton;
    public TMP_InputField messageInput;

    public string wsurl;

    //サーバーへ、メッセージを送信する
    public void SendText()
    {
        ws.Send(messageInput.text);
    }

    //サーバーから受け取ったメッセージを、ChatTextに表示する
    public void RecvText(string text)
    {
        chatText.text += (text + "\n");
    }

    //サーバーとの接続が切れた時のメッセージを、ChatTextに表示する
    public void RecvClose()
    {
        chatText.text = ("Close.");
    }


    void Start()
    {
        //接続処理。接続先サーバと、ポート番号を指定する
        ws = new WebSocket("ws://localhost:3000/");
        ws.Connect();

        //送信ボタンが押されたときに実行する処理「SendText」を登録する
        sendButton.onClick.AddListener(SendText);
        //サーバからメッセージを受信したときに実行する処理「RecvText」を登録する
        ws.OnMessage += (sender, e) => RecvText(e.Data);
        //サーバとの接続が切れたときに実行する処理「RecvClose」を登録する
        ws.OnClose += (sender, e) => RecvClose();

        // ws.OnOpen += (o, e) => { print("Open"); };
        // ws.OnError += (o, e) => { print(e.Message); };
        // ws.OnMessage += (o, e) => { print(e.Data); };
        // ws.Connect();
    }

    // void Update()
    // {
    //     if (Input.GetKeyUp("s"))
    //     {
    //         ws.Send("Test");
    //     }
    // }

    // void OnDestroy()
    // {
    //     ws.Close();
    //     ws = null;
    // }
}