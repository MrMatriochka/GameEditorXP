using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedBodyParts
{
    public SavedBodyParts()
    {
        List<string> materials = new List<string>(); ;
    }

    public List<string> materials = new List<string>();
}
public class SaveBodyParts : MonoBehaviour
{
    public List<Renderer> bodyParts = new List<Renderer>();

    public Dictionary<string, Material> materialByName = new Dictionary<string, Material>();
    public List<Material> materialsList = new List<Material>();

    [HideInInspector] public string testFile = "BodyParts_";

    public List<string> myMaterials = new List<string>();

    void Awake()
    {
        Initialize();
        
    }

    public void SaveData(string fileName)
    {
        myMaterials.Clear();

        foreach (Renderer obj in bodyParts)
        {
            myMaterials.Add(obj.material.name.Replace(" (Instance)", string.Empty));
        }

        SavedBodyParts dataToSave = new SavedBodyParts
        {
            materials = myMaterials,
        };
 
        SaveLoad<SavedBodyParts>.Save(dataToSave, fileName);
    }

    public void LoadData(string fileName)
    {
        if(PlayerPrefs.HasKey(fileName))
        {
            SavedBodyParts loadedData = SaveLoad<SavedBodyParts>.Load(fileName) ?? new SavedBodyParts();

            myMaterials = loadedData.materials;

            int i = 0;
            foreach (Renderer obj in bodyParts)
            {
                obj.material = GetMaterial(myMaterials[i]);
                i++;
            }
        }
        else 
        {
            bodyParts[0].material = GetMaterial("Tete");
            bodyParts[1].material = GetMaterial("Corp");
            bodyParts[2].material = GetMaterial("MainD");
            bodyParts[3].material = GetMaterial("MainG");
            bodyParts[4].material = GetMaterial("PiedG");
            bodyParts[5].material = GetMaterial("PiedG");
        }
    }

    void Initialize()
    {
        foreach (Material material in materialsList)
        {
            if (!materialByName.ContainsKey(material.name))
            {
                materialByName.Add(material.name, material);
            }
        }
    }

    public Material GetMaterial(string materialName)
    {
        if (materialByName.ContainsKey(materialName))
        {
            return materialByName[materialName];
        }

        Debug.LogWarning("There is no sprite for " + materialName);
        return null;
    }

    public void ClearData(string fileName)
    {
        PlayerPrefs.DeleteKey(fileName);
        LoadData(fileName);
    }
}
