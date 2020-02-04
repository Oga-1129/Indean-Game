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
        Debug.Log(DBSrc.PlayerName);
    }

    // Update is called once per frame
    public void OnClick()
    {
        Debug.Log(DBSrc.PlayerName);
        if(DBSrc.PlayerName == "Guest"){
            Debug.Log("Sign Up");
            SceneManager.LoadScene("Sign Up"); 
        }else{
            Debug.Log("Session");
            SceneManager.LoadScene("Session"); 
        }
    }

}
