using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    public float initialSpeed = 5.0f;       // 생성되면 즉시 적용될 속도
    public GameObject explosionPrefab;      // 폭팔 이팩트 프리팝

    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = transform.forward * initialSpeed;  // 시작하면 앞쪽 방향으로 initialSpeed만큼의 속도로 나간다.
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌하면 폭팔 이팩트 생성
        Instantiate(explosionPrefab, 
            collision.contacts[0].point,                            // 생성 위치는 충돌지점
            Quaternion.LookRotation(collision.contacts[0].normal)); // 생성될 때의 회전은 충돌지점의 노멀백터를 forward로 지정하는 회전

        Destroy(this.gameObject);   // 포탄 삭제
    }
}
