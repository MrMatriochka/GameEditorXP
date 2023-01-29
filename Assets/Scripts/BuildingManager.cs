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

    public GameObject spriteEditor;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        saveLvl = GetComponent<SaveLoadLevel>();
        saveLvl.LoadData();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        if (Input.GetMouseButtonUp(0) && mouseOnTrash && pendingObj != null)
        {
            Delete();
            //saveLvl.SaveData();
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
                //saveLvl.SaveData();
            }
            else if (Input.GetMouseButtonUp(0) && !canPlace && firstPlacement)
            {
                Delete();
                firstPlacement = false;
            }
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
        pos = mousePosition;
    }

    public void SelectObject(GameObject objectToSpawn)
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) return;

        if (!IsPrefabLimitExceeded(objectToSpawn))
        {
            pendingObj = Instantiate(objectToSpawn, pos, transform.rotation);
            firstPlacement = true;
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
        saveLvl.SaveData();
        levelEditorUI.SetActive(false);
        inGameUI.SetActive(true);

        foreach (GameObject obj in placedObject)
        {
            if (obj != null)
            {
                obj.GetComponent<SpriteRenderer>().enabled = false;
                obj.GetComponent<Collider2D>().enabled = false;

                int startChildCount = obj.transform.childCount;
                for (int i = 1; i < startChildCount; i++)
                {
                        obj.transform.GetChild(1).parent = obj.transform.GetChild(0);
                }

                obj.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        cam.GetComponent<CameraController>().enabled = true;
        cam.GetComponent<CameraController>().camLimit = camLimit;
        cam.GetComponent<CameraController>().FindPlayer(cam.GetComponent<CameraController>().faceLeft);
    }

    public void Stop()
    {
        levelEditorUI.SetActive(true);
        saveLvl.LoadData();

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

    public void MouseEnterTrash()
    {
        mouseOnTrash = true;
    }
    public void MousExitTrash()
    {
        mouseOnTrash = false;
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

    public void OpenSpriteEditor(GameObject prefabToEdit)
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            mouseOnTrash = false;
            spriteEditor.SetActive(true);
            transform.parent.gameObject.SetActive(false);

            saveLvl.SaveData();
            foreach (GameObject obj in placedObject)
            {
                Destroy(obj);
            }

            spriteEditor.transform.GetChild(0).GetComponent<SpriteEditor>().editedPrefab = prefabToEdit;
            spriteEditor.transform.GetChild(0).GetComponent<SpriteEditor>().OpenSpriteEditor();
        }
    }

    public void Reset()
    {
        saveLvl.ClearData();
    }
}
