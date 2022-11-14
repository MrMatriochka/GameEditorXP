using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakness : MonoBehaviour
{
    public Vector2 force;
    public Boss boss;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!boss.isInvincible)
            {
                Player player = other.GetComponent<Player>();
                player.Bounce(force);

                boss.hp--;
                boss.StartCoroutine(boss.InvincibleTimer());
            }
        }
    }
}
