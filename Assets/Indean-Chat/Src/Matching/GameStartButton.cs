using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public void OnClick(){
        StartCoroutine(MoveRoom());
    }
    public IEnumerator MoveRoom()
    {
        Debug.Log(_AWS.Game_State);
        StartCoroutine(_AWS.UpdateState("GameState", "true", "",false));
        yield return new WaitForSeconds(2);
        StartCoroutine(_AWS.GetDynamoDBState(1));
        Debug.Log(_AWS.Game_State);
    }

    void Update()
    {
        if(DBSrc.num != 1)
        {
            this.gameObject.SetActive (false);
        }
    }
}
