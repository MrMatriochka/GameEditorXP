using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLayer : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private float cameraPositionMultiplier = 1.0f; 

    // Start is called before the first frame update
    private void Start()
    {
        //transform.SetParent(null);
        if (cameraPositionMultiplier == 1.0f) return;
        mainCamera = Camera.main;
        var obj = new GameObject();
        obj.transform.parent = transform;
        var s = obj.AddComponent<SpriteRenderer>();
        var thisS = GetComponent<SpriteRenderer>();
        s.sprite = thisS.sprite;
        s.sortingOrder = thisS.sortingOrder;
        s.transform.localPosition = new Vector3(18, 0, 0);
        
        //Instantiate(obj, transform);
    }

    // Update is called once per frame
    private void Update()
    {
        if (cameraPositionMultiplier == 1.0f) return;
        transform.localPosition = new Vector3(0 - cameraPositionMultiplier * mainCamera.transform.position.x % 18, 0, 0);
    }
}
