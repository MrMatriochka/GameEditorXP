using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocCodeCheck : MonoBehaviour
{
    public List<string> codeList = new List<string>();

    public void CreateCodeList()
    {
        codeList.Clear();

        GameObject pendingObj = gameObject;
        codeList.Add(pendingObj.name);
        bool ifDone = false;
        if(pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
        {
            pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
            while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null || pendingObj.GetComponent<BlocAssemble>().midBloc != null)
            {
                codeList.Add(pendingObj.name);
                if (pendingObj.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If && pendingObj.GetComponent<BlocAssemble>().midBloc != null && !ifDone)
                {
                    GameObject ifBloc = pendingObj;
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().midBloc;
                    codeList.Add(pendingObj.name);
                    while (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                    {
                        
                        codeList.Add(pendingObj.name);
                        pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;
                    }
                    codeList.Add("IfEnd");
                    //codeList.Add(pendingObj.name);
                    pendingObj = ifBloc;
                    //pendingObj.GetComponent<BlocAssemble>().midBloc = null;
                    ifDone = true;
                }
                ifDone = false;
                if (pendingObj.GetComponent<BlocAssemble>().nextBloc != null)
                    pendingObj = pendingObj.GetComponent<BlocAssemble>().nextBloc;


            }
            codeList.Add(pendingObj.name);
        }

        
        
        print(codeList.Count);
    }

    void Test()
    {
        Test();
    }
}
