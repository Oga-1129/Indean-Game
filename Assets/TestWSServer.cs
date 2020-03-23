using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using Amazon;

public class TestWSServer : MonoBehaviour
{
    WebSocketServer ws;

    // Start is called before the first frame update
    void Start()
    {
        //ポート番号を指定
        ws = new WebSocketServer(3000);
        //クライアントからの通信時の挙動を定義したクラス、「ExWebSocketBehavior」を登録
        ws.AddWebSocketService<ExWebSocketBehavior>("/");
        //サーバの起動
        ws.Start();
        Debug.Log("サーバ起動");
    }

    public void OnDestroy()
    {
        Debug.Log("サーバ停止");
        ws.Stop();
    }
}

public class ExWebSocketBehavior : WebSocketBehavior
{
    AWSConnector _AWS; 
    private void Start()
    {
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        _AWS = new AWSConnector();
    }

    //誰が現在接続しているのか管理するリスト
    public static List<ExWebSocketBehavior> clientList = new List<ExWebSocketBehavior>();
    //接続者に番号を振るための変数
    static int globalSeq = 0;
    //地震の番号
    int seq;
    string Name;

    //誰かがログインしてきた時に呼ばれるメソッド
    protected override void OnOpen()
    {
        //ログインしてきた人には、番号をつけて、リストに登録
        Name = _AWS.Playername[this.seq];
        globalSeq++;
        this.seq = globalSeq;
        clientList.Add(this);

        Debug.Log(this.Name + "     " + " Login.(" + this.ID + ")");

        //接続者全員にメッセージを送る
        foreach(var client in clientList)
        {
            client.Send(this.Name + "   " + " Login.");
        }
    }

    //誰かがメッセージを送信してきた時に呼ばれるメソッド
    protected override void OnMessage(MessageEventArgs e)
    {
        Debug.Log(this.Name + " " + "..." + e.Data);
        //接続者全員にメッセージを送る
        foreach(var client in clientList)
        {
            client.Send(this.Name + "   " + "..." + e.Data);
        }
    }

    //誰かがログアウトした時に呼ばれるメソッド
    protected override void OnClose(CloseEventArgs e)
    {
        Debug.Log("Seq" + this.seq + " Logout.(" + this.ID + ")");

        //ログアウトした人をリストから削除
        clientList.Remove(this);

        //接続者全員にメッセージを送る
        foreach(var client in clientList)
        {
            client.Send("Seq:" + seq + " Logout.");
        }
    }
}