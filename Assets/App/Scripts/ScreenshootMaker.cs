using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ScreenshootMaker : MonoBehaviour
{
    [SerializeField] private float _delay = 0.2f;

    string pathToYourFile = @"C:\Screenshots\";
    //this is the name of the file
    string fileName = "filename";
    //this is the file type
    string fileType = ".png";

    private float _timer; 

    private int CurrentScreenshot { get => PlayerPrefs.GetInt("ScreenShot"); set => PlayerPrefs.SetInt("ScreenShot", value); }

    private void Start() 
    {
        _timer = 0;
    }

    
    private void Update() 
    {
        _timer += Time.deltaTime;

        if(_timer >= _delay)
        {
             string date = System.DateTime.Now.ToString();
            date = date.Replace("/","-");
            date = date.Replace(" ","_");
            date = date.Replace(":","-");

            UnityEngine.ScreenCapture.CaptureScreenshot(Application.dataPath + "/ScreenShots/SS_"+ date +".png", 1);

            //UnityEngine.ScreenCapture.CaptureScreenshot(pathToYourFile + fileName + CurrentScreenshot + fileType);
            //CurrentScreenshot++;

            _timer = 0;
        }
    }
}

