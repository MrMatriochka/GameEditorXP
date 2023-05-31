using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashVisualsHandler : MonoBehaviour
{
    public GameObject[] buttonsToDeactivate;
    public BuildingManager bm;

    private Image img;

    private bool holding;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        img.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bm.pendingObj != null && !holding)
        {
            OnObjectHeld();
            holding = true;
        }
        else if(bm.pendingObj == null && holding)
        {
            OnObjectLetGo();
            holding = false;
        }
    }

    void OnObjectLetGo()
    {
        img.enabled = false;
        foreach (GameObject o in buttonsToDeactivate)
        {
            o.SetActive(true);
        }
    }

    void OnObjectHeld()
    {
        img.enabled = true;
        foreach (GameObject o in buttonsToDeactivate)
        {
            o.SetActive(false);
        }
    }
}
