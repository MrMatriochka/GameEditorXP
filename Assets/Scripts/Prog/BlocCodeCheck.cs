using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocCodeCheck : MonoBehaviour
{
    public List<BlocFunction.Function> codeList = new List<BlocFunction.Function>();
    List<GameObject> bloc = new List<GameObject>();
    GameObject pendingObj;

    bool startPreview;
    Vector3 startPosition;
    Vector3 startScale;
    public Dictionary<BlocFunction.Function, Methods> methodByName = new Dictionary<BlocFunction.Function, Methods>();
    public delegate IEnumerator Methods();

    public GameObject boss;
    public GameObject player;
    public GameObject checkpoint;
    public float speed;
    float baseSpeed;
    [HideInInspector] public bool playerIsDead;
    Vector3 playerStartPosition;
    int index;
    public float waitTime;
    bool boucleOn;
    int boucleIndex;
    private void Start()
    {
        startPosition = boss.transform.position;
        startScale = boss.transform.localScale;
        baseSpeed = speed;

        if (player != null) playerStartPosition = player.transform.position;
        Initialize();
    }

    public void CreateCodeList()
    {


        ResetPreview();
        startPreview = true;
        pendingObj = gameObject;
        
        if(pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
        {
            pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;

            while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
            {
                codeList.Add(pendingObj.GetComponent<BlocFunction>().function);
                bloc.Add(pendingObj);

                if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                {
                    GameObject saveIf = pendingObj;
                    ReadBlocIf();
                    pendingObj = saveIf;
                    codeList.Add(BlocFunction.Function.EndIf);
                }

                if (pendingObj.GetComponent<BlocAssemble>().nextBloc == null && pendingObj.GetComponent<BlocAssemble>().midBloc != null)
                {
                    index = 0;
                    boucleOn = false;
                    StartCoroutine(methodByName[codeList[0]]());
                    return;
                }
                    
                if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
                
            }

            


            codeList.Add(pendingObj.GetComponent<BlocFunction>().function);
            bloc.Add(pendingObj);
            if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                codeList.Add(BlocFunction.Function.EndIf);


            index = 0;
            boucleOn = false;
            StartCoroutine(methodByName[codeList[0]]());

        }

        
    }

    void ReadBlocIf()
    {
        if(pendingObj.GetComponent<BlocAssemble>().midBloc != null)
        {
            pendingObj = pendingObj.GetComponent<BlocAssemble>().midBloc;
            while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
            {
                codeList.Add(pendingObj.GetComponent<BlocFunction>().function);
                bloc.Add(pendingObj);

                if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                {
                    GameObject saveIf = pendingObj;
                    ReadBlocIf();
                    pendingObj = saveIf;
                    codeList.Add(BlocFunction.Function.EndIf);
                }

                if (pendingObj.GetComponent<BlocAssemble>().nextBloc == null && pendingObj.GetComponent<BlocAssemble>().midBloc != null)
                    return;
                if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
                

            }
            

            codeList.Add(pendingObj.GetComponent<BlocFunction>().function);
            bloc.Add(pendingObj);
            if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                codeList.Add(BlocFunction.Function.EndIf);
        }
    }

    public void ResetPreview()
    {
        boss.transform.position = startPosition;
        boss.transform.localScale = startScale;
        boss.transform.rotation = Quaternion.Euler(Vector3.zero);
        speed = baseSpeed;
        checkpoint.GetComponent<PreviewCheckpoint>().nbOfPassage = 0;
        if (player != null)
        {
            playerIsDead = false;
            player.GetComponent<PreviewPlayer>().triggerCollider.enabled = true;
            player.transform.position = playerStartPosition;
        }

        codeList.Clear();
        startPreview = false;
    }

    //Function List
    void Initialize()
    {
        methodByName.Add(BlocFunction.Function.Avancer, Avancer);
        methodByName.Add(BlocFunction.Function.Flip, Flip);
        methodByName.Add(BlocFunction.Function.Boucle, Boucle);
        methodByName.Add(BlocFunction.Function.IfPasJoueur, IfPasJoueur);
        methodByName.Add(BlocFunction.Function.EndIf, EndIf);
    }


    IEnumerator Avancer()
    {
        //bloc[index].GetComponent<Renderer>().material.SetFloat("_Thickness", 5);
        index++;
        print("Avancer");
        float elapsedTime = 0f;
        Vector3 currentPos = boss.transform.position;
        Vector3 goToPos = boss.transform.position + (Vector3.left * speed);

        while (elapsedTime < waitTime)
        {
            boss.transform.position = Vector3.Lerp(currentPos, goToPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        boss.transform.position = goToPos;

        yield return new WaitForSeconds(0.5f);

        //bloc[index-1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);
        if (index<codeList.Count)
        StartCoroutine(methodByName[codeList[index]]());
        else if (boucleOn)
        {
            index = boucleIndex;
            StartCoroutine(methodByName[codeList[index]]());
        }

        yield return null;
    }
    IEnumerator Flip()
    {
        index++;
        print("Flip");
        boss.transform.localScale = new Vector3(-boss.transform.localScale.x, boss.transform.localScale.y, boss.transform.localScale.z);
        speed = -speed;

        yield return new WaitForSeconds(0.5f);

        //bloc[index-1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);
        if (index < codeList.Count)
            StartCoroutine(methodByName[codeList[index]]());
        else if (boucleOn)
        {
            index = boucleIndex;
            StartCoroutine(methodByName[codeList[index]]());
        }

        yield return null;
    }

    IEnumerator Boucle()
    {
        boucleOn = true;
        boucleIndex = index;

        index++;
        print("Boucle");
        yield return new WaitForSeconds(0.5f);

        //bloc[index-1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);
        if (index < codeList.Count)
            StartCoroutine(methodByName[codeList[index]]());
        else if (boucleOn)
        {
            index = boucleIndex;
            StartCoroutine(methodByName[codeList[index]]());
        }
            

        yield return null;
    }

    IEnumerator IfPasJoueur()
    {

        index++;
        print("IfpasJouer");
        yield return new WaitForSeconds(0.5f);

        //bloc[index-1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);

        if (!boss.GetComponent<PreviewBoss>().playerHere)
        {
            StartCoroutine(methodByName[codeList[index]]());
        }
        else
        {
            while (codeList[index] != BlocFunction.Function.EndIf)
            {
                index++;
            }
            StartCoroutine(methodByName[codeList[index]]());

        }

        yield return null;
    }

    IEnumerator EndIf()
    {
        index++;
        print("EndIf");
        yield return new WaitForSeconds(0.5f);

        //bloc[index-1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);
        if (index < codeList.Count)
            StartCoroutine(methodByName[codeList[index]]());
        else if (boucleOn)
        {
            index = boucleIndex;
            StartCoroutine(methodByName[codeList[index]]());
        }


        yield return null;
    }

    

}
