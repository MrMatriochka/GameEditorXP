using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewBoss : MonoBehaviour
{
    public LayerMask player;
    public Collider2D triggerCollider;
    [HideInInspector]public bool playerHere;
    void FixedUpdate()
    {
        if (triggerCollider.IsTouchingLayers(player))
        {
            playerHere = true;
        }
        else { playerHere = false; }

    }
}
