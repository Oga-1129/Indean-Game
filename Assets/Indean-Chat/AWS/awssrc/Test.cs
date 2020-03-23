// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Amazon;
// using UnityEngine.UI;
// using TMPro;

// public class Test : MonoBehaviour
// {
//     AWSConnector _AWS;
//     public TextMeshProUGUI ResultText = null;

//     public RawImage downobj;

//     public GameObject imagebox;

//     Image imagesrc;

//     public TMP_InputField inputid;
//     public TMP_InputField inputusername;

//     // Start is called before the first frame update
//     void Start()
//     {
//         UnityInitializer.AttachToGameObject(this.gameObject);
//         AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
//         _AWS = new AWSConnector ();
//         imagesrc = imagebox.GetComponent<Image>();
//     }

//     // Update is called once per frame
//     public void AWScontroller(int selectNum)
//     {
//         // switch(selectNum){
//         //     case 0:
//         //         Debug.Log("指定ファイルをS3バケットにアップロード");
//         //         string inputFileFullPath = "/Users/ogatafutoshikawa/Desktop/AWS/test.jpg";
//         //         string uploadFileToS3 = "co-test-aws/test/nyanko.jpg";
//         //         _AWS.uploadFileToS3(ResultText, inputFileFullPath, uploadFileToS3);
//         //         break;
            
//         //     case 1:
//         //         Debug.Log("指定ファイルをS3バケットからダウンロード");
//         //         _AWS.downloadFileToS3(ResultText);
//         //         imagesrc.seturl();
//         //         break;

//         //     case 2:
//         //         // Debug.Log("値をDynamoDBに生成");
//         //         // StartCoroutine(_AWS.CreateDynamoDB(ResultText, inputid, inputusername));
//         //         // break;

//         //     case 3:
//         //         Debug.Log("値をDynamoDBから取得");
//         //         StartCoroutine(_AWS.GetDynamoDB(ResultText, inputid, inputusername));
//         //         break;

//         //     case 4:
//         //         Debug.Log("値をDynamoDBから更新");
//         //         StartCoroutine(_AWS.UpdateDynamoDB(ResultText, inputid, inputusername));
//         //         break;

//         //     case 5:
//         //         Debug.Log("値をDynamoDBから削除");
//         //         StartCoroutine(_AWS.DeleteDynamoDB(ResultText, inputid, inputusername));
//         //         break;
//         //}
//     }
// }
