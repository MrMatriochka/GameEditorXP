using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<GameObject> content;
    public float throwForce;
    public float throwSpeed;
    bool isClosed = true;

    private void OnEnable()
    {
        isClosed = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if(isClosed)
            {
                //Start animation
                StartCoroutine(OpenChest());
                isClosed = false;
            }
        }
    }

    IEnumerator OpenChest()
    {
        foreach (GameObject item in content)
        {
            yield return new WaitForSeconds(throwSpeed);
            GameObject current = Instantiate(item, transform.position, transform.rotation);
            current.GetComponent<Rigidbody2D>().AddForce((new Vector2(Random.Range(-1, 1), 1))*throwForce);
        }
        yield return null;
    }
}
