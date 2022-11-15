using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestSaveTitle : MonoBehaviour
{
    private string testFolder = "folder";
    private string testFile = "file";

    private string myText = "default";


    public Text title;

    private void Awake()
    {
        myText = PlayerPrefs.GetString("Title");

        title.text = myText;
    }


    public void ChooseTitle(string newTitle)
    {
        myText = newTitle;
        SaveData();
        title.text = myText;
    }


    private void SaveData()
    {
        PlayerPrefs.SetString("Title", myText);
        PlayerPrefs.Save();
    }
}
