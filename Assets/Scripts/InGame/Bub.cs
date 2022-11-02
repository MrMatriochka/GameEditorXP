using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bub : MonoBehaviour
{
    public float moveSpeed = 1f;
    public LayerMask ground;

    private Rigidbody2D rigidbody;
    public Collider2D triggerCollider;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidbody.velocity = new Vector2(moveSpeed, rigidbody.velocity.y);
    }

    void FixedUpdate()
    {
        if (!triggerCollider.IsTouchingLayers(ground))
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
