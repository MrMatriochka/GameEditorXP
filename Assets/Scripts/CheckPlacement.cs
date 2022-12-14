using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
    BuildingManager buildManager;
    public int nbLimit;
    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildingManager>();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Placement"))
        {
            buildManager.canPlace = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Placement"))
        {
            buildManager.canPlace = false;
        }
    }
}
