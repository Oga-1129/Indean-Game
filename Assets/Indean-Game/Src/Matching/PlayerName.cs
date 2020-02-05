// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using Amazon;

// public class PlayerName : MonoBehaviour
// {
//     public int num;
//     public TextMeshProUGUI text;
//     string Pname;
//     AWSConnector _AWS;
//     // Start is called before the first frame update
//     void Start()
//     {
//         AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
//         _AWS = new AWSConnector();
//         text = text.GetComponent<TextMeshProUGUI> ();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         StartCoroutine(_AWS.GetDynamoDB(num));
//         Pname = _AWS.Pname[num - 1];
//         text.text = "Player" + num + Pname;
//     }
// }
