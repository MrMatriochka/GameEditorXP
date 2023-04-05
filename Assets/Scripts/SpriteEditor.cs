using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpriteEditor : MonoBehaviour
{
    Vector3 pos;
    [HideInInspector] public GameObject pendingObj;

    public float gridSize;
    public bool gridOn = true;

    public bool canPlace = true;

    public GameObject SpriteTemplate;
    public List<GameObject> placedObject = new List<GameObject>();

    private bool mouseOnTrash = false;
    private bool firstPlacement = false;

    public GameObject editedPrefab;
    public GameObject spriteEditor;
    public GameObject buildingManager;

    public List<GameObject> dictionary = new List<GameObject>();
    public Camera cam;
    public GameObject resizeHUD;
    public GameObject deselectBugFixer;

    static bool firstLaunch = true;
    private void Awake()
    {
        foreach (GameObject prefab in dictionary)
        {
            prefab.GetComponent<Collider2D>().enabled = false;
            prefab.GetComponent<CheckPlacement>().enabled = false;
            prefab.transform.localPosition = Vector3.zero;

            GetComponent<SaveEditedAsset>().LoadData(prefab.name);
            foreach (GameObject obj in placedObject)
            {
                obj.transform.parent = prefab.transform;
            }
            placedObject.Clear();
            prefab.transform.localPosition = new Vector3(dictionary.IndexOf(prefab)*10, -20,0);
        }
        
    }
    private void Start()
    {      
        OpenSpriteEditor();

        if (firstLaunch == false)
        {
            SaveNewPrefab();
        }
    }

    public void OpenSpriteEditor()
    {
        ClearZ();
        cam.orthographicSize = 3;
        foreach (GameObject prefab in dictionary)
        {
            prefab.GetComponent<Collider2D>().enabled = false;
            prefab.GetComponent<CheckPlacement>().enabled = false;
        }

        editedPrefab.transform.position = Vector3.zero;

        int startChildCount = editedPrefab.transform.childCount;
        for (int i = 2; i < startChildCount; i++)
        {
            if(!editedPrefab.transform.GetChild(i).gameObject.CompareTag("DontDestroy"))
            Destroy(editedPrefab.transform.GetChild(i).gameObject);
        }

        GetComponent<SaveEditedAsset>().LoadData(editedPrefab.name);

        if (editedPrefab.name == "Player")
        {
            editedPrefab.GetComponent<SaveBodyParts>().LoadData("BodyParts");
        }
    }

    void Update()
    {
        if (pendingObj != null)
        {
            if (gridOn)
            {
                pendingObj.transform.position = new Vector3(Snapping.Snap(pos.x, gridSize), Snapping.Snap(pos.y, gridSize), -1);
            }
            else { pendingObj.transform.position = pos + new Vector3(0, 0, -1); }

            resizeHUD.transform.position = cam.WorldToScreenPoint(pendingObj.transform.position);
            deselectBugFixer.transform.position = pendingObj.transform.position - new Vector3(0,0.4f,0);

            if (Input.GetMouseButtonUp(0) && canPlace)
            {
                PlaceObject();
            }

        }

        if (Input.GetMouseButtonUp(0) && mouseOnTrash && pendingObj != null)
        {
            Delete();
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            LoadZ();
        }
    }

    void PlaceObject()
    {
        if (!mouseOnTrash)
        {
            if (firstPlacement)
            {
                placedObject.Add(pendingObj);
                firstPlacement = false;
            }
            pendingObj = null;
            
        }
        
    }
    private void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = mousePosition;
    }

    public void SelectObject(Sprite localSprite)
    {
        SaveZ();
        pendingObj = Instantiate(SpriteTemplate, pos, transform.rotation);
        pendingObj.transform.position += new Vector3(0,0,-1);
        pendingObj.transform.rotation = Quaternion.Euler(0, 0, -10);
        pendingObj.GetComponent<SpriteRenderer>().sprite = localSprite;
        firstPlacement = true;

        GetComponent<SelectSprite>().selectedObj = pendingObj;
    }

    public void SelectMaterial(Material localMaterial)
    {
        GameObject bodyPart = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<BodyParts>().bodyPart;
        bodyPart.GetComponent<Renderer>().material = localMaterial;
    }
    public void Delete()
    {
        Destroy(pendingObj);
        if (!firstPlacement)
        {
            int index = placedObject.IndexOf(pendingObj);
            placedObject.RemoveAt(index);
        }
        pendingObj = null;
        resizeHUD.SetActive(false);
        
    }

    public void MouseEnterTrash()
    {
        mouseOnTrash = true;
    }
    public void MousExitTrash()
    {
        mouseOnTrash = false;
    }

    public void SaveNewPrefab()
    {
        if(editedPrefab.name == "Player")
        {
            editedPrefab.GetComponent<SaveBodyParts>().SaveData("BodyParts");
            editedPrefab.transform.GetChild(0).gameObject.SetActive(true);
        }


        GetComponent<SaveEditedAsset>().SaveData(editedPrefab.name);
        foreach (GameObject obj in placedObject)
        {
            obj.transform.GetChild(0).gameObject.SetActive(false);
            obj.transform.GetChild(1).gameObject.SetActive(false);
            obj.transform.parent = editedPrefab.transform;
            
        }
        placedObject.Clear();

        editedPrefab.GetComponent<Collider2D>().enabled = true;
        editedPrefab.GetComponent<CheckPlacement>().enabled = true;
        editedPrefab.transform.localPosition = new Vector3(dictionary.IndexOf(editedPrefab)*10, -20, 0);
        spriteEditor.SetActive(false);
        buildingManager.transform.parent.gameObject.SetActive(true);

        foreach (GameObject prefab in dictionary)
        {
            prefab.GetComponent<Collider2D>().enabled = true;
            prefab.GetComponent<CheckPlacement>().enabled = true;
        }

        buildingManager.GetComponent<SaveLoadLevel>().LoadData("Level");
        cam.orthographicSize = 10;

        firstLaunch = false;
    }

    public void Reset()
    {
        GetComponent<SaveEditedAsset>().ClearData(editedPrefab.name);
        if (editedPrefab.name == "Player")
        {
            editedPrefab.GetComponent<SaveBodyParts>().ClearData("BodyParts");
        }
    }


    List<string> saveZID = new List<string>();
    // ctrlZ
    public void SaveZ()
    {
        GetComponent<SaveEditedAsset>().SaveData("Z"+saveZID.Count);
        saveZID.Add("Z" + saveZID.Count);
    }

    public void LoadZ()
    {
        if(saveZID.Count !=0)
        {
            resizeHUD.SetActive(false);
            GetComponent<SaveEditedAsset>().LoadData(saveZID[saveZID.Count - 1]);
            saveZID.RemoveAt(saveZID.Count - 1);
        }
        
    }

    public void ClearZ()
    {
        foreach (string save in saveZID)
        {
            PlayerPrefs.DeleteKey(save);
        }
        saveZID.Clear();
    }
}
