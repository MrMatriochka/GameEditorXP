using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float damping = 1.5f; // movement speed
    public Vector2 offset = new Vector2(0f, 0f); // special effect if you want the character to be not in center of screen
    public bool faceLeft; //  mirror reflection of OFFSET along the y axis
    private Transform player;
    private int lastX;
    [HideInInspector] public float camLimit;
    void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        FindPlayer(faceLeft);
    }
    public void FindPlayer(bool playerFaceLeft)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (player)
        {
            int currentX = Mathf.RoundToInt(player.position.x);
            if (currentX > lastX) faceLeft = false; else if (currentX < lastX) faceLeft = true;
            lastX = Mathf.RoundToInt(player.position.x);

            Vector3 target;
            if (faceLeft)
            {
                target = new Vector3(player.position.x - offset.x, transform.position.y, transform.position.z);
            }
            else
            {
                target = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);
            }
            Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
            transform.position = currentPosition;
        }

        if(transform.position.x<=0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= camLimit)
        {
            transform.position = new Vector3(camLimit, transform.position.y, transform.position.z);
        }
    }
}
