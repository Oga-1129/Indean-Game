using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartClick : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;
    // Start is called before the first frame update
    void Start()
    {
        DBSrc = DB.GetComponent<SampleDataBase>();  
        DBSrc.SelectDB();
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
