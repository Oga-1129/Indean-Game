using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpButton : MonoBehaviour
{
    public GameObject DB;
    SampleDataBase DBSrc;

    public GameObject SUName;
    SignUpName SUNSrc;
    // Start is called before the first frame update
    void Start()
    {
        DBSrc = DB.GetComponent<SampleDataBase>();   
        SUNSrc = SUName.GetComponent<SignUpName>();
    }

    public void onClick()
    {
        DBSrc.UpdateDB(SUNSrc.inputField.text, 1, 1);
        SceneManager.LoadScene("Session");   
    }
}
