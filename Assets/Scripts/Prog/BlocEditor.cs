using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class BlocEditor : MonoBehaviour
{
    Vector3 pos;
    public Vector3 posOffset;
    public static GameObject pendingObj;
    [SerializeField] TMP_Text blocRestant;


    public float gridSize;
    public bool gridOn = true;

    private bool mouseOnTrash = false;

    public int maxBlocNb;
    public int blocNb = 0;

    [HideInInspector] public Vector3 decalage;
    void Update()
    {
        if (pendingObj != null)
        {
            if (gridOn)
            {
                pendingObj.transform.position = new Vector3(Snapping.Snap(pos.x, gridSize), Snapping.Snap(pos.y, gridSize), 0)+posOffset;
            }
            else { pendingObj.transform.position = pos + posOffset; }

            if (Input.GetMouseButtonUp(0))
            {
                PlaceObject();
            }

        }

        if (Input.GetMouseButtonUp(0) && mouseOnTrash && pendingObj != null)
        {
            Delete();
        }

        if(blocRestant != null)
        blocRestant.text = "Blocs restants: " + (maxBlocNb - blocNb);
    }
    void PlaceObject()
    {
        if (!mouseOnTrash)
        {

            BlocAssemble assembler = pendingObj.GetComponent<BlocAssemble>();
            BlocAssemble lastAssembler = GetLastObjOfbloc().GetComponent<BlocAssemble>();

            if (assembler.collidingBloc != null)
            {
                if (assembler.canAssembleNext && assembler.canAssemblePrevious)
                {
                    if (assembler.canAssemblePrevious && assembler.collidingBloc.GetComponent<BlocAssemble>().nextBloc == null)
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

                    if (assembler.canAssembleNext && assembler.collidingNextBloc.GetComponent<BlocAssemble>().previousBloc == null)
                    {
                        assembler.transform.position = assembler.collidingNextBloc.GetComponent<BlocAssemble>().previousBlocPosition.transform.position;
                        assembler.collidingNextBloc.GetComponent<BlocAssemble>().previousBloc = pendingObj;
                        assembler.nextBloc = assembler.collidingNextBloc;
                    }

                    pendingObj = null;
                    return;
                }
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
                
                else if (assembler.collidingBloc.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                {
                    if (assembler.canAssemblePrevious && assembler.collidingBloc.GetComponent<BlocAssemble>().midBloc == null)
                    {
                        assembler.collidingBloc.GetComponent<BlocAssemble>().midBloc = pendingObj;
                        assembler.previousBloc = assembler.collidingBloc;
                    }
                }
            }
            else if (lastAssembler.canAssembleNext && lastAssembler.collidingBloc.GetComponent<BlocAssemble>().previousBloc == null)
            {
                lastAssembler.collidingBloc.GetComponent<BlocAssemble>().previousBloc = lastAssembler.gameObject;
                lastAssembler.nextBloc = lastAssembler.collidingBloc;
            }

            pendingObj = null;
        }
    }
    private void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3(mousePosition.x, mousePosition.y, 0) - decalage;
    }
    public void SelectObject(GameObject bloc)
    {
        if (blocNb < maxBlocNb)
        {
            blocNb++;
            pendingObj = Instantiate(bloc, pos, transform.rotation);
        }
    }
    
    
    List<GameObject> objectToDestroy = new List<GameObject>();
    public void Delete()
    {

        CreateDeleteList();

        foreach (GameObject obj in objectToDestroy)
        {
            Destroy(obj);
            blocNb--;
            if (blocNb < 0) blocNb = 0; 
        }

        
        
        pendingObj = null;
    }
    void CreateDeleteList()
    {
        objectToDestroy.Clear();

        objectToDestroy.Add(pendingObj);

        if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
        {
            if(pendingObj.GetComponent<BlocAssemble>().midBloc != null)
            {
                GameObject saveFirstIf = pendingObj;
                while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
                {
                    if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                    {
                        GameObject saveIf = pendingObj;
                        ReadBlocIf();
                        pendingObj = saveIf;
                    }

                    if (pendingObj.GetComponent<BlocAssemble>().nextBloc == null && pendingObj.GetComponent<BlocAssemble>().midBloc != null)
                        return;
                    if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                        pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;

                }
                pendingObj = saveFirstIf;
            }
        }

        if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
        {
            pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;

            while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
            {
                objectToDestroy.Add(pendingObj);

                if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                {
                    GameObject saveIf = pendingObj;
                    ReadBlocIf();
                    pendingObj = saveIf;
                }

                if (pendingObj.GetComponent<BlocAssemble>().nextBloc == null && pendingObj.GetComponent<BlocAssemble>().midBloc != null)
                    return;
                if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;

            }

            objectToDestroy.Add(pendingObj);

        }
        

    }
    void ReadBlocIf()
    {
        if (pendingObj.GetComponent<BlocAssemble>().midBloc != null)
        {
            pendingObj = pendingObj.GetComponent<BlocAssemble>().midBloc;
            while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
            {
                objectToDestroy.Add(pendingObj);

                if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                {
                    GameObject saveIf = pendingObj;
                    ReadBlocIf();
                    pendingObj = saveIf;
                }

                if (pendingObj.GetComponent<BlocAssemble>().nextBloc == null && pendingObj.GetComponent<BlocAssemble>().midBloc != null)
                    return;
                if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;


            }


            objectToDestroy.Add(pendingObj);
        }
    
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

    public void NextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("ProgLvl", sceneIndex + 1);
        if (sceneIndex+1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else
            SceneManager.LoadScene(1);
    }
}
