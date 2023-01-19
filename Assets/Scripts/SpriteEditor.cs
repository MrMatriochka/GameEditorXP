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
    private void Awake()
    {
        foreach (GameObject prefab in dictionary)
        {
            prefab.GetComponent<BoxCollider2D>().enabled = false;
            prefab.GetComponent<CheckPlacement>().enabled = false;
            prefab.transform.localPosition = Vector3.zero;

            GetComponent<SaveEditedAsset>().LoadData(prefab.name);
            foreach (GameObject obj in placedObject)
            {
                obj.transform.parent = prefab.transform;
            }
            placedObject.Clear();
            prefab.transform.localPosition = new Vector3(0,-20,0);
        }
        
    }
    private void Start()
    {
        OpenSpriteEditor();
    }

    public void OpenSpriteEditor()
    {
        cam.orthographicSize = 3;
        foreach (GameObject prefab in dictionary)
        {
            prefab.GetComponent<BoxCollider2D>().enabled = false;
            prefab.GetComponent<CheckPlacement>().enabled = false;
        }

        editedPrefab.transform.position = Vector3.zero;

        int startChildCount = editedPrefab.transform.childCount;
        for (int i = 1; i < startChildCount; i++)
        {
            Destroy(editedPrefab.transform.GetChild(i).gameObject);
        }

        GetComponent<SaveEditedAsset>().LoadData(editedPrefab.name);
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

            if (Input.GetMouseButtonUp(0) && canPlace)
            {
                PlaceObject();
            }

        }

        if (Input.GetMouseButtonUp(0) && mouseOnTrash && pendingObj != null)
        {
            Delete();
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
        pendingObj = Instantiate(SpriteTemplate, pos, transform.rotation);
        pendingObj.transform.position += new Vector3(0,0,-1);
        pendingObj.GetComponent<SpriteRenderer>().sprite = localSprite;
        firstPlacement = true;
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

    public void SaveNewPrefab()
    {
        GetComponent<SaveEditedAsset>().SaveData(editedPrefab.name);
        foreach (GameObject obj in placedObject)
        {
            obj.transform.parent = editedPrefab.transform;
        }
        placedObject.Clear();
        editedPrefab.GetComponent<BoxCollider2D>().enabled = true;
        editedPrefab.GetComponent<CheckPlacement>().enabled = true;
        editedPrefab.transform.localPosition = new Vector3(0, -20, 0);
        spriteEditor.SetActive(false);
        buildingManager.transform.parent.gameObject.SetActive(true);

        foreach (GameObject prefab in dictionary)
        {
            prefab.GetComponent<BoxCollider2D>().enabled = true;
            prefab.GetComponent<CheckPlacement>().enabled = true;
        }

        buildingManager.GetComponent<SaveLoadLevel>().LoadData();

        cam.orthographicSize = 10;
    }
}
