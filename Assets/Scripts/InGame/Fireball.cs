using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 1;
    public Vector2 force;
    Collider2D triggerCollider;
    public LayerMask walls;
    private void Start()
    {
        if (speed < 0)
            transform.localScale *= -1;

        triggerCollider = GetComponent<Collider2D>();
    }
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if(triggerCollider.IsTouchingLayers(walls)|| triggerCollider.IsTouchingLayers(walls))
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
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
    }
}
