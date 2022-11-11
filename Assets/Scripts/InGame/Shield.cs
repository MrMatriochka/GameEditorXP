using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float time;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (!player.isInvincible)
            {
                player.StartCoroutine(player.InvincibleTimer(time));
            }
            gameObject.SetActive(false);
        }
    }
}
