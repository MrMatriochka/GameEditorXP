using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;

    public GameObject levelEditorUI;

    Vector3 pos;
    [HideInInspector] public GameObject pendingObj;
    [SerializeField] private Material[] materials;

    public float gridSize;
    public bool gridOn = true;

    public bool canPlace = true;

    public List<GameObject> placedObject = new List<GameObject>();

    void Update()
    {
        if(pendingObj != null)
        {
            if(gridOn)
            {
                pendingObj.transform.position = new Vector3(RoundToNearestGrid(pos.x), RoundToNearestGrid(pos.y), 0);
            }
            else { pendingObj.transform.position = pos; }

            UpdateMaterials();

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceObject();
            }
            
        }
    }

    void PlaceObject()
    {
        pendingObj.GetComponent<SpriteRenderer>().material = materials[2];
        placedObject.Add(pendingObj);
        pendingObj = null;
    }
    private void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = mousePosition;
    }

    public void SelectObject(int index)
    {
        pendingObj = Instantiate(objects[index], pos, transform.rotation);
    }

    float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff + gridSize / 2;
        if(xDiff>(gridSize/2))
        {
            pos += gridSize * 1.5f;
        }
        return pos;
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
        levelEditorUI.SetActive(false);
        foreach (GameObject obj in placedObject)
        {
            if(obj!=null)
            {
                obj.GetComponent<SpriteRenderer>().enabled = false;
                obj.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
