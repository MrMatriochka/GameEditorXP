using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float jumpHeight = 10f;
    public float jumpDistance = 10f;
    public float jumpDelay = 2;
    public LayerMask ground;
    public float invincibilityTime;
    public int maxHp;
    [HideInInspector] public int hp;
    [HideInInspector] public bool isInvincible;
    private SpriteRenderer renderer;
    private GameObject player;
    private Rigidbody2D rb;
    public Collider2D triggerCollider;

    private bool isGrounded;
    public Transform groundCheck;

    float startScale;
    private bool canJump = true;
    private int playerPos = 1;

    states state = states.Jump;
    enum states
    {
        Jump,
        Run
    }

    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
        startScale = transform.localScale.x;
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        state = states.Jump;
        hp = maxHp;
        isInvincible = false;
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(1, 1, 1, 1);
    }
  

    private void Update()
    {
        if(hp == 0)
        {
            gameObject.SetActive(false);
        }
        switch (state)
        {
            case states.Run:
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                if (!triggerCollider.IsTouchingLayers(ground))
                {
                    Flip();
                }
                break;

            case states.Jump:

                CheckPlayerPos();
                CheckGround();
                if (isGrounded && canJump)
                {
                    StartCoroutine(Jump());
                    canJump = false;
                }
                break;

            default:
                Debug.Log("NOTHING");
                break;
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
    }

    IEnumerator Jump()
    {
        rb.AddForce(new Vector2(jumpDistance*playerPos, jumpHeight), ForceMode2D.Impulse);
        yield return new WaitForSeconds(jumpDelay);
        canJump = true;
        yield return null;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
        isGrounded = colliders.Length > 1;
    }

    private void CheckPlayerPos()
    {
        float checkPlayerPos = player.transform.position.x - transform.position.x;
        if (checkPlayerPos > 0)
        {
            transform.localScale = new Vector2(startScale, transform.localScale.y);
            playerPos = 1;
        }
        if (checkPlayerPos < 0)
        {
            transform.localScale = new Vector2(startScale * -1, transform.localScale.y);
            playerPos = -1;
        }
    }

    public IEnumerator InvincibleTimer()
    {
        isInvincible = true;
        renderer.color = new Color(1, 1, 1, 0.2f);
        state = states.Run;
        yield return new WaitForSeconds(invincibilityTime);
        renderer.color = new Color(1, 1, 1, 1);
        isInvincible = false;
        state = states.Jump;
        yield return null;
    }
}
