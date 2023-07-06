using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class WebData
{
    public string id = "";
    public string group = "";
    public string username = "";
    public string data = "";
}

public class NetworkManager : MonoBehaviour
{
    static string myId;
    static string myUsername;

    private void Start()
    {
       if(PlayerPrefs.HasKey("myId"))
        {
            myId = PlayerPrefs.GetString("myId");
        }
        if(PlayerPrefs.HasKey("myUsername"))
        {
            myUsername = PlayerPrefs.GetString("myUsername");
        }
    }
    public void SetUsername(TMP_InputField inputField)
    {
        myUsername = inputField.text;
        PlayerPrefs.SetString("myUsername", myUsername);
    }
    IEnumerator Upload(string levelSave)
    {
        WebData dataToSave = new WebData
        {
            id = "",
            group = "default",
            username = myUsername,
            data = levelSave
        };
        string jsonData = JsonUtility.ToJson(dataToSave);
        string uri = "https://api.studioxp.ca/items/game_editor_data";

        using (UnityWebRequest request = new UnityWebRequest(uri, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer 3prCBvKlJncjeXjAOROUBQZ3qIUCMHDQ");
            byte[] rawData = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(rawData);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Form upload complete!");

                jsonData = request.downloadHandler.text;
                jsonData = jsonData.Remove(0, 8);
                jsonData = jsonData.Remove((jsonData.Length - 1), 1);

                WebData returnedData = JsonUtility.FromJson<WebData>(jsonData);
                myId = returnedData.id;
                PlayerPrefs.SetString("myId", myId);
            }
        } 
    }
    public void ButtonUpload(string levelSave)
    {
        StartCoroutine(Upload(levelSave));
    }
   
    IEnumerator GetIdByUsername()
    {
        string uri = "https://api.studioxp.ca/items/game_editor_data?filter[username][_eq]=default";

        using (UnityWebRequest request =UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer 3prCBvKlJncjeXjAOROUBQZ3qIUCMHDQ");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Form download complete!");
                print(request.downloadHandler.text);
                string jsonData = FixJson(request.downloadHandler.text);
                WebData returnedData = JsonUtility.FromJson<WebData>(jsonData);
                myId = returnedData.id;
            }
        }
        
    }
    public void ButtonGetId()
    {
        StartCoroutine(GetIdByUsername());
    }

    string FixJson(string value)
    {
        value = value.Remove(0,9);
        value = value.Remove((value.Length-3), 2);
        return value;
    }

    IEnumerator UpdateData(string levelSave)
    {
        WebData dataToSave = new WebData
        {
            id = myId,
            group = "default",
            username = myUsername,
            data = levelSave
        };
        string jsonData = JsonUtility.ToJson(dataToSave);
        string uri = "https://api.studioxp.ca/items/game_editor_data/"+myId;

        using (UnityWebRequest request = new UnityWebRequest(uri, "PATCH"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer 3prCBvKlJncjeXjAOROUBQZ3qIUCMHDQ");
            byte[] rawData = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(rawData);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
    public void ButtonUpdate(string levelSave)
    {
        StartCoroutine(UpdateData(levelSave));
    }
}
