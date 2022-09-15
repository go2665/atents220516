using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Range(0,30)]
    public float spawnInterval = 3.0f;

    [Range(1, 10)]
    public int maxSpawnCount = 1;

    private float spawnTimer = 0.0f;
    private int currentSpawnCount = 0;

    bool waitMode = true;

    private void Start()
    {
        currentSpawnCount = 0;
        ResetSpawnTimer();
    }

    private void Update()
    {
        if (!waitMode)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer < 0 && currentSpawnCount < maxSpawnCount)
            {
                Spawn();
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
}
