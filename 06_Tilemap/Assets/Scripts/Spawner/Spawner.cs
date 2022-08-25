using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Spawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int maxSpawn = 1;        // 최대 몬스터 생성 수
    public float spawnDelay = 1.0f; // 몬스터 생성 간격

    public Vector2Int spawnAreaMin; // grid 기준
    public Vector2Int spawnAreaMax;

    int currentSpawn = 0;           // 현재 몬스터 생성 수
    float delayCount = 0.0f;

    SceneMonsterManager monsterManager;

    private void Start()
    {
        monsterManager = transform.GetComponentInParent<SceneMonsterManager>();
    }

    // 현재 이 스포너에서 생성되고 살아남은 몬스터의 수가 maxSpawn보다 작으면 spawnDelay초 후에 몬스터를 생성한다.
    private void Update()
    {
        if(currentSpawn < maxSpawn)         // 최대 스폰 수보다 생성되어 있는 슬라임의 수가 적으면
        {
            delayCount += Time.deltaTime;   // 카운트다운
            if( delayCount > spawnDelay )   // 원래 딜레이시간보다 커지면
            {
                currentSpawn++; // 이 스포너가 생성한 슬라임 수
                
                Vector2Int randomPos;
                do
                {
                    randomPos = new(Random.Range(spawnAreaMin.x, spawnAreaMax.x + 1),
                        Random.Range(spawnAreaMin.y, spawnAreaMax.y + 1));
                }
                while (!IsEmptyPostion(randomPos));
                
                GameObject obj = Instantiate(monsterPrefab, this.transform);    // 생성하고
                Slime slime = obj.GetComponent<Slime>();
                slime.onDead += MonsterDead;    // 죽었을 때 currentSpawn의 수를 줄이는 함수 실행

                slime.transform.position = monsterManager.GridToWorld(randomPos);   // 위치 변경

                delayCount = 0.0f; // 딜레이용 카운트 다운 초기화
            }
        }
    }

    // pos 그리드 위치에 다른 슬라임이 없으면 true, 있으면 false
    bool IsEmptyPostion(Vector2Int pos)
    {
        Slime[] slimes = GetComponentsInChildren<Slime>();

        return true;
    }

    private void MonsterDead()
    {
        currentSpawn--; // 몬스터가 죽으면 갯수 감소
    }

    private void OnDrawGizmos()
    {
        if(monsterManager !=null)
        {
            Vector3 min = monsterManager.GridToWorld(spawnAreaMin) - new Vector2(0.5f,0.5f);
            Vector3 max = monsterManager.GridToWorld(spawnAreaMax) + new Vector2(0.5f, 0.5f);
            Vector3 p2 = new(max.x, min.y);
            Vector3 p3 = new(min.x, max.y);
                        
            Handles.color = Color.red;
            Handles.DrawLine(min, p2, 5.0f);
            Handles.DrawLine(p2, max, 5.0f);
            Handles.DrawLine(max, p3, 5.0f);
            Handles.DrawLine(p3, min, 5.0f);
        }
    }
}