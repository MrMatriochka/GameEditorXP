using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocPreviousPosition : MonoBehaviour
{
    BlocAssemble assembler;

    private void Start()
    {
        assembler = transform.parent.gameObject.GetComponent<BlocAssemble>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Bloc") && other.GetComponent<BlocAssemble>().type != BlocAssemble.BlocType.If && BlocEditor.pendingObj == assembler.gameObject)
        {
            assembler.canAssemblePrevious = true;
            assembler.collidingBloc = other.gameObject;
            other.GetComponent<BlocAssemble>().nextBlocPosition.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (other.CompareTag("Bloc") && other.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If && BlocEditor.pendingObj == assembler.gameObject)
        {
            assembler.canAssemblePrevious = true;
            assembler.collidingBloc = other.gameObject;
            other.GetComponent<BlocAssemble>().midBlocPosition.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (other.CompareTag("BlocEnd") &&  BlocEditor.pendingObj == assembler.gameObject)
        {
            assembler.canAssemblePrevious = true;
            assembler.collidingWithBlocEnd = true;
            assembler.collidingBloc = other.transform.parent.gameObject;
            other.transform.parent.GetComponent<BlocAssemble>().nextBlocPosition.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bloc"))
        {
            assembler.canAssemblePrevious = false;
            assembler.collidingBloc = null;
            if(other.GetComponent<BlocAssemble>().type == BlocAssemble.BlocType.If)
            {
                other.GetComponent<BlocAssemble>().midBlocPosition.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                other.GetComponent<BlocAssemble>().nextBlocPosition.GetComponent<SpriteRenderer>().enabled = false;
            }
            
        }

        if (other.CompareTag("BlocEnd"))
        {

            assembler.canAssemblePrevious = false;
            assembler.collidingWithBlocEnd = false;


            assembler.collidingBloc = null;

            other.transform.parent.GetComponent<BlocAssemble>().nextBlocPosition.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

}
