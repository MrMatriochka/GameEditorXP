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

    private void Start()
    {
        OpenSpriteEditor();
    }

    public void OpenSpriteEditor()
    {
        editedPrefab.GetComponent<BoxCollider2D>().enabled = false;
        editedPrefab.GetComponent<CheckPlacement>().enabled = false;

        editedPrefab.transform.position = Vector3.zero;
        GetComponent<SaveEditedAsset>().LoadData(editedPrefab.name);
    }

    void Update()
    {
        if (pendingObj != null)
        {
            if (gridOn)
            {
                pendingObj.transform.position = new Vector3(Snapping.Snap(pos.x, gridSize), Snapping.Snap(pos.y, gridSize), 0);
            }
            else { pendingObj.transform.position = pos; }

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
        editedPrefab.GetComponent<BoxCollider2D>().enabled = true;
        editedPrefab.GetComponent<CheckPlacement>().enabled = true;
        editedPrefab.transform.localPosition = Vector3.zero;
        spriteEditor.SetActive(false);
        buildingManager.transform.parent.gameObject.SetActive(true);

        buildingManager.GetComponent<SaveLoadLevel>().LoadData();
    }
}
