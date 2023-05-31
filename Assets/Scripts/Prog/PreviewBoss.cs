using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewBoss : MonoBehaviour
{
    public LayerMask ground;
    public Collider2D triggerCollider;
    [HideInInspector]public bool hole;
    [HideInInspector]public bool seePlayer;
    [HideInInspector]public float facing = 1;

    public GameObject fireball;
    public GameObject mouth;
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position+Vector3.up/2, Vector2.left*facing,2000);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                seePlayer = true;
            }
            else
            {
                seePlayer = false;
            }
        }
        else
        {
            seePlayer = false;
        }


        if (triggerCollider.IsTouchingLayers(ground))
        {
            hole = false;
        }
        else { hole = true; }
    }

    public void Attack()
    {
        GameObject obj = Instantiate(fireball, mouth.transform.position, Quaternion.identity);
        obj.GetComponent<PreviewFirebal>().speed *= facing;
    }
}
