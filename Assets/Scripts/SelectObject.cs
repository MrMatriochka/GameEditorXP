using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    public GameObject selectedObj;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, 1000))
            {
                if(hit.collider.gameObject.CompareTag("Placement"))
                {
                    Select(hit.collider.gameObject);
                }
            }
        }
    }

    void Select(GameObject obj)
    {
        if (obj == selectedObj) return;
        Outline outline = obj.GetComponent<Outline>();
        if (outline == null) obj.AddComponent<Outline>();
        else outline.enabled = true;
        selectedObj = obj;
    }
}
