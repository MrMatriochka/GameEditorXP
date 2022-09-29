using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
    BuildingManager buildManager;
    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildingManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Placement"))
        {
            buildManager.canPlace = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Placement"))
        {
            buildManager.canPlace = true;
        }
    }
}
