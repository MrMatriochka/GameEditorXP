using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 force;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (!player.isInvincible)
            {
                player.hp--;
                player.StartCoroutine(player.InvincibleTimer());
                player.UpdateLife();

                float playerPos = player.transform.position.x - transform.position.x;
                if (playerPos > 0)
                {
                    player.Bounce(force);
                }
                else
                {
                    player.Bounce(new Vector2(-force.x, force.y));
                }
            }
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
