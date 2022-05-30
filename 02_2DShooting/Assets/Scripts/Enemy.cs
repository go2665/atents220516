using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f;

    private void Update()
    {
        transform.Translate(-transform.right * speed * Time.deltaTime); // 계속 자신의 왼쪽 방향으로 날아간다.
    }
}
