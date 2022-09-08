using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    public float initialSpeed = 5.0f;       // 생성되면 즉시 적용될 속도
    public GameObject explosionPrefab;      // 폭팔 이팩트 프리팝
    public float damage;

    protected Rigidbody rigid;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        rigid.velocity = transform.forward * initialSpeed;  // 시작하면 앞쪽 방향으로 initialSpeed만큼의 속도로 나간다.
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // 충돌하면 폭팔 이팩트 생성
        Instantiate(explosionPrefab, 
            collision.contacts[0].point,                            // 생성 위치는 충돌지점
            Quaternion.LookRotation(collision.contacts[0].normal)); // 생성될 때의 회전은 충돌지점의 노멀백터를 forward로 지정하는 회전

        // 맞은 대상이 HP가 깎일 수 있는 대상이면 HP를 감소시킨다.
        IHit hitTarget = collision.gameObject.GetComponent<IHit>();
        if(hitTarget != null)
        {
            hitTarget.HP -= damage;
        }

        Destroy(this.gameObject);   // 포탄 삭제
    }
}
