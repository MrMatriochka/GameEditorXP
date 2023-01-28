using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocNextPosition : MonoBehaviour
{
    public bool canAssemble;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bloc"))
        {
            
            GetComponent<SpriteRenderer>().enabled = true;
            
            if (BlocEditor.pendingObj != other.gameObject && !other.transform.GetChild(0).GetComponent<BlocPreviousPosition>().isOccuupied)
            {
                canAssemble = true;
                transform.parent.GetComponent<BlocAssemble>().touchingObj = other.transform.GetChild(0);
                other.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bloc"))
        {
            canAssemble = false;
            GetComponent<SpriteRenderer>().enabled = false;
            other.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }

        if (BlocEditor.pendingObj == null)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        if (BlocEditor.pendingObj == transform.parent.gameObject)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
