using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBloc : MonoBehaviour
{
    public GameObject selectedObj;
    BlocEditor blocEditor;

    void Start()
    {
        blocEditor = GetComponent<BlocEditor>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && BlocEditor.pendingObj == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Bloc") && hit.collider.GetComponent<BlocAssemble>().type != BlocAssemble.BlocType.Start)
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

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        blocEditor.decalage = new Vector3(mousePosition.x, mousePosition.y, 0) - obj.transform.position;

        selectedObj = obj;


        if(selectedObj.GetComponent<BlocAssemble>().previousBloc != null)
        {
            if (selectedObj.GetComponent<BlocAssemble>().previousBloc.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.Start)
            {
                selectedObj.GetComponent<BlocAssemble>().previousBloc.GetComponent<BlocAssemble>().midBloc = null;
                selectedObj.GetComponent<BlocAssemble>().previousBloc.GetComponent<BlocAssemble>().nextBloc = null;
            }

            if (selectedObj.GetComponent<BlocAssemble>().previousBloc.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.Boucle)
            {
                selectedObj.GetComponent<BlocAssemble>().previousBloc.GetComponent<BlocAssemble>().midBloc = null;
                selectedObj.GetComponent<BlocAssemble>().previousBloc.GetComponent<BlocAssemble>().nextBloc = null;
            }

            if (selectedObj.GetComponent<BlocAssemble>().previousBloc.GetComponent<BlocAssemble>().midBloc == selectedObj)
            {
                selectedObj.GetComponent<BlocAssemble>().previousBloc.GetComponent<BlocAssemble>().midBloc = null;
            }
            else
            {
                selectedObj.GetComponent<BlocAssemble>().previousBloc.GetComponent<BlocAssemble>().nextBloc = null;
            }
            
            selectedObj.GetComponent<BlocAssemble>().previousBloc = null;
        }


        GetLastObjOfbloc().GetComponent<BlocAssemble>().lastOfThePendingBloc = true;

        Move();
    }

    void Deselect()
    {
        GetLastObjOfbloc().GetComponent<BlocAssemble>().lastOfThePendingBloc = false;
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

    GameObject GetLastObjOfbloc()
    {
        if (selectedObj.GetComponent<BlocAssemble>().nextBloc != null)
        {
            GameObject lastObj = selectedObj;
            while (lastObj.GetComponent<BlocAssemble>().nextBloc != null)
            {
                lastObj = lastObj.GetComponent<BlocAssemble>().nextBloc;
            }
            return lastObj;
        }
        else
        {
            return selectedObj;
        }
    }
}
