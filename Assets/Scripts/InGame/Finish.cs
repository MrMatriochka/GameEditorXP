using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelFinished();
        }
    }

    void LevelFinished()
    {
        GameObject.FindObjectOfType<BuildingManager>().Stop();
    }
}
