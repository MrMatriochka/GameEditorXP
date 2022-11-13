using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public float jumpHeight = 10f;
    public float jumpDistance = 10f;
    public float jumpDelay = 2;
    public LayerMask ground;

    private Rigidbody2D rigidbody;
    public Collider2D triggerCollider;

    private bool isGrounded;
    public Transform groundCheck;

    private bool canJump = true;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!triggerCollider.IsTouchingLayers(ground) && canJump)
        {
            Flip();
        }
        CheckGround();
        if (isGrounded && canJump)
        {
            StartCoroutine(Jump());
            canJump = false;
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        jumpDistance *= -1;
    }

    IEnumerator Jump()
    {
        rigidbody.AddForce(new Vector2(jumpDistance, jumpHeight), ForceMode2D.Impulse);
        yield return new WaitForSeconds(jumpDelay);
        canJump = true;
        yield return null;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
        isGrounded = colliders.Length > 1;
    }
}
