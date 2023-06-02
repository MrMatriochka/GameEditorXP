using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakness : MonoBehaviour
{
    public Vector2 force;
    public Animator enemy;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHazard"))
        {
                Player player = other.GetComponentInParent<Player>();
                player.Bounce(force);
                enemy.SetBool("IsDead", true);
            if(transform.parent.GetComponent<Bub>() != null)
            {
                transform.parent.GetComponent<Collider2D>().enabled = false;
                transform.parent.GetComponent<Rigidbody2D>().simulated = false;
                transform.parent.GetComponent<Bub>().enabled = false;
            }

            if (transform.parent.GetComponent<Boss>() != null)
            {
                transform.parent.GetComponent<Rigidbody2D>().simulated = false;
                transform.parent.GetComponent<Boss>().enabled = false;
            }
        }
    }
}
