using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using TMPro;

public class Matching : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;
    AWSConnector _AWS;
    string PlayerName;
    int num;

    public TextMeshProUGUI text;
    string Pname;
    // Start is called before the first frame update
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        _AWS = new AWSConnector();
        DBSrc = DB.GetComponent<SampleDataBase>();  
        DBSrc.SelectDB();
        PlayerName = DBSrc.PlayerName;
        Debug.Log(num);
        num++;
        StartCoroutine(_AWS.GetDynamoDB(num));
        StartCoroutine(_AWS.UpdateDynamoDB("P"+ num + "N" , PlayerName , true , num));

        text = text.GetComponent<TextMeshProUGUI> ();
    }

    // Update is called once per frame
    public void GetPName()
    {
        Debug.Log(num);
        StartCoroutine(_AWS.GetDynamoDB(num-1));
    }
    public void SetPName()
    {
        Pname = _AWS.Pname[num-1];
        text.text = "Player" + num + "    " + Pname;
    }

    public void updatesqldb(){
        num = _AWS.membernum;
        Debug.Log("N:" + _AWS.membernum);
        DBSrc.UpdateDB(PlayerName, 0 , num);
    }
}
