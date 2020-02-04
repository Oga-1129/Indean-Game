using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onClick()
    {
        Debug.Log("in");
        SceneManager.LoadScene("Main");   
    }
}
