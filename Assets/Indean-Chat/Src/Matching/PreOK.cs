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
        StartCoroutine(_AWS.GetDynamoDBPlayer(1));
    }

    public void OnClick()
    {
        Debug.Log(_AWS.myThema[DBSrc.num-1]);
        if(_AWS.myThema[DBSrc.num-1] != " "){
            if(_AWS.PlayerPre[DBSrc.num-1] == "true")
            {
                StartCoroutine(_AWS.UpdatePlayer("P"+ DBSrc.num + "Pre", "false",false));
                text.text = "準備完了";
            }else if(_AWS.PlayerPre[DBSrc.num-1] == "false")
            {
                StartCoroutine(_AWS.UpdatePlayer("P"+ DBSrc.num + "Pre", "true",false));
                text.text = "やり直し";
            }
        }else{
            StartCoroutine(worningtext());
        }
    }

    IEnumerator worningtext(){
        worning_text.text = "テーマが未設定です。";
        yield return new WaitForSeconds(2);
        worning_text.text = "";
    }
}
