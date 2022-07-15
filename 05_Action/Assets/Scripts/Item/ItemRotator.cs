using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    public float spinSpeed = 360.0f;
    public float moveDistance = 1.0f;

    float timeElapsed = 0;
    Vector3 startPosition;
    float moveHalf;

    private void Start()
    {
        startPosition = transform.position;
        moveHalf = moveDistance * 0.5f;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        transform.position = startPosition + moveHalf * new Vector3(0, (1-Mathf.Cos(timeElapsed)), 0);
        transform.Rotate(spinSpeed * Time.deltaTime * Vector3.up);
    }
}
