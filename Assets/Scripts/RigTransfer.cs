using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigTransfer : MonoBehaviour
{
    public GameObject body;
    public GameObject head;
    public GameObject handL;
    public GameObject handR;
    public GameObject feetL;
    public GameObject feetR;

    void Start()
    {
        int startChildCount = transform.childCount;
        for (int i = 3; i < startChildCount; i++)
        {
            string part = transform.GetChild(3).GetComponent<RigTag>().bodyPart;
            if(part == "Sprite_Body" || part == "")
            {
                transform.GetChild(3).parent = body.transform;
            }
            if (part == "Sprite_Head")
            {
                transform.GetChild(3).parent = head.transform;
            }
            if (part == "Sprite_HandL")
            {
                transform.GetChild(3).parent = handL.transform;
            }
            if (part == "Sprite_HandR")
            {
                transform.GetChild(3).parent = handR.transform;
            }
            if (part == "Sprite_FeetL")
            {
                transform.GetChild(3).parent = feetL.transform;
            }
            if (part == "Sprite_FeetR")
            {
                transform.GetChild(3).parent = feetR.transform;
            }
        }
    }
}
