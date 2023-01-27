using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocAssemble : MonoBehaviour
{
    Collider2D collision;
    public LayerMask layer;
    [HideInInspector] public Transform touchingObj;

    private void Start()
    {
        collision = GetComponent<Collider2D>();
    }
    public bool CanAssemble()
    {
        if(collision.IsTouchingLayers(layer))
        {
            return true;
        }
        else { return false;  }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("BlocPos"))
        {
            touchingObj = other.transform;
        }
    }
}
