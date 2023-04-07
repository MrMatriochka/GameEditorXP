using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 1;
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            GameObject.Find("Checkpoint").GetComponent<PreviewCheckpoint>().enemyCount--;
            Destroy(gameObject);
        }
        //if (collision.CompareTag("Enemy"))
        //{
        //    GameObject.Find("BlocStart").GetComponent<BlocCodeCheck>().ResetPreview();
        //    Destroy(gameObject);
        //}
    }
}
