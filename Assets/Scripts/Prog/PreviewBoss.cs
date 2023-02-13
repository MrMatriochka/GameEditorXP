using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewBoss : MonoBehaviour
{
    public LayerMask ground;
    public Collider2D triggerCollider;
    [HideInInspector]public bool isGrounded;
    void FixedUpdate()
    {
        if (triggerCollider.IsTouchingLayers(ground))
        {
            isGrounded = true;
        }
        else { isGrounded = false; }

    }
}
