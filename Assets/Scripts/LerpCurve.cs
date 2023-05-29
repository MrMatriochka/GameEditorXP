using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpCurve : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private GameObject endPos;
    [SerializeField] private float duration;
    private float elapsedTime;
    [SerializeField] private AnimationCurve curve;
    void Start()
    {
        startPosition = transform.position;
        endPosition = endPos.transform.position;
    }
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / duration;
        transform.position = Vector3.Lerp(startPosition,endPosition, curve.Evaluate(percentageComplete));
    }
}
