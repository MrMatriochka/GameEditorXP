using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 1;
    // Update is called once per frame

    private void Start()
    {
        if (speed < 0)
            transform.localScale *= -1;
    }
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().hp--;
            collision.GetComponent<Player>().UpdateLife();
        }
    }
}
