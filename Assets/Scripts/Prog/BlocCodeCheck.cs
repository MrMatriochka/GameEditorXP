using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BlocCodeCheck : MonoBehaviour
{
    public List<BlocFunction.Function> codeList = new List<BlocFunction.Function>();
    List<GameObject> bloc = new List<GameObject>();
    public List<GameObject> ennemyList = new List<GameObject>();
    GameObject pendingObj;

    Vector3 startPosition;
    Vector3 startScale;
    public Dictionary<BlocFunction.Function, Methods> methodByName = new Dictionary<BlocFunction.Function, Methods>();
    public delegate IEnumerator Methods();

    public GameObject boss;
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

        Initialize();
    }

    public void CreateCodeList()
    {

        ResetPreview();
        codeList.Clear();
        bloc.Clear();

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
                    bloc.Add(pendingObj.transform.GetChild(0).gameObject);
                }

                if (pendingObj.GetComponent<BlocAssemble>().nextBloc == null && pendingObj.GetComponent<BlocAssemble>().midBloc != null)
                {
                    index = 0;
                    boucleOn = false;
                    CodeString();
                    StartCoroutine(methodByName[codeList[0]]());
                    return;
                }
                    
                if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
                
            }


            codeList.Add(pendingObj.GetComponent<BlocFunction>().function);
            bloc.Add(pendingObj);
            if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
            {
                codeList.Add(BlocFunction.Function.EndIf);
                bloc.Add(pendingObj.transform.GetChild(0).gameObject);
            }   

            index = 0;
            boucleOn = false;
            CodeString();
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
                    bloc.Add(pendingObj.transform.GetChild(0).gameObject);
                }

                if (pendingObj.GetComponent<BlocAssemble>().nextBloc == null && pendingObj.GetComponent<BlocAssemble>().midBloc != null)
                    return;
                if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                {
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
                }
            }
            

            codeList.Add(pendingObj.GetComponent<BlocFunction>().function);
            bloc.Add(pendingObj);
            if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                codeList.Add(BlocFunction.Function.EndIf);
        }
    }

    public void ResetPreview()
    {
        StopAllCoroutines();
        boss.GetComponent<Animator>().SetBool("IsWalking", false);
        boss.transform.position = startPosition;
        boss.transform.localScale = startScale;
        boss.transform.rotation = Quaternion.Euler(Vector3.zero);
        boss.GetComponent<PreviewBoss>().facing = 1;
        speed = baseSpeed;
        checkpoint.GetComponent<PreviewCheckpoint>().enemyCount = checkpoint.GetComponent<PreviewCheckpoint>().enemyTotal;
        checkpoint.GetComponent<PreviewCheckpoint>().checkpointCount = checkpoint.GetComponent<PreviewCheckpoint>().checkpointTotal;
        foreach(Transform obj in checkpoint.transform)
        {
            obj.gameObject.SetActive(true);
        }
        foreach(GameObject obj in ennemyList)
        {
            obj.SetActive(true);
        }
        foreach(GameObject obj in bloc)
        {
            if(obj != null)
            obj.GetComponent<Renderer>().material.SetFloat("_Thickness", 0);
        }

    }

    //Function List
    void Initialize()
    {
        methodByName.Add(BlocFunction.Function.Avancer, Avancer);
        methodByName.Add(BlocFunction.Function.Attaquer, Attaquer);
        methodByName.Add(BlocFunction.Function.Flip, Flip);
        methodByName.Add(BlocFunction.Function.Boucle, Boucle);
        methodByName.Add(BlocFunction.Function.IfJoueur, IfJoueur);
        methodByName.Add(BlocFunction.Function.IfObstacle, IfObstacle);
        methodByName.Add(BlocFunction.Function.EndIf, EndIf);
    }


    IEnumerator Avancer()
    {
        bloc[index].GetComponent<Renderer>().material.SetFloat("_Thickness", 1.5f);
        index++;
        print(index + ": Avancer");
        float elapsedTime = 0f;
        Vector3 currentPos = boss.transform.position;
        Vector3 goToPos = boss.transform.position + (Vector3.left * speed);

        boss.GetComponent<Animator>().SetBool("IsWalking", true);

        while (elapsedTime < waitTime)
        {
            boss.transform.position = Vector3.Lerp(currentPos, goToPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        boss.transform.position = goToPos;
        boss.GetComponent<Animator>().SetBool("IsWalking", false);
        yield return new WaitForSeconds(0.5f);

        bloc[index-1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);
        if (index<codeList.Count)
        StartCoroutine(methodByName[codeList[index]]());
        else if (boucleOn)
        {
            index = boucleIndex;
            StartCoroutine(methodByName[codeList[index]]());
        }

        yield return null;
    }

    IEnumerator Attaquer()
    {
        bloc[index].GetComponent<Renderer>().material.SetFloat("_Thickness", 1.5f);
        index++;
        print(index + ": Attaquer");

        //boss.GetComponent<PreviewBoss>().Attack();
        boss.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(1f);

        bloc[index - 1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);
        if (index < codeList.Count)
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
        bloc[index].GetComponent<Renderer>().material.SetFloat("_Thickness", 1.5f);
        index++;
        print(index + ": Flip");
        boss.transform.localScale = new Vector3(-boss.transform.localScale.x, boss.transform.localScale.y, boss.transform.localScale.z);
        speed = -speed;
        boss.GetComponent<PreviewBoss>().facing *= -1;

        yield return new WaitForSeconds(0.5f);

        bloc[index-1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);
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

        bloc[index].GetComponent<Renderer>().material.SetFloat("_Thickness", 1.5f);
        index++;
        print(index + ": Boucle");
        yield return new WaitForSeconds(0.5f);

        bloc[index-1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);
        if (index < codeList.Count)
            StartCoroutine(methodByName[codeList[index]]());
        else if (boucleOn)
        {
            index = boucleIndex;
            StartCoroutine(methodByName[codeList[index]]());
        }
            

        yield return null;
    }


    IEnumerator IfJoueur()
    {
        bloc[index].GetComponent<Renderer>().material.SetFloat("_Thickness", 1.5f);
        index++;
        print("IfJoueur");
        yield return new WaitForSeconds(0.5f);

        bloc[index - 1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);

        if (boss.GetComponent<PreviewBoss>().seePlayer)
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

    IEnumerator IfObstacle()
    {
        bloc[index].GetComponent<Renderer>().material.SetFloat("_Thickness", 1.5f);
        index++;
        print(index+": IfObstacle");
        yield return new WaitForSeconds(0.5f);

        bloc[index - 1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);

        if (boss.GetComponent<PreviewBoss>().hole)
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
        bloc[index].GetComponent<Renderer>().material.SetFloat("_Thickness", 1.5f);
        index++;
        print(index + ": EndIf");
        yield return new WaitForSeconds(0.5f);

        bloc[index-1].GetComponent<Renderer>().material.SetFloat("_Thickness", 0);

        if (index < codeList.Count)
            StartCoroutine(methodByName[codeList[index]]());
        else if (boucleOn)
        {
            index = boucleIndex;
            StartCoroutine(methodByName[codeList[index]]());
        }


        yield return null;
    }


    public TMP_Text codeText;
    public void CodeString()
    {
        string code = "Start() \n{\n";
        int tabNb = 1;
        foreach (BlocFunction.Function line in codeList)
        {
            if (line == BlocFunction.Function.EndIf)
                tabNb--;

            for (int i = 0; i < tabNb; i++)
            {
                code += "\t";
            }
            
            if(line == BlocFunction.Function.EndIf)
            {
                code += "}\n";
            }   
            else if(line == BlocFunction.Function.IfJoueur)
            {
                code += "If(Joueur)\n";
                for (int i = 0; i < tabNb; i++)
                {
                    code += "\t";
                }
                code += "{\n";
                tabNb++;
            }
            else if (line == BlocFunction.Function.IfObstacle)
            {
                code += "If(Obstacle)\n";
                for (int i = 0; i < tabNb; i++)
                {
                    code += "\t";
                }
                code += "{\n";
                tabNb++;
            }
            else if (line == BlocFunction.Function.Boucle)
            {
                code += "Boucle()\n";
                for (int i = 0; i < tabNb; i++)
                {
                    code += "\t";
                }
                code += "{\n";
                tabNb++;
            }
            else
            {
                code += line.ToString() + "();\n";
            }
          
        }
        code += "}";
        codeText.text = code;
    }

}
