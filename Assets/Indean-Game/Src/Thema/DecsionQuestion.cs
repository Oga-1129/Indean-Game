using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        //SQLite操作用
        DBSrc = DB.GetComponent<SampleDataBase>(); 
        PlayerNum = DBSrc.num;
        _AWS = new AWSConnector();
        thema = thema.GetComponent<TMP_InputField>();
        text = text.GetComponent<TextMeshProUGUI> ();
    }

    // Update is called once per frame
    public void OnClick()
    {
        _AWS.UpdateDynamoDB("P" + PlayerNum + "Q", thema.text, true, 0);
        SceneManager.LoadScene("Matching"); 
    }

    void update()
    {
        text.text = thema.text;
    }
}
