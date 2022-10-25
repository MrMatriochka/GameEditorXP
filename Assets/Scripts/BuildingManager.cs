using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer;
public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;

    public GameObject levelEditorUI;
    public GameObject inGameUI;

    Vector3 pos;
    [HideInInspector] public GameObject pendingObj;
    [SerializeField] private Material[] materials;

    public float gridSize;
    public bool gridOn = true;

    public bool canPlace = true;

    public List<GameObject> placedObject = new List<GameObject>();

    private GameObject cam;

    public float camSpeed;
    public float camLimit;
    public GameObject leftButton;
    public GameObject rightButton;

    private bool mouseOnTrash = false;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
    }

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

            if (Input.GetMouseButtonUp(0) && canPlace)
            {
                PlaceObject();
            }

        }

        if (Input.GetMouseButtonDown(0) && pendingObj == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Prefab"))
                {
                    SelectObject(1);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && mouseOnTrash)
        {
            Delete();
        }
    }

    void PlaceObject()
    {
        if (!mouseOnTrash)
        {
            pendingObj.GetComponent<SpriteRenderer>().material = materials[2];
            placedObject.Add(pendingObj);
            pendingObj = null;
        }
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
        inGameUI.SetActive(true);
        cam.GetComponent<CameraController>().enabled = true;
    }

    public void Stop()
    {
        levelEditorUI.SetActive(true);
        foreach (GameObject obj in placedObject)
        {
            if (obj != null)
            {
                obj.GetComponent<SpriteRenderer>().enabled = true;
                obj.transform.GetChild(0).gameObject.SetActive(false);
                obj.transform.GetChild(0).localPosition = new Vector2(0,0);
                obj.transform.GetChild(0).localRotation = Quaternion.Euler(0,0,0);
            }
        }
        inGameUI.SetActive(false);
        cam.GetComponent<CameraController>().enabled = false;
        cam.transform.position = new Vector3(0, 0, -10);
        Destroy(GameObject.FindWithTag("DeadBody"));
    }

    public void MoveScreen(int direction)
    {
        cam.transform.position += new Vector3(camSpeed * direction,0,0);

        rightButton.SetActive(true);
        leftButton.SetActive(true);

        if (cam.transform.position.x >= camLimit)
        {
            cam.transform.position = new Vector3(camLimit, 0, -10);
            rightButton.SetActive(false);
        }
        if(cam.transform.position.x <= 0)
        {
            cam.transform.position = new Vector3(0, 0, -10);
            leftButton.SetActive(false);
        }
    }

    public void Delete()
    {
        Destroy(pendingObj);
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
}
