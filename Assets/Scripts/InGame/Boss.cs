using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Boss : MonoBehaviour
{
    public float moveSpeed = 1f;
    public LayerMask ground;
    public LayerMask ennemi;

    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D triggerColliderGround;
    public Collider2D triggerColliderWall;

    public GameObject fireball;
    public GameObject mouth;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (PlayerPrefs.HasKey("ProgLvl"))
        {
            if (PlayerPrefs.GetInt("ProgLvl") == SceneManager.GetActiveScene().buildIndex +2)
            {
                StartCoroutine(Avancer());
            }
            if (PlayerPrefs.GetInt("ProgLvl") == SceneManager.GetActiveScene().buildIndex + 3)
            {
                anim.SetTrigger("Attack");
                StartCoroutine(Avancer());
                
            }
            if (PlayerPrefs.GetInt("ProgLvl") == SceneManager.GetActiveScene().buildIndex + 4)
            {
                StartCoroutine(AttackBoucle());
            }
            if (PlayerPrefs.GetInt("ProgLvl") == SceneManager.GetActiveScene().buildIndex + 5)
            {
                StartCoroutine(AttackBoucle());
            }
        }
    }


    void FixedUpdate()
    {
        if (PlayerPrefs.HasKey("ProgLvl"))
        {
            if (PlayerPrefs.GetInt("ProgLvl") == SceneManager.GetActiveScene().buildIndex + 4)
            {
                anim.SetBool("IsWalking", true);
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            if (PlayerPrefs.GetInt("ProgLvl") == SceneManager.GetActiveScene().buildIndex + 5)
            {
                anim.SetBool("IsWalking", true);
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                if (!triggerColliderGround.IsTouchingLayers(ground) || triggerColliderWall.IsTouchingLayers(ennemi))
                {
                    Flip();
                }
            }
            if (PlayerPrefs.GetInt("ProgLvl") == SceneManager.GetActiveScene().buildIndex + 6)
            {
                anim.SetBool("IsWalking", true);
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                if (!triggerColliderGround.IsTouchingLayers(ground) || triggerColliderWall.IsTouchingLayers(ennemi))
                {
                    Flip();
                }

                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * moveSpeed, 10);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        anim.SetTrigger("Attack"); 
                    }
                }
            }
        }
        
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
    }

    public void Attack()
    {
        GameObject obj = Instantiate(fireball, mouth.transform.position, Quaternion.identity);
        obj.GetComponent<Fireball>().speed *= -moveSpeed;
    }
    
    IEnumerator AttackBoucle()
    {
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("Attack"); 
        StartCoroutine(AttackBoucle());
        yield return null;
    }
    IEnumerator Avancer()
    {
        float elapsedTime = 0f;
        Vector3 currentPos = transform.position;
        Vector3 goToPos = transform.position + (Vector3.right * moveSpeed*5f);
        anim.SetBool("IsWalking", true);
        while (elapsedTime < 2)
        {
            transform.position = Vector3.Lerp(currentPos, goToPos, (elapsedTime / 2));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.position = goToPos;
        anim.SetBool("IsWalking", false);
        yield return null;
    }
}
