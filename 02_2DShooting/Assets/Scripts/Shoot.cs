using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RequireComponent를 사용하면 이 스크립트를 가진 게임 오브젝트가 해당 컴포넌트가 없을 경우 자동으로 추가해준다.
[RequireComponent(typeof(Rigidbody2D))] 
public class Shoot : MonoBehaviour
{
    public float speed = 10.0f;
    Rigidbody2D rigid = null;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 백터 : 힘의 방향과 크기
        rigid.velocity = transform.right * speed;
    }
}
