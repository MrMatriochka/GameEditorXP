using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movingSpeed;
    public float jumpForce;
    private float moveInput;

    private bool facingRight = false;

    private bool isGrounded;
    public Transform groundCheck;

    private Rigidbody2D rb;
    private SpriteRenderer renderer;

    [HideInInspector] public int score;
    private Text scoreUI;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        scoreUI = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            Vector3 direction = transform.right * moveInput;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
        } 
            
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
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
        renderer.flipX = !renderer.flipX;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
        isGrounded = colliders.Length > 1;
    }

    public void UpdateScore()
    {
        scoreUI.text = score.ToString();
    }
}
