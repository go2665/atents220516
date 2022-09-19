using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IHit
{
    public GameObject enemyPrefab;          // 스폰시킬 적 프리팹

    [Range(0,30)]
    public float spawnInterval = 3.0f;      // 적 스폰 시간 간격

    [Range(1, 10)]
    public int maxSpawnCount = 1;           // 한번 활성화 했을 때 생성하는 적의 수

    private float spawnTimer = 0.0f;        // spawnInterval 시간 간격 확인용
    private int currentSpawnCount = 0;      // 이번 활성화에서 생성한 적 수

    private bool waitMode = true;           // 대기모드 여부(true면 대기 상태. false가 되면 적을 스폰하기 시작)

    public float totalRepairTime = 5.0f;    // 스포너가 파괴되었을 때 정상으로 돌아오는데 걸리는 시간
    private float repairElapsed = 0.0f;     // 파괴 이후 경과 시간

    private ParticleSystem fireEffect;      // 파괴 중 표시할 불 이팩트
    private ParticleSystem explosionEffect; // 파괴 될 때 사용할 폭팔 이팩트

    private float hp;                       // hp
    private float maxHP = 1000.0f;          // 최대 hp
    private bool isDead = false;            // 사망여부 (true면 사망)
    public float HP 
    { 
        get => hp;
        set 
        {
            hp = value;
            if (hp < 0)
            {
                // HP가 0보다 작아지면 Dead함수 실행
                hp = 0;
                Dead();
            }
            hp = Mathf.Min(hp, maxHP);
            onHealthChange?.Invoke(hp / maxHP);
        }
    }

    public float MaxHP { get => maxHP; }

    public Action<float> onHealthChange { get; set; }  

    public Action onDead { get; set; }


    void Awake()
    {
        explosionEffect = transform.GetChild(3).GetComponent<ParticleSystem>(); // 자식으로 있는 파티클 찾기
        fireEffect = transform.GetChild(4).GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        // 시작할 때 값들 초기화
        currentSpawnCount = 0;  
        hp = maxHP;
        isDead = false;
        ResetSpawnTimer();  // 일정 시간 후에 스폰하도록 시간 초기화
    }

    private void Update()
    {
        if (!waitMode && !isDead)   // wait 모드가 아니고 살아있으면
        {
            spawnTimer -= Time.deltaTime;   // 적 생성을 위해 시간 감소
            if (spawnTimer < 0 && currentSpawnCount < maxSpawnCount)    
            {
                // 시간도 다 지나고 이번 활성화에서 생성할 수 있는 적 수가 남아있을 때 스폰
                Spawn();
            }
        }
        if(isDead)
        {
            repairElapsed += Time.deltaTime;        // 스포너가 죽었으면 수리시간 증가
            if(repairElapsed > totalRepairTime)
            {
                Restore();                          // 충분히 다 수리되면 복구
            }
        }
    }

    public void WaitModeOff()
    {
        // 스포너 컨트롤러의 델리게이트로 인해 실행되는 함수
        // 스포너 컨트롤러가 활성화하라고 신호를 보낼 때 실행됨.
        waitMode = false;       // 대기모드 해제하고
        currentSpawnCount = 0;  // 스폰 회수 초기화
    }

    private void Spawn()
    {
        // 적 스폰하는 함수
        GameObject obj = Instantiate(enemyPrefab, 
            transform.position + UnityEngine.Random.insideUnitSphere * 0.1f, 
            transform.rotation);

        // 하나의 스포너가 계속 적을 생성할 때 사용하던 코드
        //IHit hitTarget = obj.GetComponent<IHit>();        
        //hitTarget.onDead += ResetSpawnTimer;
        //hitTarget.onDead += () => currentSpawnCount--;

        currentSpawnCount++;    // 생성 갯수 증가
        ResetSpawnTimer();      // 스폰 시간 초기화

        if( currentSpawnCount >= maxSpawnCount )
        {
            waitMode = true;    // 최대치만큼 생성했으면 대기모드로 전환
        }
    }

    private void ResetSpawnTimer()
    {
        // 한마리 스폰 후 시간 초기화하는 함수
        spawnTimer = spawnInterval; // 스폰 시간 초기화
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
    }

    public void Dead()
    {
        // 죽었을 때 실행될 함수
        if (!isDead)
        {
            //Debug.Log("사망");
            isDead = true;
            explosionEffect.Simulate(0);    // 파티클 시스템 재사용을 위해 내부의 시간 재조정
            explosionEffect.Play();         // 파티클 시스템 재생
            fireEffect.Simulate(0);
            fireEffect.Play();

            onDead?.Invoke();       // 죽었다고 신호보내기
        }
    }

    void Restore()
    {
        // 스포너가 파괴되고 수리가 완료되었을 때 실행될 함수

        //Debug.Log("부활");
        explosionEffect.Stop(); // 이팩트 끄고
        fireEffect.Stop();
        isDead = false;         // 살았다고 표시
        hp = maxHP;             // HP 최대로 올리고
        repairElapsed = 0.0f;   // 수리 경과시간 초기화
    }
}
