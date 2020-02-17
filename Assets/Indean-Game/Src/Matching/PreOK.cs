using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using TMPro;

public class PreOK : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;
    AWSConnector _AWS;
    public TextMeshProUGUI text;
    public TextMeshProUGUI worning_text;
    InputChat _inputchat;
    Matching _matching;
    public GameObject matching;
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

        text = text.GetComponent<TextMeshProUGUI>();
        worning_text = worning_text.GetComponent<TextMeshProUGUI>(); 

        _matching = matching.GetComponent<Matching>();
        StartCoroutine(_AWS.GetDynamoDB(1));
    }

    public void OnClick()
    {
        if(_AWS.PlayerPre[DBSrc.num-1] == "true")
        {
            StartCoroutine(_AWS.UpdateDynamoDB("P"+ DBSrc.num + "Pre", "false",0, "", false));
            text.text = "準備完了";
        }else if(_AWS.PlayerPre[DBSrc.num-1] == "false")
        {
            StartCoroutine(_AWS.UpdateDynamoDB("P"+ DBSrc.num + "Pre", "true",  0, "", false));
            text.text = "やり直し";
        }
    }
}
