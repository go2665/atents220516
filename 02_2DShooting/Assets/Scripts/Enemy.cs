using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f;

    private void Update()
    {
        transform.Translate(-transform.right * speed * Time.deltaTime);
    }
}
