using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBloc : MonoBehaviour
{
    public GameObject selectedObj;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && BlocEditor.pendingObj == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Bloc"))
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
        BlocEditor.pendingObj = selectedObj;
    }
}
