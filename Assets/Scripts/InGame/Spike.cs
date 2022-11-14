using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public Vector2 force;
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

                float playerPos = player.transform.position.x - transform.position.x;
                if(playerPos>0)
                {
                    player.Bounce(force);
                }
                else
                {
                    player.Bounce(new Vector2(-force.x,force.y));
                }
            }
        }
    }
}
