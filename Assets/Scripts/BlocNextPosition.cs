using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocNextPosition : MonoBehaviour
{
    BlocAssemble assembler;
    private void Start()
    {
        assembler = transform.parent.gameObject.GetComponent<BlocAssemble>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bloc") &&  (BlocEditor.pendingObj == assembler.gameObject || assembler.lastOfThePendingBloc))
        {
            assembler.canAssembleNext = true;
            assembler.collidingBloc = other.gameObject;
            other.GetComponent<BlocAssemble>().previousBlocPosition.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bloc"))
        {
            assembler.canAssembleNext = false;
            assembler.collidingBloc = null;
            other.GetComponent<BlocAssemble>().previousBlocPosition.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
