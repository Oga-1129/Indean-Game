using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SignUpName : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI text;
    public string signname;
    // Start is called before the first frame update
    void Start()
    {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<TMP_InputField> ();
        text = text.GetComponent<TextMeshProUGUI> ();
    }

    // Update is called once per frame
    void Update()
    {
        //テキストにinputFieldの内容を反映
        text.text = inputField.text;
        if(text.text == "" || text.text == "Guest")
        {
            text.text = "Guest";
        }
    }
}
