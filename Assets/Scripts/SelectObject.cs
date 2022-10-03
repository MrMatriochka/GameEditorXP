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
                    Select(hit.collider.gameObject);
                }
                else
                {
                    if (selectedObj != null) Deselect();
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
        if (obj == selectedObj) return;

        if (selectedObj != null) Deselect();

        selectedObj = obj;

        Move();
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
    public void Move()
    {
        buildManager.pendingObj = selectedObj;
    }
}
