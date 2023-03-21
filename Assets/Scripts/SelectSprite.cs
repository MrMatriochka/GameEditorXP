using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSprite : MonoBehaviour
{
    public GameObject selectedObj;
    public GameObject resizeHUD;
    SpriteEditor spriteEditor;

    void Start()
    {
        spriteEditor = GetComponent<SpriteEditor>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && spriteEditor.pendingObj == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Sprite"))
                {
                    spriteEditor.SaveZ();
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
        //if (Input.GetMouseButtonUp(0) && selectedObj != null)
        //{
        //    Deselect();
        //}
    }

    void Select(GameObject obj)
    {
        //if (obj == selectedObj) return;

        if (selectedObj != null) Deselect();

        selectedObj = obj;

        //selectedObj.transform.GetChild(1).gameObject.SetActive(true);
        resizeHUD.SetActive(true);
        selectedObj.GetComponent<Renderer>().material.SetFloat("_Thickness", 5);
        Move();
    }

    void Deselect()
    {
        //selectedObj.transform.GetChild(1).gameObject.SetActive(false);
        resizeHUD.SetActive(false);
        selectedObj.GetComponent<Renderer>().material.SetFloat("_Thickness", 0);
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
        spriteEditor.pendingObj = selectedObj;
    }

    public void SizeUp()
    {
        if (selectedObj.transform.localScale.x <2)
        {
            spriteEditor.SaveZ();
            selectedObj.transform.localScale += new Vector3(0.25f, 0.25f, 0.25f);
        }
            
        
    }

    public void SizeDown()
    {
        if (selectedObj.transform.localScale.x > 0.25f)
        {
            spriteEditor.SaveZ();
            selectedObj.transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
        }
            
    }

    public void RotateLeft()
    {
        spriteEditor.SaveZ();
        selectedObj.transform.Rotate(Vector3.forward, -10);
    }

    public void RotateRight()
    {
        spriteEditor.SaveZ();
        selectedObj.transform.Rotate(Vector3.forward, 10);
    }
}
