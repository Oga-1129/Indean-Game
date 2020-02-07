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
    // Start is called before the first frame update
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        //AWS通信
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        //SQLite操作用
        DBSrc = DB.GetComponent<SampleDataBase>(); 
        DBSrc.SelectDB();
        PlayerNum = DBSrc.num;
        thema = thema.GetComponent<TMP_InputField>();
        text = text.GetComponent<TextMeshProUGUI> ();
    }

    // Update is called once per frame
    public void OnClick()
    {
        Debug.Log("P" + PlayerNum + "Q");
        StartCoroutine(_AWS.UpdateDynamoDB("P" + PlayerNum + "Q", thema.text, true, 0));
    }

    void Update()
    {
        text.text = thema.text;
    }
}
