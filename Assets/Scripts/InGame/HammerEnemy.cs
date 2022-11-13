using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerEnemy : MonoBehaviour
{
    public Vector2 throwing;
    public float attackSpeed;
    float startScale;

    public GameObject hammer;

    private GameObject player;
    private bool canThrow = true;

    public float moveSpeed = 1f;
    public LayerMask ground;

    private Rigidbody2D rb;
    public Collider2D triggerCollider;
    private void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
        startScale = transform.localScale.x;
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        canThrow = true;
    }
    void Update()
    {
        float playerPos = player.transform.position.x - transform.position.x;
        if(playerPos >0)
        {
            transform.localScale = new Vector2(startScale, transform.localScale.y);
            if (triggerCollider.IsTouchingLayers(ground))
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
        }
        if (playerPos < 0)
        {
            transform.localScale = new Vector2(startScale * -1, transform.localScale.y);
            if (triggerCollider.IsTouchingLayers(ground))
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
        }

        if(canThrow)
        {
            canThrow = false;
            StartCoroutine(Throw());
        }
    }

    IEnumerator Throw()
    {
        GameObject projectile = Instantiate(hammer, transform.position, transform.rotation);
        float playerPos = player.transform.position.x - transform.position.x;
        if (playerPos < 0)
        {
            projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-throwing.x,throwing.y), ForceMode2D.Impulse);
        }
        else
        {
            projectile.GetComponent<Rigidbody2D>().AddForce(throwing, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(attackSpeed);
        canThrow = true;
        yield return null;
    }

}
