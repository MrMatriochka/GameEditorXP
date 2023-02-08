using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocAssemble : MonoBehaviour
{
     public GameObject previousBloc;
     public GameObject midBloc;
     public GameObject nextBloc;
     
    [HideInInspector] public bool canAssembleNext;
     public bool canAssemblePrevious;
    public GameObject collidingBloc;

    public GameObject previousBlocPosition;
    public GameObject nextBlocPosition;
    public GameObject midBlocPosition;

    [HideInInspector] public bool lastOfThePendingBloc;
   
    public bool collidingWithBlocEnd;
    public float decalageBlocIf;
    public GameObject bot;
    Vector3 botBasePos;
    Vector3 nextBlocPosBasePos;
    Vector3 midBaseBlocPosBasePos;

    public GameObject midBase;

    public enum BlocType
    {
        Normal,
        If,
        Boucle
    }

    public BlocType type;
    private void Start()
    {
        if(type == BlocType.If)
        {

            botBasePos = bot.transform.localPosition;
            nextBlocPosBasePos = nextBlocPosition.transform.localPosition;
            midBaseBlocPosBasePos = midBase.transform.localPosition;
        }

        if (type == BlocType.Boucle)
        {

            botBasePos = bot.transform.localPosition;
            midBaseBlocPosBasePos = midBase.transform.localPosition;
        }
    }
    private void Update()
    {
        if (BlocEditor.pendingObj == gameObject || lastOfThePendingBloc)
        {
            previousBlocPosition.GetComponent<BlocPreviousPosition>().enabled = true;
            nextBlocPosition.GetComponent<BlocNextPosition>().enabled = true;
        }
        else if(type != BlocType.Boucle)
        {
            previousBlocPosition.GetComponent<BlocPreviousPosition>().enabled = false;
            nextBlocPosition.GetComponent<BlocNextPosition>().enabled = false;
        }


        if (previousBloc != null)
        {
            if (type == BlocType.If)
            {
                transform.position = previousBloc.GetComponent<BlocAssemble>().nextBlocPosition.transform.position + (Vector3.right * decalageBlocIf);
            }
            else
            {
                transform.position = previousBloc.GetComponent<BlocAssemble>().nextBlocPosition.transform.position;
            }

            if (previousBloc.GetComponent<BlocAssemble>().type == BlocType.If && previousBloc.GetComponent<BlocAssemble>().midBloc !=null)
            {
                if(previousBloc.GetComponent<BlocAssemble>().midBloc == gameObject)
                {
                    if (type == BlocType.If)
                    {
                        transform.position = previousBloc.GetComponent<BlocAssemble>().midBlocPosition.transform.position + (Vector3.right * decalageBlocIf);
                    }
                    else
                    {
                        transform.position = previousBloc.GetComponent<BlocAssemble>().midBlocPosition.transform.position;
                    }
                }
                
            }
            
            previousBlocPosition.SetActive(false);
        }
        else if (type != BlocType.Boucle)
        {
            previousBlocPosition.SetActive(true);
        }

        if (nextBloc != null)
        {
            nextBlocPosition.SetActive(false);
        }
        else
        {
            nextBlocPosition.SetActive(true);
        }

        if (type == BlocType.If)
        {
            //resize
            //if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse0))
            //{
            //    float nbOfMidBloc = GetNumberOfMidBloc();
            //    bot.transform.localPosition = botBasePos - new Vector3(0, 0.75f * nbOfMidBloc, 0);
            //    nextBlocPosition.transform.localPosition = nextBlocPosBasePos - new Vector3(0, 0.75f * nbOfMidBloc, 0);

            //    midBase.transform.localPosition = midBaseBlocPosBasePos - new Vector3(0, 0.4f * nbOfMidBloc, 0);
            //    midBase.transform.localScale = new Vector3(1, (nbOfMidBloc / 2) + 1, 1);
            //}


            if (midBloc != null)
            {
                midBlocPosition.SetActive(false);
                
            }
            else
            {
                midBlocPosition.SetActive(true);
            }
        }

        //if (type == BlocType.Boucle)
        //{
        //    if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse0))
        //    {
        //        float nbOfMidBloc = GetNumberOfMidBloc();
        //        bot.transform.localPosition = botBasePos - new Vector3(0, 0.75f * nbOfMidBloc, 0);

        //        midBase.transform.localPosition = midBaseBlocPosBasePos - new Vector3(0, 0.4f * nbOfMidBloc, 0);
        //        midBase.transform.localScale = new Vector3(1, (nbOfMidBloc / 2) + 1, 1);
        //    }
        //}
    }

    public List<GameObject> codeList = new List<GameObject>();
    GameObject pendingObj;
    int GetNumberOfMidBloc()
    {
        if (type == BlocType.Boucle && nextBloc !=null)
            midBloc = nextBloc;


        if (midBloc != null)
        {
            codeList.Clear();
            pendingObj = midBloc;
            ReadBlocIf();


            
            int objCount = 0;
            foreach(GameObject obj in codeList)
            {
                if (obj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If) objCount += 3;
                else objCount++;
            }
            return objCount;
        }

        return 0;
    }
    void ReadBlocIf()
    {

            while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
            {
                codeList.Add(pendingObj);

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
            codeList.Add(pendingObj);
        
    }
}
