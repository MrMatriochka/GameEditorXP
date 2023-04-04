using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadBody"))
        {
            transform.parent.GetComponent<PreviewCheckpoint>().checkpointCount--;
            gameObject.SetActive(false);
        }
    }
}
