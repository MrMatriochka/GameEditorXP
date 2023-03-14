using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedAsset
{
    public SavedAsset()
    {
        List<int> index = new List<int>();
        List<Vector3> positions = new List<Vector3>();
        List<Quaternion> rotations = new List<Quaternion>();
        List<Vector3> scales = new List<Vector3>();
        List<string> sprites = new List<string>(); ;
    }

    public List<int> index = new List<int>();
    public List<Vector3> positions = new List<Vector3>();
    public List<Quaternion> rotations = new List<Quaternion>();
    public List<Vector3> scales = new List<Vector3>();
    public List<string> sprites = new List<string>();
}

public class SaveEditedAsset : MonoBehaviour
{
    public Dictionary<string, Sprite> spriteByName = new Dictionary<string, Sprite>();
    public List<Sprite> spritesList = new List<Sprite>();

    private string testFolder = "Save";
    [HideInInspector] public string testFile = "Asset_";

    private List<Vector3> myPositions = new List<Vector3>();
    private List<Quaternion> myRotations = new List<Quaternion>();
    private List<Vector3> myScales = new List<Vector3>();
    private List<string> mySprites = new List<string>();
    private List<int> myIndex = new List<int>();

    SpriteEditor editor;

    public GameObject spriteTemplate;
    void Awake()
    {
        editor = GetComponent<SpriteEditor>();
        Initialize();
    }

    public void SaveData(string fileName)
    {
        myPositions.Clear();
        myRotations.Clear();
        myScales.Clear();
        mySprites.Clear();
        myIndex.Clear();

        int newIndex = 0;
        foreach (GameObject obj in editor.placedObject)
        {
            myPositions.Add(obj.transform.position);
            myRotations.Add(obj.transform.rotation);
            myScales.Add(obj.transform.localScale);
            mySprites.Add(obj.GetComponent<SpriteRenderer>().sprite.name);
            myIndex.Add(newIndex);
            newIndex++;
        }

        SavedAsset dataToSave = new SavedAsset
        {
            positions = myPositions,
            rotations = myRotations,
            scales = myScales,
            sprites = mySprites,
            index = myIndex
        };

        SaveLoad<SavedAsset>.Save(dataToSave, fileName);
    }

    public void LoadData(string fileName)
    {
        foreach (GameObject obj in editor.placedObject)
        {
            Destroy(obj);
        }
        editor.placedObject.Clear();


        SavedAsset loadedData = SaveLoad<SavedAsset>.Load(fileName) ?? new SavedAsset();

        myPositions = loadedData.positions;
        myRotations = loadedData.rotations;
        myScales = loadedData.scales;
        mySprites = loadedData.sprites;
        myIndex = loadedData.index;

        foreach (int i in myIndex)
        {
            GameObject myObject = Instantiate(spriteTemplate);
            myObject.GetComponent<SpriteRenderer>().sprite = GetSprite(mySprites[i]);
            editor.placedObject.Add(myObject);
            myObject.transform.position = myPositions[i];
            myObject.transform.rotation = myRotations[i];
            myObject.transform.localScale = myScales[i];
        }
    }

    void Initialize()
    {
        foreach (Sprite sprite in spritesList)
        {
            if (!spriteByName.ContainsKey(sprite.name))
            {
                spriteByName.Add(sprite.name, sprite);
            }
        }
    }

    public Sprite GetSprite(string spriteName)
    {
        if (spriteByName.ContainsKey(spriteName))
        {
            return spriteByName[spriteName];
        }

        Debug.LogWarning("There is no sprite for " + spriteName);
        return null;
    }

    public void ClearData(string fileName)
    {
        PlayerPrefs.DeleteKey(fileName);
        LoadData(fileName);
    }
}
