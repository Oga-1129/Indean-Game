﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class InputChat : MonoBehaviour
{
  //オブジェクトと結びつける
    public TMP_InputField inputField;
    public TextMeshProUGUI text;

    int HH;
    int  MM;
    int SS;

    string Name;
    string PlayerName;

    void Start () {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<TMP_InputField> ();
        text = text.GetComponent<TextMeshProUGUI> ();
    }

    public void InputText(){
        DateTime dt = DateTime.Now;
        HH = dt.Hour;
        MM = dt.Minute;
        SS = dt.Second;
        string[] testtext = new string[2];

        PlayerName = "root";
        Name = "<color=red><size=150%><margin=0.5em>" + PlayerName + "</size></color>";

        testtext[0] = "<indent=25%>" + inputField.text + "</indent>";

        testtext[1] = "<color=red><size=70%><margin=1em>" + string.Format( "{0:D2}:{1:D2}:{2:D2}", HH , MM , SS) + "</size></color>";


        //テキストにinputFieldの内容を反映
        text.text += "<size=150%>\n</size>" + Name  + testtext[0] + "\n" +  testtext[1];
        inputField.text = "";
    }
}
