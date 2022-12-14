using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakness : MonoBehaviour
{
    public Vector2 force;
    public Boss boss;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHazard"))
        {
            if (!boss.isInvincible)
            {
                Player player = other.GetComponentInParent<Player>();
                player.Bounce(force);

                boss.hp--;
                boss.StartCoroutine(boss.InvincibleTimer());
            }
        }
    }
}
