using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image : MonoBehaviour
{
    string url = "";

    // Start is called before the first frame update
    [System.Obsolete]
    IEnumerator Sample()
    {
        WWW www = new WWW(url);

        // 画像ダウンロード完了を待機
        yield return www;

        // webサーバから取得した画像をRawImagで表示する
        RawImage rawImage = GetComponent<RawImage>();
        rawImage.texture = www.textureNonReadable;
        
    }

    public void seturl(){
        // wwwクラスのコンストラクタに画像URLを指定
        url = "https://co-test-aws.s3-ap-northeast-1.amazonaws.com/co-test-aws/test/nyanko.jpg";
        StartCoroutine ("Sample");
    }
}
