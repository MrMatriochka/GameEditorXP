using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPlayer : MonoBehaviour
{
    public LayerMask boss;
    public Collider2D triggerCollider;
    public Vector2 knockback;
    [HideInInspector] public bool touchBoss;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (triggerCollider.IsTouchingLayers(boss))
        {
            touchBoss = true;
        }
        else { touchBoss = false; }

    }

    public void IsHurted()
    {
        Bounce(knockback);
        triggerCollider.enabled = false;
    }
    public void Bounce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
