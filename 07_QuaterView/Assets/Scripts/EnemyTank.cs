using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyTank : MonoBehaviour, IHit
{
    public GameObject shellPrefab;
    public float fireAngle = 15.0f;
    public float attackRange = 20.0f;

    Transform firePosition;
    FireData fireData;

    ParticleSystem ps;
    Rigidbody rigid;
    Collider tankCollider;
    NavMeshAgent agent;

    Vector3 hitPoint = Vector3.zero;

    float hp = 0.0f;
    float maxHP = 100.0f;
    bool isDead = false;
    private PlayerTank player;

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

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        ps = transform.GetChild(3).GetComponent<ParticleSystem>();
        tankCollider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        
        Transform turret = transform.Find("TankRenderers").Find("TankTurret");    // 포탑 찾고
        firePosition = turret.GetChild(0);

        fireData = new FireData(shellPrefab.GetComponent<Shell>().data);
    }

    private void Start()
    {
        hp = maxHP;
        player = FindObjectOfType<PlayerTank>();
    }

    void Update()
    {
        if (!isDead)
        {
            fireData.CurrentCoolTime -= Time.deltaTime;

            Vector3 playerPos = player.transform.position;
            Vector3 dir = playerPos - transform.position;

            // ( dir.sqrMagnitude < attackRange * attackRange ) 
            // (dir.x * dir.x + dir.y * dir.y + dir.z * dir.z) < attackRange * attackRange 
            // * 4번, + 2번, < 1번 

            // ( Vector3.Angle( dir, transform.forward ) < fireAngle )
            // acos(dir.x * transform.forward.x + dir.y * transform.forward.y + dir.z * transform.forward.z) / root(dir.x * dir.x + dir.y * dir.y + dir.z * dir.z) * root(transform.forward.x * transform.forward.x + transform.forward.y * transform.forward.y + transform.forward.z * transform.forward.z)
            // * 9번, + 6번, / 1번, root 2번, acos 1번, < 1번

            if (fireData.IsFireReady
                && (dir.sqrMagnitude < attackRange * attackRange)
                && (Vector3.Angle(dir, transform.forward) < fireAngle) )
            {
                Instantiate(shellPrefab, firePosition.position, firePosition.rotation);
                fireData.ResetCoolTime();
            }

            agent.SetDestination(playerPos);
        }
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

    public void Dead()
    {
        agent.isStopped = true;
        agent.enabled = false;

        rigid.drag = 0.0f;  // 마찰력 감소
        rigid.angularDrag = 0.0f;   // 회전 마찰력 제거
        rigid.constraints = RigidbodyConstraints.None;  // 회전 묶어놓았던 것을 해지

        // 공격을 받은 위치의 바닥 아래에서 적 탱크 중심부로 향하는 백터
        Vector3 forceDirection = (transform.position - hitPoint).normalized;
        // forceDirection + 위쪽으로 가하는 힘
        rigid.AddForceAtPosition(forceDirection + Vector3.up * 10.0f, hitPoint, ForceMode.VelocityChange);
        
        // forceDirection의 오른쪽 축을 기준으로 회전력 추가
        rigid.AddTorque(Quaternion.Euler(0, 90, 0) * forceDirection * 5.0f, ForceMode.VelocityChange);

        transform.GetChild(1).gameObject.SetActive(false);  // 흙먼지 트레일 2개 비활성화
        transform.GetChild(2).gameObject.SetActive(false);

        ps.Play();  // 탱크 폭팔 파티클 시스템 재생        
        //Debug.Log("사망");

        isDead = true;      // 사망 표시
        onDead?.Invoke();   // 죽었음을 알림
    }

    void DestroyProcess()
    {
        tankCollider.enabled = false;   // 컬라이더 비할성화(반드시 여기있어야 함)
        rigid.drag = 10.0f;             // 천천히 떨어지도록 마찰력
        rigid.angularDrag = 3.0f;       // 회전도 멈추도록
        Destroy(this.gameObject, 5.0f); // 시간이 지나면 사라지도록 처리
    }
}
