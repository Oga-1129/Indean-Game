using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Amazon;
using TMPro;
using System;

public class CreateSession : MonoBehaviour
{
    AWSConnector _AWS; 
    public TMP_InputField inputid;
    int roomid;
    // Start is called before the first frame update
    void Start()
    {
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        _AWS = new AWSConnector();
    }

    // Update is called once per frame
    public void OnClick()
    {
        Int32.TryParse(inputid.text, out roomid);
        StartCoroutine(_AWS.CreateDynamoDB(roomid));
    }
}
