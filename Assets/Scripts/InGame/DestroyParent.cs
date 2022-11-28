using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    public void DeathFunction()
    {
        transform.parent.gameObject.SetActive(false);
    }

}
