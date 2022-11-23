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

    public float camSpeed;
    public float camLimit;
    public GameObject leftButton;
    public GameObject rightButton;

    private bool mouseOnTrash = false;
    private bool firstPlacement = false;

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
                pendingObj.transform.position = new Vector3(Snapping.Snap(pos.x,gridSize), Snapping.Snap(pos.y, gridSize), 0);
            }
            else { pendingObj.transform.position = pos; }

            UpdateMaterials();

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
        levelEditorUI.SetActive(false);
        inGameUI.SetActive(true);
        foreach (GameObject obj in placedObject)
        {
            if(obj!=null)
            {
                obj.GetComponent<SpriteRenderer>().enabled = false;
                obj.GetComponent<Collider2D>().enabled = false;
                obj.transform.GetChild(0).gameObject.SetActive(true);
            }
        }     
        cam.GetComponent<CameraController>().enabled = true;

        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.score = 0;
        player.UpdateScore();
        player.hp = player.maxHp;
        player.UpdateLife();
        player.isInvincible = false;
        player.transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        player.lastCheckpoint = player.transform.parent;
        player.canMove = true;
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
                obj.GetComponent<Collider2D>().enabled = true;
            }
        }

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

    public void MoveScreen(int direction)
    {
        cam.transform.position += new Vector3(camSpeed * direction,0,-10);

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
}
