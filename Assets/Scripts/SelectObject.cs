using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectObject : MonoBehaviour
{
    public GameObject selectedObj;
    public GameObject resizeHUD;
    public GameObject rotateHUD;
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
                //else
                //{
                //    if (selectedObj != null) Deselect();
                //}
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
        //if (obj == selectedObj) return;

        if (selectedObj != null) Deselect();

        selectedObj = obj;
        //resizeHUD.SetActive(true);
        if(selectedObj.GetComponent<ObjInfo>().canRotate)
            rotateHUD.SetActive(true);
        else
            rotateHUD.SetActive(false);
        Move();
    }

    void Deselect()
    {
        //resizeHUD.SetActive(false);
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

    public void SizeUp()
    {
        if (selectedObj.transform.localScale.x < selectedObj.GetComponent<ObjInfo>().maxSize)
        {
            buildManager.SaveZ();
            selectedObj.transform.localScale += new Vector3(0.25f, 0.25f, 0.25f);
        }
            

    }

    public void SizeDown()
    {
        if (selectedObj.transform.localScale.x > selectedObj.GetComponent<ObjInfo>().minSize)
        {
            buildManager.SaveZ();
            selectedObj.transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
        }
            
    }

    public void RotateLeft()
    {
        buildManager.SaveZ();
        selectedObj.transform.Rotate(Vector3.forward, -10);
    }

    public void RotateRight()
    {
        buildManager.SaveZ();
        selectedObj.transform.Rotate(Vector3.forward, 10);
    }
}
