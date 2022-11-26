using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedLevel
{
    public SavedLevel()
    {
        List<int> index = new List<int>();
        List<Vector3> positions =new List<Vector3>(); ;
        List<string> prefabs = new List<string>(); ;
    }

    public List<int> index = new List<int>();
    public List<Vector3> positions = new List<Vector3>();
    public List<string> prefabs = new List<string>();
}

public class SaveLoadLevel : MonoBehaviour
{
    public Dictionary<string, GameObject> prefabByName = new Dictionary<string, GameObject>();
    public List<GameObject> prefabsList = new List<GameObject>();

    private string testFolder = "Save";
    private string testFile = "Level";

    private List<Vector3> myPositions = new List<Vector3>();
    private List<string> myPrefabs = new List<string>();
    private List<int> myIndex = new List<int>();

    private BuildingManager manager;

    void Awake()
    {
        manager = gameObject.GetComponent<BuildingManager>();
        Initialize();
    }


    public void SaveData()
    {
        myPositions.Clear();
        myPrefabs.Clear();
        myIndex.Clear();

        int newIndex = 0;
        foreach (GameObject obj in manager.placedObject)
        {
            myPositions.Add(obj.transform.position);       
            myPrefabs.Add(obj.name.Replace("(Clone)", string.Empty));
            myIndex.Add(newIndex);
            newIndex++;
        }

        SavedLevel dataToSave = new SavedLevel
        {
            positions = myPositions,
            prefabs = myPrefabs,
            index = myIndex
        };

        SaveLoad<SavedLevel>.Save(dataToSave, testFile);
    }

    public void LoadData()
    {
        foreach (GameObject obj in manager.placedObject)
        {
            Destroy(obj);
        }
        manager.placedObject.Clear();


        SavedLevel loadedData = SaveLoad<SavedLevel>.Load(testFile) ?? new SavedLevel();

        myPositions = loadedData.positions;
        myPrefabs = loadedData.prefabs;
        myIndex = loadedData.index;

        foreach (int i in myIndex)
        {
            GameObject myObject = Instantiate(GetPrefab(myPrefabs[i]));
            manager.placedObject.Add(myObject);
            myObject.transform.position = myPositions[i];
        }

        if(manager.placedObject.Count == 0)
        {
            GameObject myObject = Instantiate(GetPrefab("Player"));
            manager.placedObject.Add(myObject);
            myObject.transform.position = new Vector3(-9, 0, 0);
        }
    }

    void Initialize()
    {
        foreach (GameObject prefab in prefabsList)
        {
            if (!prefabByName.ContainsKey(prefab.name))
            {
                prefabByName.Add(prefab.name, prefab);
            }
        }
    }

    public GameObject GetPrefab(string prefabName)
    {
        if (prefabByName.ContainsKey(prefabName))
        {
            return prefabByName[prefabName];
        }

        Debug.LogWarning("There is not inventory ui for " + prefabName);
        return null;
    }
}
