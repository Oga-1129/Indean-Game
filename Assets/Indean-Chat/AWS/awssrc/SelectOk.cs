using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOk : MonoBehaviour
{
    public GameObject DDButton;
    dropdown ddsrc;

    public GameObject test_aws;
    Test testscript;



    void Start()
    {
        ddsrc = DDButton.GetComponent<dropdown>();
        testscript = test_aws.GetComponent<Test>();
    }

    public void onClick()
    {
        testscript.AWScontroller(ddsrc.selectNum);
    }
}
