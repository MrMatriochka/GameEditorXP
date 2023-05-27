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


    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;

    [HideInInspector]  public Vector3 lastCheckpoint;

    private Rigidbody2D rb;

    [HideInInspector] public int score;
    private Text scoreUI;

    Animator anim;

    public GameObject feet;
    public AudioClip jumpSFX;
    public AudioClip hitSFX;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        scoreUI = GameObject.Find("ScoreText").GetComponent<Text>();
        lastCheckpoint = transform.position;
        
        for (int i = 0; i < GameObject.Find("Health").transform.childCount; i++)
        {
            heart.Add(GameObject.Find("Health").transform.GetChild(i).gameObject);
        }
        hp = maxHp;
        UpdateLife();
        UpdateScore();
    }

    void Update()
    {
        
        if (Input.GetButton("Horizontal")&&canMove)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            Vector3 direction = transform.right * moveInput;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
        } 
            
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            
            GetComponent<AudioSource>().PlayOneShot(jumpSFX);
        }

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
        
        if(hp == 0 || transform.position.y <= -13)
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

        if (rb.velocity.x > 10)
            rb.velocity = new Vector2(10,rb.velocity.y);
        if (rb.velocity.y > 10)
            rb.velocity = new Vector2(rb.velocity.x,10);

        if (rb.velocity.x < -10)
            rb.velocity = new Vector2(-10, rb.velocity.y);
        if (rb.velocity.y < -10)
            rb.velocity = new Vector2(rb.velocity.x, -10);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        anim.gameObject.transform.localScale =new Vector3(-anim.gameObject.transform.localScale.x, anim.gameObject.transform.localScale.y, anim.gameObject.transform.localScale.z);
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
        GetComponent<AudioSource>().PlayOneShot(hitSFX);
        isInvincible = true;
        //GetComponent<Renderer>().color = new Color(1, 1, 1, 0.2f);
        canMove = false;
        feet.SetActive(false);
        yield return new WaitForSeconds(loseControlAfterHit);
        canMove = true;
        feet.SetActive(true);
        yield return new WaitForSeconds(invincibilityTime-loseControlAfterHit);
        //GetComponent<Renderer>().color = new Color(1, 1, 1, 1);
        isInvincible = false;
        yield return null;
    }

    public IEnumerator InvincibleTimer(float time)
    {
        GetComponent<AudioSource>().PlayOneShot(hitSFX);
        isInvincible = true;
        //GetComponent<Renderer>().color = new Color(1, 1, 1, 0.2f);
        canMove = false;
        feet.SetActive(false);
        yield return new WaitForSeconds(loseControlAfterHit);
        canMove = true;
        feet.SetActive(true);
        yield return new WaitForSeconds(time - loseControlAfterHit);
        //GetComponent<Renderer>().color = new Color(1, 1, 1, 1);
        isInvincible = false;
        yield return null;
    }

    void Death()
    {
        hp = maxHp;
        //GetComponent<Renderer>().color = new Color(1, 1, 1, 1);
        isInvincible = false;
        canMove = true;
        feet.SetActive(true);
        StopAllCoroutines();
        UpdateLife();
        
        GameOverMenu gameOver = GameObject.Find("GameManager").GetComponent<GameOverMenu>();
        gameOver.gameOverMenu.SetActive(true);
        gameOver.inGameUI.SetActive(false);
        gameOver.player = gameObject;

        gameObject.SetActive(false);

    }

    public void TpToLastcheckpoint()
    {
        transform.position = lastCheckpoint;
    }

    public void Bounce(Vector2 force)
    {
        rb.AddForce(force , ForceMode2D.Impulse );
        GetComponent<AudioSource>().PlayOneShot(jumpSFX);
    }
}
