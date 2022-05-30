using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_Small : MonoBehaviour
{
    public float speed = 3.0f;

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);   // 그냥 위로 올라가기
    }
}