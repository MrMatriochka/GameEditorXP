using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movingSpeed;
    public float jumpForce;
    public float invincibilityTime;
    public int maxHp;
    [HideInInspector] public List<GameObject> heart;
    [HideInInspector] public int hp;
    [HideInInspector] public bool isInvincible;

    private float moveInput;


    private bool facingRight = false;

    private bool isGrounded;
    public Transform groundCheck;

    [HideInInspector]  public Transform lastCheckpoint;

    private Rigidbody2D rb;
    private SpriteRenderer renderer;

    [HideInInspector] public int score;
    private Text scoreUI;

    void Start()
    {
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

        if(hp == 0)
        {
            Death();
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
        yield return new WaitForSeconds(invincibilityTime);
        renderer.color = new Color(1, 1, 1, 1);
        isInvincible = false;
        yield return null;
    }

    public IEnumerator InvincibleTimer(float time)
    {
        isInvincible = true;
        renderer.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(time);
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
