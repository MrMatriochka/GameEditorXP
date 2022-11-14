using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakness : MonoBehaviour
{
    public Boss boss;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!boss.isInvincible)
            {
                boss.hp--;
                boss.StartCoroutine(boss.InvincibleTimer());
            }
        }
    }
}
