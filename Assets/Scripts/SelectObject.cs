using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectObject : MonoBehaviour
{
    public GameObject selectedObj;
    public GameObject objPannel;
    public TextMeshProUGUI objNameTxt;

    BuildingManager buildManager;

    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildingManager>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.CompareTag("Placement"))
                {
                    Select(hit.collider.gameObject);
                }
                else
                {
                    Deselect();
                }
                return;
            }
            //Deselect();
        }
    }

    void Select(GameObject obj)
    {
        if (obj == selectedObj) return;
        if (selectedObj != null) Deselect();
        Outline outline = obj.GetComponent<Outline>();
        if (outline == null) obj.AddComponent<Outline>();
        else outline.enabled = true;
        objPannel.SetActive(true);
        objNameTxt.text = obj.name;
        selectedObj = obj;
    }

    void Deselect()
    {
        selectedObj.GetComponent<Outline>().enabled = false;
        selectedObj = null;
        objPannel.SetActive(false);
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
