using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movingSpeed;
    public float jumpForce;
    public float invincibilityTime;
    public float loseControlAfterHit;
    public int maxHp;
    [HideInInspector] public List<GameObject> heart;
    [HideInInspector] public int hp;
    [HideInInspector] public bool isInvincible;

    private float moveInput;
    [HideInInspector] public bool canMove = true;


    private bool facingRight = false;

    private bool isGrounded;
    public Transform groundCheck;

    [HideInInspector]  public Transform lastCheckpoint;

    private Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer renderer;

    [HideInInspector] public int score;
    private Text scoreUI;

    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        scoreUI = GameObject.Find("ScoreText").GetComponent<Text>();

        for (int i = 0; i < GameObject.Find("Health").transform.childCount; i++)
        {
            heart.Add(GameObject.Find("Health").transform.GetChild(i).gameObject);
        }
        hp = maxHp;
    }

    void Update()
    {
        if (Input.GetButton("Horizontal")&&canMove)
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
        
        if(hp == 0)
        {
            Death();
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
        
        if(!isGrounded)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
        
        moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        renderer.flipX = !renderer.flipX;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
        isGrounded = colliders.Length > 2;
    }

    public void UpdateScore()
    {
        if(scoreUI == null)
        {
            scoreUI = GameObject.Find("ScoreText").GetComponent<Text>();
        }
        
        scoreUI.text = score.ToString();
    }
    
    public void UpdateLife()
    {
        foreach (GameObject obj in heart)
        {
            if(hp >= heart.IndexOf(obj)+1)
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    public IEnumerator InvincibleTimer()
    {
        isInvincible = true;
        renderer.color = new Color(1, 1, 1, 0.2f);
        canMove = false;
        yield return new WaitForSeconds(loseControlAfterHit);
        canMove = true;
        yield return new WaitForSeconds(invincibilityTime-loseControlAfterHit);
        renderer.color = new Color(1, 1, 1, 1);
        isInvincible = false;
        yield return null;
    }

    public IEnumerator InvincibleTimer(float time)
    {
        isInvincible = true;
        renderer.color = new Color(1, 1, 1, 0.2f);
        canMove = false;
        yield return new WaitForSeconds(loseControlAfterHit);
        canMove = true;
        yield return new WaitForSeconds(time - loseControlAfterHit);
        renderer.color = new Color(1, 1, 1, 1);
        isInvincible = false;
        yield return null;
    }

    void Death()
    {
        hp = maxHp;
        renderer.color = new Color(1, 1, 1, 1);
        isInvincible = false;
        UpdateLife();
        
        GameOverMenu gameOver = GameObject.Find("GameManager").GetComponent<GameOverMenu>();
        gameOver.gameOverMenu.SetActive(true);
        gameOver.inGameUI.SetActive(false);
        gameOver.player = gameObject;

        gameObject.SetActive(false);

    }

    public void TpToLastcheckpoint()
    {
        transform.position = lastCheckpoint.position;
    }

    public void Bounce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
