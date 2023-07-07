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
        List<Quaternion> rotations = new List<Quaternion>();
        List<Vector3> scales = new List<Vector3>();
        List<string> prefabs = new List<string>();
    }

    public List<int> index = new List<int>();
    public List<Vector3> positions = new List<Vector3>();
    public List<Quaternion> rotations = new List<Quaternion>();
    public List<Vector3> scales = new List<Vector3>();
    public List<string> prefabs = new List<string>();
}

public class SaveLoadLevel : MonoBehaviour
{
    public Dictionary<string, GameObject> prefabByName = new Dictionary<string, GameObject>();
    public List<GameObject> prefabsList = new List<GameObject>();

    //private string testFolder = "Save";
    private string testFile = "Level";

    private List<Vector3> myPositions = new List<Vector3>();
    private List<Quaternion> myRotations = new List<Quaternion>();
    private List<Vector3> myScales = new List<Vector3>();
    private List<string> myPrefabs = new List<string>();
    private List<int> myIndex = new List<int>();

    private BuildingManager manager;
    private NetworkManager network;

    void Awake()
    {
        manager = gameObject.GetComponent<BuildingManager>();
        network = gameObject.GetComponent<NetworkManager>();
        Initialize();
    }

    private void Start()
    {
        StartCoroutine(AutoSave());
    }

    IEnumerator AutoSave()
    {
        SaveData("Level");
        yield return new WaitForSeconds(30);
        StartCoroutine(AutoSave());
        yield return null;
    }
    public void SaveData(string filename)
    {
        myPositions.Clear();
        myRotations.Clear();
        myScales.Clear();
        myPrefabs.Clear();
        myIndex.Clear();

        int newIndex = 0;
        foreach (GameObject obj in manager.placedObject)
        {
            myPositions.Add(obj.transform.position);
            myRotations.Add(obj.transform.rotation);
            myScales.Add(obj.transform.localScale);
            myPrefabs.Add(obj.name.Replace("(Clone)", string.Empty));
            myIndex.Add(newIndex);
            newIndex++;
        }

        SavedLevel dataToSave = new SavedLevel
        {
            positions = myPositions,
            prefabs = myPrefabs,
            rotations = myRotations,
            scales = myScales,
            index = myIndex
        };

        SaveLoad<SavedLevel>.Save(dataToSave, filename);

        if(!network.isProf)
        {
            if (PlayerPrefs.HasKey("myId"))
            {
                network.ButtonUpdate(dataToSave);
            }
            else
            {
                network.ButtonUpload(dataToSave);
            }
        }
    }

    public void LoadData(string filename)
    {
        foreach (GameObject obj in manager.placedObject)
        {
            Destroy(obj);
        }
        manager.placedObject.Clear();

        if(PlayerPrefs.HasKey(filename))
        {
            SavedLevel loadedData = SaveLoad<SavedLevel>.Load(filename) ?? new SavedLevel();

            myPositions = loadedData.positions;
            myRotations = loadedData.rotations;
            myScales = loadedData.scales;
            myPrefabs = loadedData.prefabs;
            myIndex = loadedData.index;

            foreach (int i in myIndex)
            {
                GameObject myObject = Instantiate(GetPrefab(myPrefabs[i]));
                manager.placedObject.Add(myObject);
                myObject.transform.position = myPositions[i];
                myObject.transform.rotation = myRotations[i];
                myObject.transform.localScale = myScales[i];
            }
        }
        

        //default
        if(manager.placedObject.Count == 0)
        {
            //player
            GameObject myObject = Instantiate(GetPrefab("Player"));
            manager.placedObject.Add(myObject);
            myObject.transform.position = new Vector3(-7.75f, -4, 0);

            //finish
            myObject = Instantiate(GetPrefab("Finish"));
            manager.placedObject.Add(myObject);
            myObject.transform.position = new Vector3(7.75f, -4f, 0);

            //platforms
            myObject = Instantiate(GetPrefab("PlatformDirtL"));
            manager.placedObject.Add(myObject);
            myObject.transform.position = new Vector3(-7.5f, -5.5f, 0);

            myObject = Instantiate(GetPrefab("PlatformDirtL"));
            manager.placedObject.Add(myObject);
            myObject.transform.position = new Vector3(-4.5f, -5.5f, 0);

            myObject = Instantiate(GetPrefab("PlatformDirtL"));
            manager.placedObject.Add(myObject);
            myObject.transform.position = new Vector3(-1.5f, -5.5f, 0);

            myObject = Instantiate(GetPrefab("PlatformDirtL"));
            manager.placedObject.Add(myObject);
            myObject.transform.position = new Vector3(1.5f, -5.5f, 0);

            myObject = Instantiate(GetPrefab("PlatformDirtL"));
            manager.placedObject.Add(myObject);
            myObject.transform.position = new Vector3(4.5f, -5.5f, 0);

            myObject = Instantiate(GetPrefab("PlatformDirtL"));
            manager.placedObject.Add(myObject);
            myObject.transform.position = new Vector3(7.5f, -5.5f, 0);
        }
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteKey(testFile);
        LoadData("Level");
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

        Debug.LogWarning("There is no prefab for " + prefabName);
        return null;
    }
}
