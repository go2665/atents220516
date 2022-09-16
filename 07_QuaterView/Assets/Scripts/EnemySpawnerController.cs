using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawnerController : MonoBehaviour
{
    public float activateInterval = 5.0f;   // 스포너 활성화 시간 간격

    EnemySpawner[] enemySpawners;           // 스포너 배열
    Action[] onSpawnActivate;               // 스포너가 활성화 될 때 실행될 델리게이트 배열

    int spawnerIndex = 0;                   // 이번에 활성화 될 스포너의 인덱스

    private void Awake()
    {
        // 스포너 컨트롤러는 스포너만 자식으로 가진다.
        enemySpawners = new EnemySpawner[transform.childCount]; // 자식 수만큼 배열 크기 설정
        onSpawnActivate = new Action[transform.childCount];     // 스포너가 활성화 될 때 실행될 델리게이트가 같은 크기로 배열 설정
        for ( int i=0;i<transform.childCount;i++)
        {
            enemySpawners[i] = transform.GetChild(i).GetComponent<EnemySpawner>();  // EnemySpawner 찾기
            onSpawnActivate[i] += enemySpawners[i].WaitModeOff;                     // 활성화 시 실행될 함수 등록
        }

        // GetComponentsInChildren의 순서는 보장이 안되서 사용안함
        //enemySpawners = GetComponentsInChildren<EnemySpawner>();  
        //onSpawnActivate = new Action[enemySpawners.Length];
        //for(int i=0; i<enemySpawners.Length; i++)
        //{
        //    onSpawnActivate[i] += enemySpawners[i].WaitModeOff;
        //}
    }

    private void Start()
    {
        StartCoroutine(SpawnerActivator()); // 코루틴 실행
    }

    IEnumerator SpawnerActivator()
    {
        while (true)
        {
            ActivateSpawner();          // 스포너 활성화
            yield return new WaitForSeconds(activateInterval);  // activateInterval초 만큼 대기
        }
    }

    void ActivateSpawner()
    {
        onSpawnActivate[spawnerIndex]?.Invoke();                    // 델리게이트에 등록된 함수 실행
        spawnerIndex = (spawnerIndex + 1) % onSpawnActivate.Length; // 인덱스 증가(최대치가 되면 다시 0으로)
    }

}
