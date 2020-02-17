using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Amazon;

public class DecsionQuestion : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;
    AWSConnector _AWS;
    int PlayerNum;
    public TMP_InputField thema;
    public TextMeshProUGUI text;
    public TextMeshProUGUI worning_text;
    public TextMeshProUGUI updatetext;
    // Start is called before the first frame update
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        //AWS通信
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        _AWS = new AWSConnector();

        //SQLite操作用
        DBSrc = DB.GetComponent<SampleDataBase>(); 
        DBSrc.SelectDB();
        PlayerNum = DBSrc.num;
        thema = thema.GetComponent<TMP_InputField>();
        text = text.GetComponent<TextMeshProUGUI> ();
        updatetext = updatetext.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void OnClick()
    {

        worning_text.text = "";
        string s = thema.text;
        int length = s.Length;
        if(length >= 3){
            updatetext.text = "テーマの登録中・・・";
            StartCoroutine(_AWS.UpdatePlayer("P" + PlayerNum + "Q", thema.text,false));
        }else{
            worning_text.text = "3文字以上で設定してください。";
        }
    }

    void Update()
    {
        text.text = thema.text;
    }
}
