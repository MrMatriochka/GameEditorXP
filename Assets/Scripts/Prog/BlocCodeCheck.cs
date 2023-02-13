using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocCodeCheck : MonoBehaviour
{
    public List<BlocFunction.Function> codeList = new List<BlocFunction.Function>();
    GameObject pendingObj;

    bool startPreview;
    Vector3 startPosition;
    Vector3 startScale;
    public Dictionary<BlocFunction.Function, Methods> methodByName = new Dictionary<BlocFunction.Function, Methods>();
    public delegate void Methods();

    public GameObject boss;
    public GameObject player;
    public float speed;
    public float baseSpeed;
    [HideInInspector] public bool playerIsDead;
    Vector3 playerStartPosition;
    int index;
    private void Start()
    {
        startPosition = boss.transform.position;
        startScale = boss.transform.localScale;
        baseSpeed = speed;

        if (player != null) playerStartPosition = player.transform.position;
        Initialize();
    }
    private void Update()
    {
        if (startPreview)
        {
            index = 0;
            while (index < codeList.Count)
            {
                BlocFunction.Function bloc = codeList[index];
                index++;
                methodByName[bloc]();
                
            }


        }
    }
    public void CreateCodeList()
    {
        

        ResetPreview();
        startPreview = true;
        pendingObj = gameObject;
        //codeList.Add(pendingObj.name);
        
        if(pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
        {
            pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
           
            while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
            {
                codeList.Add(pendingObj.GetComponent<BlocFunction>().function);

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
            if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                codeList.Add(BlocFunction.Function.EndIf);
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
        methodByName.Add(BlocFunction.Function.IfObstacle, IfObstacle);
        methodByName.Add(BlocFunction.Function.IfToucheLeJoueur, IfToucheLeJoueur);
        methodByName.Add(BlocFunction.Function.PertePV, PertePV);
        methodByName.Add(BlocFunction.Function.EndIf, EndIf);
    }

    void Avancer()
    {
        boss.transform.position -= transform.right*speed*Time.deltaTime;
    }
    void Flip()
    {
        boss.transform.localScale = new Vector3(-boss.transform.localScale.x, boss.transform.localScale.y, boss.transform.localScale.z);
        speed = -speed;
    }

    void IfObstacle()
    {

            bool checkIf = true;
            while (checkIf)
            {
                if(codeList[index] == BlocFunction.Function.EndIf)
                {
                    checkIf = false;
                    index++;
                    return;
                }
                BlocFunction.Function bloc = codeList[index];
                index++;

                if (!boss.GetComponent<PreviewBoss>().isGrounded)
                methodByName[bloc]();
            }
    }

    void EndIf()
    {
        return;
    }

    void PertePV()
    {
        player.GetComponent<PreviewPlayer>().IsHurted();
    }

    void IfToucheLeJoueur()
    {
        bool checkIf = true;
        while (checkIf)
        {
            if (codeList[index] == BlocFunction.Function.EndIf)
            {
                checkIf = false;
                index++;
                return;
            }

            BlocFunction.Function bloc = codeList[index];
            index++;

            if (player.GetComponent<PreviewPlayer>().touchBoss)
                methodByName[bloc]();
        }
    }
}
