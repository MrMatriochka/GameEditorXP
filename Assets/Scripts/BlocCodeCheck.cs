using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocCodeCheck : MonoBehaviour
{
    public List<string> codeList = new List<string>();
    GameObject pendingObj;
    public void CreateCodeList()
    {
        codeList.Clear();

        pendingObj = gameObject;
        codeList.Add(pendingObj.name);
        
        if(pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
        {
            pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
           
            while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
            {
                codeList.Add(pendingObj.name);

                if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                {
                    GameObject saveIf = pendingObj;
                    ReadBlocIf();
                    pendingObj = saveIf;
                    codeList.Add("IfEnd");
                }

                if (pendingObj.GetComponent<BlocAssemble>().nextBloc == null && pendingObj.GetComponent<BlocAssemble>().midBloc != null)
                    return;
                if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
                
            }
            codeList.Add(pendingObj.name);
        }

    }

    void ReadBlocIf()
    {
        if(pendingObj.GetComponent<BlocAssemble>().midBloc != null)
        {
            pendingObj = pendingObj.GetComponent<BlocAssemble>().midBloc;
            while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
            {
                codeList.Add(pendingObj.name);

                if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
                {
                    GameObject saveIf = pendingObj;
                    ReadBlocIf();
                    pendingObj = saveIf;
                    codeList.Add("IfEnd");
                }

                if (pendingObj.GetComponent<BlocAssemble>().nextBloc == null && pendingObj.GetComponent<BlocAssemble>().midBloc != null)
                    return;
                if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
                

            }
            codeList.Add(pendingObj.name);
        }
    }
}
