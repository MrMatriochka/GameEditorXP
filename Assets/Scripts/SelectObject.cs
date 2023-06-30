using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectObject : MonoBehaviour
{
    public GameObject selectedObj;
    BuildingManager buildManager;

    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildingManager>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && buildManager.pendingObj == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.CompareTag("Placement"))
                {
                    buildManager.SaveZ();
                    Select(hit.collider.gameObject);
                }
            }
            else
            {
                if (selectedObj != null) Deselect();
            }
        }
        if (Input.GetMouseButtonUp(0) && selectedObj != null)
        {
            Deselect();
        }
    }

    void Select(GameObject obj)
    {
        if (selectedObj != null) Deselect();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        buildManager.decalage = new Vector3(mousePosition.x, mousePosition.y, 0) -obj.transform.position;

        selectedObj = obj;
        buildManager.pendingObj = selectedObj;
    }

    void Deselect()
    {
        selectedObj = null;
    }

    public void Delete()
    {
        GameObject objToDestroy = selectedObj;
        Deselect();
        Destroy(objToDestroy);
    }
}
