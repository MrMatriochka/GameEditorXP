using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocAssemble : MonoBehaviour
{
    public GameObject previousBloc;
     public GameObject midBloc;
     public GameObject nextBloc;
     

    [HideInInspector] public bool canAssembleNext;
    [HideInInspector] public bool canAssemblePrevious;
    [HideInInspector] public GameObject collidingBloc;

    public GameObject previousBlocPosition;
    public GameObject nextBlocPosition;
    public GameObject midBlocPosition;

    [HideInInspector] public bool lastOfThePendingBloc;

    public bool isBlocIf;
    public bool collidingWithBlocEnd;
    public float decalageBlocIf;
    public GameObject bot;
    Vector3 botBasePos;
    Vector3 nextBlocPosBasePos;
    Vector3 midBaseBlocPosBasePos;

    public GameObject midBase;
    

    private void Start()
    {
        if(isBlocIf)
        {

            botBasePos = bot.transform.localPosition;
            nextBlocPosBasePos = nextBlocPosition.transform.localPosition;
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
        else
        {
            previousBlocPosition.GetComponent<BlocPreviousPosition>().enabled = false;
            nextBlocPosition.GetComponent<BlocNextPosition>().enabled = false;
        }


        if (previousBloc != null)
        {
            if (isBlocIf)
            {
                transform.position = previousBloc.GetComponent<BlocAssemble>().nextBlocPosition.transform.position + (Vector3.right * decalageBlocIf);
            }
            else
            {
                transform.position = previousBloc.GetComponent<BlocAssemble>().nextBlocPosition.transform.position;
            }

            if (previousBloc.GetComponent<BlocAssemble>().isBlocIf && previousBloc.GetComponent<BlocAssemble>().midBloc !=null)
            {
                if(previousBloc.GetComponent<BlocAssemble>().midBloc == gameObject)
                {
                    if (isBlocIf)
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
        else
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

        if (isBlocIf)
        {
            //resize
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse0))
            {
                float nbOfMidBloc = GetNumberOfMidBloc();
                bot.transform.localPosition = botBasePos - new Vector3(0, 0.75f * nbOfMidBloc, 0);
                nextBlocPosition.transform.localPosition = nextBlocPosBasePos - new Vector3(0, 0.75f * nbOfMidBloc, 0);

                midBase.transform.localPosition = midBaseBlocPosBasePos - new Vector3(0, 0.4f * nbOfMidBloc, 0);
                midBase.transform.localScale = new Vector3(1,(nbOfMidBloc/2)+1,1);
            }


            if (midBloc != null)
            {
                midBlocPosition.SetActive(false);
                
            }
            else
            {
                midBlocPosition.SetActive(true);
            }
        }
        
    }

    int GetNumberOfMidBloc()
    {
        if (midBloc != null)
        {
            GameObject lastObj = midBloc;
            int objCount = 1;
            while (lastObj.GetComponent<BlocAssemble>().nextBloc != null)
            {
                objCount++;
                lastObj = lastObj.GetComponent<BlocAssemble>().nextBloc;
            }
            return objCount;
        }

        return 0;
    }
}
