using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocEditor : MonoBehaviour
{
    Vector3 pos;
    public static GameObject pendingObj;


    public float gridSize;
    public bool gridOn = true;

    private bool mouseOnTrash = false;


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

            BlocAssemble assembler = pendingObj.GetComponent<BlocAssemble>();
            BlocAssemble lastAssembler = GetLastObjOfbloc().GetComponent<BlocAssemble>();
            if (assembler.collidingBloc != null)
            {
                if (assembler.canAssembleNext && assembler.collidingBloc.GetComponent<BlocAssemble>().previousBloc == null)
                {
                    assembler.transform.position = assembler.collidingBloc.GetComponent<BlocAssemble>().previousBlocPosition.transform.position;
                    assembler.collidingBloc.GetComponent<BlocAssemble>().previousBloc = pendingObj;
                    assembler.nextBloc = assembler.collidingBloc;
                }
                else if (assembler.canAssemblePrevious && assembler.collidingBloc.GetComponent<BlocAssemble>().nextBloc == null)
                {
                    if (assembler.collidingBloc.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If && !assembler.collidingWithBlocEnd)
                    {
                        assembler.collidingBloc.GetComponent<BlocAssemble>().midBloc = pendingObj;
                        assembler.previousBloc = assembler.collidingBloc;
                    }
                    else
                    {
                        assembler.collidingBloc.GetComponent<BlocAssemble>().nextBloc = pendingObj;
                        assembler.previousBloc = assembler.collidingBloc;
                    }
                }
                else if (lastAssembler.canAssembleNext && lastAssembler.collidingBloc.GetComponent<BlocAssemble>().previousBloc == null)
                {
                    lastAssembler.collidingBloc.GetComponent<BlocAssemble>().previousBloc = lastAssembler.gameObject;
                    lastAssembler.nextBloc = lastAssembler.collidingBloc;
                }
                else if (assembler.collidingBloc.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                {
                    if (assembler.canAssemblePrevious && assembler.collidingBloc.GetComponent<BlocAssemble>().midBloc == null)
                    {
                        assembler.collidingBloc.GetComponent<BlocAssemble>().midBloc = pendingObj;
                        assembler.previousBloc = assembler.collidingBloc;
                    }
                }
            }
            

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
    }

    public void Delete()
    {
        List<GameObject> objectToDestroy = new List<GameObject>();
        while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
        {
            objectToDestroy.Add(pendingObj);
            if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If && pendingObj.GetComponent<BlocAssemble>().midBloc!=null)
            {
                GameObject ifBloc = pendingObj;
                pendingObj = pendingObj.GetComponent<BlocAssemble>().midBloc;
                objectToDestroy.Add(pendingObj);
                while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                {
                    objectToDestroy.Add(pendingObj);
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
                }
                objectToDestroy.Add(pendingObj);
                pendingObj = ifBloc;
                pendingObj.GetComponent<BlocAssemble>().midBloc = null;
            }

            if(pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;

            
        }
        objectToDestroy.Add(pendingObj);

        foreach (GameObject obj in objectToDestroy)
        {
            Destroy(obj);
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

    GameObject GetLastObjOfbloc()
    {
        if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
        {
            GameObject lastObj = pendingObj;
            while (lastObj.GetComponent<BlocAssemble>().nextBloc != null)
            {
                lastObj = lastObj.GetComponent<BlocAssemble>().nextBloc;
            }
            return lastObj;
        }
        else
        {
            return pendingObj;
        }
    }
}
