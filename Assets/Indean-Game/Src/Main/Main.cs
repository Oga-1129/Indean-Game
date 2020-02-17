using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;

public class Main : MonoBehaviour
{
    AWSConnector _AWS;
    // Start is called before the first frame update
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        //AWSConnectorのオブジェクト化
        _AWS = new AWSConnector();
    }
}
