using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : MonoBehaviour, IHit
{
    ParticleSystem ps;
    Rigidbody rigid;
    Collider tankCollider;

    Vector3 hitPoint = Vector3.zero;

    float hp = 0.0f;
    float maxHP = 100.0f;
    bool isDead = false;

    public float HP 
    { 
        get => hp; 
        set
        {
            hp = value;
            if (hp < 0)
            {
                hp = 0;
                if(!isDead) // HP가 0보다 작아지면 Dead함수 실행
                    Dead();
            }
            hp = Mathf.Min(hp, maxHP);
        }
    }

    public float MaxHP { get => maxHP; }

    public Action onHealthChange { get; set; }
    public Action onDead { get; set; }

    public void Dead()
    {
        isDead = true;      // 사망 표시
        rigid.drag = 1.0f;  // 마찰력 감소
        rigid.angularDrag = 0.0f;   // 회전 마찰력 제거
        rigid.constraints = RigidbodyConstraints.None;  // 회전 묶어놓았던 것을 해지

        // 공격을 받은 위치의 바닥 아래에서 적 탱크 중심부로 향하는 백터
        Vector3 forceDirection = (transform.position - hitPoint).normalized;
        // forceDirection + 위쪽으로 가하는 힘
        rigid.AddForce(forceDirection + Vector3.up * 10.0f, ForceMode.VelocityChange);
        // forceDirection의 오른쪽 축을 기준으로 회전력 추가
        rigid.AddTorque(Quaternion.Euler(0, 90, 0) * forceDirection * 5.0f, ForceMode.VelocityChange);

        transform.GetChild(1).gameObject.SetActive(false);  // 흙먼지 트레일 2개 비활성화
        transform.GetChild(2).gameObject.SetActive(false);
        
        ps.Play();  // 탱크 폭팔 파티클 시스템 재생        
        //Debug.Log("사망");

    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        ps = transform.GetChild(3).GetComponent<ParticleSystem>();
        tankCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        hp = maxHP;
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if( collision.gameObject.CompareTag("Shell"))
        {
            //Debug.Log("피격");

            hitPoint = collision.contacts[0].point;
            hitPoint.y = -1.0f;
        }

        // 사망 이후 땅에 부딪쳤을 때
        if (isDead && collision.gameObject.CompareTag("Ground"))
        {
            //Debug.Log(collision.gameObject.name);
            DestroyProcess();
        }
    }

    private void DestroyProcess()
    {
        tankCollider.enabled = false;   // 컬라이더 비할성화(반드시 여기있어야 함)
        rigid.drag = 10.0f;             // 천천히 떨어지도록 마찰력
        rigid.angularDrag = 3.0f;       // 회전도 멈추도록
        Destroy(this.gameObject, 5.0f); // 시간이 지나면 사라지도록 처리
    }
}
