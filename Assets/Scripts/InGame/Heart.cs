using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if(player.hp != player.maxHp)
            {
                player.hp++;
                player.UpdateLife();
            }       
            gameObject.SetActive(false);
        }
    }
}
