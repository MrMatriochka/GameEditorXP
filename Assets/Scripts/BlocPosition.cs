using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocPosition : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bloc"))
        {
            //other.GetComponent<BlocAssemble>().touchingObj = transform;
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bloc"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void Update()
    {
        if(transform.childCount == 0)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }

        if(BlocEditor.pendingObj == null)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        if (BlocEditor.pendingObj == transform.parent.gameObject)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
