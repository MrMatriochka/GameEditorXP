using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject levelEditorUI;
    public GameObject inGameUI;
    public Scrollbar map;
    Vector3 pos;
    [HideInInspector] public GameObject pendingObj;
    [SerializeField] private Material[] materials;

    public float gridSize;
    public bool gridOn = true;

    public bool canPlace = true;

    public List<GameObject> placedObject = new List<GameObject>();
    [HideInInspector] public List<GameObject> objectToDestroy = new List<GameObject>();

    private GameObject cam;

    public float camLimit;

    private bool mouseOnTrash = false;
    private bool firstPlacement = false;

    [HideInInspector] public SaveLoadLevel saveLvl;

    SelectObject selectObj;

    [HideInInspector] public Vector3 decalage;

    public Sprite trashIn;
    public Sprite trashOut;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        saveLvl = GetComponent<SaveLoadLevel>();
        saveLvl.LoadData("Level");
        saveZID.Clear();
        selectObj = GetComponent<SelectObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && mouseOnTrash && pendingObj != null)
        {
            Delete();
        }

        if (pendingObj != null)
        {
            if(gridOn)
            {
                pendingObj.transform.position = new Vector3(Snapping.Snap(pos.x,gridSize), Snapping.Snap(pos.y, gridSize), 0);
            }
            else { pendingObj.transform.position = pos; }

            UpdateMaterials();

            if (Input.GetMouseButtonUp(0) && canPlace)
            {
                PlaceObject();
            }
            else if (Input.GetMouseButtonUp(0) && !canPlace && firstPlacement)
            {
                Delete();
                firstPlacement = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            LoadZ();
        }
    }

    void PlaceObject()
    {
        if (!mouseOnTrash)
        {
            pendingObj.GetComponent<SpriteRenderer>().material = materials[2];
            if(firstPlacement)
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
        pos = new Vector3(mousePosition.x, mousePosition.y, 0) - decalage;
    }

    public void SelectObject(PrefabInButton script)
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) return;

        GameObject objectToSpawn = script.prefab;

        if (!IsPrefabLimitExceeded(objectToSpawn))
        {
            SaveZ();
            pendingObj = Instantiate(objectToSpawn, pos, transform.rotation);
            firstPlacement = true;
            selectObj.selectedObj = pendingObj;
        }
    }

    void UpdateMaterials()
    {
        if(canPlace)
        {
            pendingObj.GetComponent<SpriteRenderer>().material = materials[0];
        }
        else
        {
            pendingObj.GetComponent<SpriteRenderer>().material = materials[1];
        }
    }

    public void Play()
    {
        saveLvl.SaveData("Level");
        levelEditorUI.SetActive(false);
        inGameUI.SetActive(true);

        foreach (GameObject obj in placedObject)
        {
            if (obj != null)
            {
                obj.GetComponent<SpriteRenderer>().enabled = false;
                obj.GetComponent<Collider2D>().enabled = false;

                int startChildCount = obj.transform.childCount;
                for (int i = 2; i < startChildCount; i++)
                {
                        obj.transform.GetChild(2).parent = obj.transform.GetChild(1);
                }

                obj.transform.GetChild(0).gameObject.SetActive(false);
                obj.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        cam.GetComponent<CameraController>().enabled = true;
        cam.GetComponent<CameraController>().camLimit = camLimit;
        cam.GetComponent<CameraController>().FindPlayer(cam.GetComponent<CameraController>().faceLeft);

        saveZID.Clear();
    }

    public void Stop()
    {
        levelEditorUI.SetActive(true);
        saveLvl.LoadData("Level");

        foreach (GameObject obj in objectToDestroy)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        objectToDestroy.Clear();

        inGameUI.SetActive(false);
        cam.GetComponent<CameraController>().enabled = false;
        
        cam.transform.position = new Vector3(0, 0, -10);
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
    }

    public void MouseEnterTrash(GameObject obj)
    {
        mouseOnTrash = true;
        if (selectObj.selectedObj != null)
            obj.SetActive(true);
    }
    
    public void MouseEnterTrashImage(GameObject obj)
    {
        mouseOnTrash = true;
        if (selectObj.selectedObj != null)
            obj.GetComponent<Image>().sprite = trashIn;
    }

    public void MousExitTrash(GameObject obj)
    {
        mouseOnTrash = false;

            obj.SetActive(false);
    }
    public void MouseExitTrashImage(GameObject obj)
    {
        mouseOnTrash = false;
        if (selectObj.selectedObj != null)
            obj.GetComponent<Image>().sprite = trashOut;
    }
    

    public bool IsPrefabLimitExceeded(GameObject prefab)
    {
        int limit = prefab.GetComponent<CheckPlacement>().nbLimit;
        if (limit == 0)
        {
            return false;
        }
        else
        {
            int actualNb = 0;
            foreach (GameObject obj in placedObject)
            {
                if (obj.name.Replace("(Clone)", string.Empty) == prefab.name)
                {
                    actualNb++;
                }
            }
            if (actualNb < limit)
            {
                return false;
            }
            else
            {
                return true;
            }
        }   
    }

    public void MoveMap()
    {
        cam.transform.position = new Vector3(camLimit*map.value, 0, -10);
    }

    public void Reset()
    {
        saveLvl.ClearData();
    }

    // ctrlZ
    List<string> saveZID = new List<string>();
    public void SaveZ()
    {
        saveLvl.SaveData("Z" + saveZID.Count);
        saveZID.Add("Z" + saveZID.Count);
    }

    public void LoadZ()
    {
        if (saveZID.Count != 0)
        {
            saveLvl.LoadData(saveZID[saveZID.Count - 1]);
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
