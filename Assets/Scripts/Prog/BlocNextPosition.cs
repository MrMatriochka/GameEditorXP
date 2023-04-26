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
        if (other.CompareTag("Bloc") &&  (BlocEditor.pendingObj == assembler.gameObject || assembler.lastOfThePendingBloc) && other.GetComponent<BlocAssemble>().type != BlocAssemble.BlocType.Start)
        {
            assembler.canAssembleNext = true;
            assembler.collidingBloc = other.gameObject;
            assembler.collidingNextBloc = other.gameObject;

            other.GetComponent<BlocAssemble>().previousBlocPosition.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bloc"))
        {
            assembler.canAssembleNext = false;
            assembler.collidingBloc = null;
            assembler.collidingNextBloc = null;

            if (other.GetComponent<BlocAssemble>().type != BlocAssemble.BlocType.Start)
                other.GetComponent<BlocAssemble>().previousBlocPosition.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
