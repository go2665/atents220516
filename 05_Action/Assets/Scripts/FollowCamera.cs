using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target = null;
    public float speed = 3.0f;

    Vector3 offset = Vector3.zero;

    private void Start()
    {
        if(target == null)
        {
            target = FindObjectOfType<PlayerInputController>().transform;
        }
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
    }
}
