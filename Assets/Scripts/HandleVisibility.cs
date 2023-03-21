using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleVisibility : MonoBehaviour
{
    public float displayDistance = 1f;

    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseDistance = Vector2.Distance(mousePosition, gameObject.transform.position);

        if(mouseDistance<displayDistance)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
