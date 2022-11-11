using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //GameObject.FindObjectOfType<BuildingManager>().Stop();
            Player player = other.GetComponent<Player>();
            if (!player.isInvincible)
            {
                player.hp--;
                player.StartCoroutine(player.InvincibleTimer());
                player.UpdateLife();
            }
        }
    }
}
