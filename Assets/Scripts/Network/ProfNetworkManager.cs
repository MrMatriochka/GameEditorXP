using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ProfNetworkManager : MonoBehaviour
{
    [HideInInspector] public WebData[] groupData;
    public GameObject studentListLocation;
    public GameObject studentButtonPrefab;
    public SaveLoadLevel loader;

    private void Start()
    {
        StartCoroutine(AutoRefresh());
    }
    IEnumerator AutoRefresh()
    {
        RefreshButton();
        yield return new WaitForSeconds(30);
        StartCoroutine(AutoRefresh());
        yield return null;
    }
    IEnumerator GetGroupData(string groupName)
    {
        string uri = "https://api.studioxp.ca/items/game_editor_data?filter[group][_eq]=" + groupName;

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer tr4mfJ9rvQS9qucARPIQmuAQ61bf1Bgc");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Form download complete!");

                string jsonData = FixJson(request.downloadHandler.text);
                groupData = JsonHelper.FromJson<WebData>(jsonData);

                int tempChildCount = studentListLocation.transform.childCount;
                for (int i = 0; i < tempChildCount; i++)
                    {
                        Destroy(studentListLocation.transform.GetChild(0).gameObject);
                        yield return null;
                }

                int tempId = 0;
                foreach (WebData student in groupData)
                {
                    GameObject studentButton = Instantiate(studentButtonPrefab, studentListLocation.transform);
                    studentButton.GetComponent<StudentButton>().managerRef = GetComponent<ProfNetworkManager>();
                    studentButton.GetComponent<StudentButton>().id = tempId;
                    tempId++;
                }
            }
        }

    }
    public void RefreshButton()
    {
        StartCoroutine(GetGroupData("default"));
    }

    public void LoadLevel(int id)
    {
        SavedLevel dataToSave = groupData[id].data;
        SaveLoad<SavedLevel>.Save(dataToSave, "Level");
        loader.LoadData("Level");

    }

    IEnumerator DeleteGroupData(string groupName)
    {
        for (int i = 0; i < groupData.Length; i++)
        {
            string uri = "https://api.studioxp.ca/items/game_editor_data/" + groupData[i].id;

            using (UnityWebRequest request = UnityWebRequest.Delete(uri))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer tr4mfJ9rvQS9qucARPIQmuAQ61bf1Bgc");

                yield return request.SendWebRequest();

            }
            yield return null;
        }

        int tempChildCount = studentListLocation.transform.childCount;
        for (int i = 0; i < tempChildCount; i++)
        {
            Destroy(studentListLocation.transform.GetChild(0).gameObject);
            yield return null;
        }

        int tempId = 0;
        foreach (WebData student in groupData)
        {
            GameObject studentButton = Instantiate(studentButtonPrefab, studentListLocation.transform);
            studentButton.GetComponent<StudentButton>().managerRef = GetComponent<ProfNetworkManager>();
            studentButton.GetComponent<StudentButton>().id = tempId;
            tempId++;
        }
    }
    public void DeleteButton()
    {
        StartCoroutine(DeleteGroupData("default"));
    }

    string FixJson(string value)
    {
        value = value.Remove(0,9);
        value = value.Remove((value.Length - 3), 2);
        value = "{\"Items\":[" + value + "]}";
        return value;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
