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

    public Password password;
    public bool startTransi;
    void Start()
    {
        startPosition = transform.position;
        endPosition = endPos.transform.position;
        if (startTransi) StartCoroutine(Lerp());
    }
    //void FixedUpdate()
    //{
    //    elapsedTime += Time.deltaTime;
    //    float percentageComplete = elapsedTime / duration;
    //    transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percentageComplete));
    //}

    public IEnumerator Lerp()
    {
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = endPosition;
        if (startTransi) Destroy(gameObject);
        yield return null;
        password.Loading();
        yield return null;
    }
}
