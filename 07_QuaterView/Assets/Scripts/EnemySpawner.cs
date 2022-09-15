using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IHit
{
    public GameObject enemyPrefab;

    [Range(0,30)]
    public float spawnInterval = 3.0f;

    [Range(1, 10)]
    public int maxSpawnCount = 1;

    private float spawnTimer = 0.0f;
    private int currentSpawnCount = 0;

    bool waitMode = true;

    public float totalRepairTime = 5.0f;
    float repairElapsed = 0.0f;

    ParticleSystem fireEffect;
    ParticleSystem explosionEffect;

    float hp;
    float maxHP = 1000.0f;
    bool isDead = false;
    public float HP 
    { 
        get => hp;
        set 
        {
            hp = value;
            //Debug.Log(hp);
            if (hp < 0)
            {
                hp = 0;
                if (!isDead) // HP가 0보다 작아지면 Dead함수 실행
                    Dead();
            }
            hp = Mathf.Min(hp, maxHP);
        }
    }

    public float MaxHP { get => maxHP; }

    public Action onHealthChange { get; set; }

    public Action onDead { get; set; }


    void Awake()
    {
        explosionEffect = transform.GetChild(3).GetComponent<ParticleSystem>();
        fireEffect = transform.GetChild(4).GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        currentSpawnCount = 0;
        hp = maxHP;
        isDead = false;
        ResetSpawnTimer();
    }

    private void Update()
    {
        if (!waitMode && !isDead)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer < 0 && currentSpawnCount < maxSpawnCount)
            {
                Spawn();
            }
        }
        if(isDead)
        {
            repairElapsed += Time.deltaTime;
            if(repairElapsed > totalRepairTime)
            {
                Restore();
            }
        }
    }

    private void Spawn()
    {
        GameObject obj = Instantiate(enemyPrefab, 
            transform.position + UnityEngine.Random.insideUnitSphere * 0.1f, 
            transform.rotation);
        //IHit hitTarget = obj.GetComponent<IHit>();
        //hitTarget.onDead += ResetSpawnTimer;
        //hitTarget.onDead += () => currentSpawnCount--;
        currentSpawnCount++;
        ResetSpawnTimer();

        if( currentSpawnCount >= maxSpawnCount )
        {
            waitMode = true;
        }
    }

    private void ResetSpawnTimer()
    {
        spawnTimer = spawnInterval;
    }    

    public void WaitModeOff()
    {
        waitMode = false;
        currentSpawnCount = 0;
    }

    public void Dead()
    {
        //Debug.Log("사망");
        isDead = true;        
        explosionEffect.Simulate(0);    // 파티클시스템 내부의 시간 재조정
        explosionEffect.Play();
        fireEffect.Simulate(0);
        fireEffect.Play();
    }

    void Restore()
    {
        //Debug.Log("부활");
        explosionEffect.Stop();
        fireEffect.Stop();
        isDead = false;
        hp = maxHP;
        repairElapsed = 0.0f;
    }
}
