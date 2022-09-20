using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tank : MonoBehaviour, IHit
{
    public GameObject[] shellPrefabs;   // 포탄용 프리팹
    public float maxHP = 100.0f;        // 최대 HP

    protected bool isDead = false;      // 사망 여부

    protected Transform turret;         // 포탑의 트랜스폼
    protected Transform firePosition;   // 발사 위치
    protected FireData[] fireDatas;     // 발사 쿨타임 관리
    protected Vector3 hitPoint = Vector3.zero;    // 충돌한 위치

    private float hp;                   // 현재 HP
    private Image hpBar;

    GameObject minimapIndicator;

    // 각종 컴포넌트들
    protected Rigidbody rigid;
    protected Collider tankCollider;
    protected ParticleSystem tankExplosionEffect;    

    // 프로퍼티들
    public float HP
    {
        get => hp;
        set
        {
            if (value != hp)
            {
                hp = value;
                if (hp <= 0)    // HP가 0보다 작거나 같아지면 사망처리
                {
                    hp = 0;
                    Dead();
                }
                hp = Mathf.Min(hp, maxHP);  // HP 상승할때를 대비한 코드
                onHealthChange?.Invoke(hp / maxHP);
            }
        }
    }
    public float MaxHP { get => maxHP; }

    // 델리게이트들
    public Action<float> onHealthChange { get; set; }  // HP 변경이 있을 때 실행될 델리게이트
    public Action onDead { get; set; }          // 죽을 때 실행될 델리게이트

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();                              // 리지드바디 가져오기
        tankExplosionEffect = transform.GetChild(3).GetComponent<ParticleSystem>();      // 탱크 폭팔 파티클 시스템
        minimapIndicator = transform.GetChild(5).gameObject;
        tankCollider = GetComponent<Collider>();                        // 탱크의 컬라이드

        turret = transform.Find("TankRenderers").Find("TankTurret");    // 포탑 찾고
        firePosition = turret.GetChild(0);                              // 발사 위치 찾기

        fireDatas = new FireData[shellPrefabs.Length];                  // fireData 배열만들기
        for (int i = 0; i < shellPrefabs.Length; i++)
        {
            Shell shell = shellPrefabs[i].GetComponent<Shell>();        // 포탄에서 데이터 가져와서
            fireDatas[i] = new FireData(shell.data);                    // fireData만들기
        }

        hpBar = GetComponentInChildren<Image>();
        onHealthChange += (ratio) =>
        {
            hpBar.fillAmount = ratio;
            hpBar.color = Color.Lerp(Color.red, Color.green, ratio);
        };

    }

    protected virtual void Start()
    {
        HP = maxHP;     // 시작할 때 최대HP로 시작        
    }

    protected virtual void Update()
    {
        if (!isDead)    // 살아있으면
        {
            foreach (var data in fireDatas)
            {
                data.CurrentCoolTime -= Time.deltaTime; // 포탄 쿨타임 처리
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shell"))   // 포탄에 맞았을때 피격지점 수정하기
        {
            hitPoint = collision.contacts[0].point;
            hitPoint.y = -1.0f;
        }

        // 사망 이후 땅에 부딪쳤을 때
        if (isDead && collision.gameObject.CompareTag("Ground"))
        {
            DestroyProcess();
        }
    }

    public virtual void TakeDamage(float damage)
    {
        HP -= damage;
    }

    public virtual void Dead()
    {
        if (!isDead)            // 살아 있을때 한번만 실행
        {
            isDead = true;      // 사망 표시

            minimapIndicator.SetActive(false);

            rigid.drag = 0.0f;          // 마찰력 감소
            rigid.angularDrag = 0.0f;   // 회전 마찰력 제거
            rigid.constraints = RigidbodyConstraints.None;  // 회전 묶어놓았던 것을 해지

            // 공격을 받은 위치의 바닥 아래에서 탱크 중심부로 향하는 백터
            Vector3 forceDirection = (transform.position - hitPoint).normalized;
            // forceDirection에 위쪽으로 가하는 힘 추가
            rigid.AddForceAtPosition(forceDirection + Vector3.up * 10.0f, hitPoint, ForceMode.VelocityChange);

            // forceDirection의 오른쪽 축을 기준으로 회전력 추가
            rigid.AddTorque(Quaternion.Euler(0, 90, 0) * forceDirection * 5.0f, ForceMode.VelocityChange);

            transform.GetChild(1).gameObject.SetActive(false);  // 흙먼지 트레일 2개 비활성화
            transform.GetChild(2).gameObject.SetActive(false);

            tankExplosionEffect.Play(); // 탱크 폭팔 파티클 시스템 재생        

            onDead?.Invoke();           // 죽었음을 알림
        }
    }

    void DestroyProcess()
    {
        tankCollider.enabled = false;   // 컬라이더 비할성화(반드시 여기있어야 함)
        rigid.drag = 10.0f;             // 천천히 떨어지도록 마찰력
        rigid.angularDrag = 3.0f;       // 회전도 멈추도록
        Destroy(this.gameObject, 5.0f); // 시간이 지나면 사라지도록 처리
    }
}
