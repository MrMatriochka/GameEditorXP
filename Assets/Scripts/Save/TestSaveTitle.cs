using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TestClass
{
    public TestClass()
    {
        string testData;
    }

    public string testData = "default";
}

public class TestSaveTitle : MonoBehaviour
{
    private string testFolder = "folder";
    private string testFile = "file";

    private string myText = "default";


    public Text title;

    private void Awake()
    {
        // the ?? is a null comparison so if the value returns null, it generates new data
        TestClass loadedData = SaveLoad<TestClass>.Load(testFolder, testFile) ?? new TestClass();

        myText = loadedData.testData;

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
        TestClass dataToSave = new TestClass
        {
            testData = myText
        };

        SaveLoad<TestClass>.Save(dataToSave, testFolder, testFile);
    }
}
