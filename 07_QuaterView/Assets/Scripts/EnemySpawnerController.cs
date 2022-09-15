using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawnerController : MonoBehaviour
{
    public float activateInterval = 5.0f;

    EnemySpawner[] enemySpawners;
    Action[] onSpawnActivate;

    int spawnerIndex = 0;

    private void Awake()
    {
        enemySpawners = new EnemySpawner[transform.childCount];
        onSpawnActivate = new Action[transform.childCount];
        for ( int i=0;i<transform.childCount;i++)
        {
            enemySpawners[i] = transform.GetChild(i).GetComponent<EnemySpawner>();
            onSpawnActivate[i] += enemySpawners[i].WaitModeOff;
        }

        //enemySpawners = GetComponentsInChildren<EnemySpawner>();
        //onSpawnActivate = new Action[enemySpawners.Length];
        //for(int i=0; i<enemySpawners.Length; i++)
        //{
        //    onSpawnActivate[i] += enemySpawners[i].WaitModeOff;
        //}
    }

    private void Start()
    {
        StartCoroutine(SpawnerActivator());
    }

    IEnumerator SpawnerActivator()
    {
        while (true)
        {
            ActivateNextSpawner();
            yield return new WaitForSeconds(activateInterval);
        }
    }

    void ActivateNextSpawner()
    {
        onSpawnActivate[spawnerIndex]?.Invoke();
        spawnerIndex = (spawnerIndex + 1) % onSpawnActivate.Length;
    }

}
