using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Amazon;
using TMPro;

public class GameStartButton : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;
    AWSConnector _AWS;
    public TextMeshProUGUI worning_text;

    // Start is called before the first frame update
    void Start()
    {
        DBSrc = DB.GetComponent<SampleDataBase>();  
        DBSrc.SelectDB();
        //AWS通信
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        //AWSConnectorのオブジェクト化
        _AWS = new AWSConnector();
        worning_text = worning_text.GetComponent<TextMeshProUGUI>(); 
    }

    public void OnClick()
    {
        SceneManager.LoadScene("Main"); 
        // if(_AWS.PlayerPre[0] == "true" &&  _AWS.PlayerPre[1] == "true" &&  _AWS.PlayerPre[2] == "true" &&  _AWS.PlayerPre[3] == "true")
        // {
            
        // }else{
        //     worning_text.text = "メンバーの準備がまだです";
        // }
    }

    void Update()
    {
        if(DBSrc.num != 1)
        {
            this.gameObject.SetActive (false);
        }
    }
}
