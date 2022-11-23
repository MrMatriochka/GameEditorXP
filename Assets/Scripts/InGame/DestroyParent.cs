using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    public Vector2 force;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHazard"))
        {
            Player player = other.GetComponentInParent<Player>();
            transform.parent.gameObject.SetActive(false);
            player.Bounce(force);
        }
    }
}
