using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocEditor : MonoBehaviour
{
    Vector3 pos;
    [HideInInspector]public static GameObject pendingObj;
    public List<GameObject> placedObject = new List<GameObject>();

    public float gridSize;
    public bool gridOn = true;

    private bool mouseOnTrash = false;
    private bool firstPlacement = false;
    void Update()
    {
        if (pendingObj != null)
        {
            if (gridOn)
            {
                pendingObj.transform.position = new Vector3(Snapping.Snap(pos.x, gridSize), Snapping.Snap(pos.y, gridSize), 0);
            }
            else { pendingObj.transform.position = pos ; }

            if (Input.GetMouseButtonUp(0))
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

            if (pendingObj.GetComponent<BlocAssemble>().CanAssemble() == true)
            {
                pendingObj.transform.position = pendingObj.GetComponent<BlocAssemble>().touchingObj.transform.position;

                pendingObj.transform.parent = pendingObj.GetComponent<BlocAssemble>().touchingObj;

            }
            else { pendingObj.transform.parent = null; }

            pendingObj = null;
        }
    }
    private void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = mousePosition;
    }
    public void SelectObject(GameObject bloc)
    {
        pendingObj = Instantiate(bloc, pos, transform.rotation);
        pendingObj.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
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
}
