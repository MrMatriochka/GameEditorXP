using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movingSpeed;
    public float jumpForce;
    private float moveInput;

    private bool facingRight = false;

    private bool isGrounded;
    public Transform groundCheck;

    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            rigidbody.velocity = new Vector2(moveInput * movingSpeed, rigidbody.velocity.y);
        } 
            
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
        
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
        isGrounded = colliders.Length > 1;
    }
}
