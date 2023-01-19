using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigDetection : MonoBehaviour
{
    public Collider2D body;
    public Collider2D head;
    public Collider2D handL;
    public Collider2D handR;
    public Collider2D feetL;
    public Collider2D feetR;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Sprite"))
        {
            if (collision.IsTouching(body))
            {
                collision.GetComponent<RigTag>().bodyPart = "Sprite_Body";
            }
            if (collision.IsTouching(head))
            {
                collision.GetComponent<RigTag>().bodyPart = "Sprite_Head";
            }
            if (collision.IsTouching(handL))
            {
                collision.GetComponent<RigTag>().bodyPart = "Sprite_HandL";
            }
            if (collision.IsTouching(handR))
            {
                collision.GetComponent<RigTag>().bodyPart = "Sprite_HandR";
            }
            if (collision.IsTouching(feetL))
            {
                collision.GetComponent<RigTag>().bodyPart = "Sprite_FeetL";
            }
            if (collision.IsTouching(feetR))
            {
                collision.GetComponent<RigTag>().bodyPart = "Sprite_FeetR";
            }
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    collision.GetComponent<RigTag>().bodyPart = "";
    //}
}
