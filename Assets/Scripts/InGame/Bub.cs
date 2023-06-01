using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bub : MonoBehaviour
{
    public float moveSpeed = 1f;
    public LayerMask ground;
    public LayerMask ennemi;

    private Rigidbody2D rb;
    public Collider2D triggerColliderGround;
    public Collider2D triggerColliderWall;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void FixedUpdate()
    {
        if (!triggerColliderGround.IsTouchingLayers(ground) || triggerColliderWall.IsTouchingLayers(ennemi))
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
    }
}
