using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartClick : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;
    public TextMeshProUGUI pname;

    // Start is called before the first frame update
    void Start()
    {
        DBSrc = DB.GetComponent<SampleDataBase>();  
        DBSrc.SelectDB();
        if(DBSrc.PlayerName != null) pname.text = "Name : " + DBSrc.PlayerName;
    }

    // Update is called once per frame
    public void OnClick()
    {
        if(DBSrc.PlayerName == "Guest" || DBSrc.PlayerName == ""){
            SceneManager.LoadScene("Sign Up"); 
        }else{
            SceneManager.LoadScene("Session"); 
        }
    }

}
