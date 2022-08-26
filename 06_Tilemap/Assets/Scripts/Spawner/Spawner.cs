using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;
using UnityEngine.UIElements;
using static UnityEditor.ShaderData;

public class Spawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int maxSpawn = 1;        // 최대 몬스터 생성 수
    public float spawnDelay = 1.0f; // 몬스터 생성 간격

    public Vector2Int spawnAreaMin; // grid 기준
    public Vector2Int spawnAreaMax;

    int currentSpawn = 0;           // 현재 몬스터 생성 수
    float delayCount = 0.0f;

    SubMapManager subMapManager;
    List<Vector2Int> spawnPositions;    // 몬스터가 스폰 가능한 지역의 목록
    List<Slime> spawnMonsters;

    public GridMap GridMap => subMapManager.GridMap;

    public Vector2Int WorldToGrid(Vector3 postion)
    {
        return subMapManager.WorldToGrid(postion);
    }

    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        return subMapManager.GridToWorld(gridPos);
    }

    private void Start()
    {
        subMapManager = transform.GetComponentInParent<SubMapManager>();
        spawnPositions = subMapManager.SpawnablePostions(spawnAreaMin, spawnAreaMax);
        maxSpawn = Mathf.Min(maxSpawn, spawnPositions.Count);
        spawnMonsters = new List<Slime>();
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
                List<Vector2Int> shuffleList = ShufflePositions();
                if (shuffleList.Count > 0)
                {
                    randomPos = shuffleList[0];

                    GameObject obj = Instantiate(monsterPrefab, this.transform);        // 생성하고
                    Slime slime = obj.GetComponent<Slime>();
                    slime.onDead += MonsterDead;    // 죽었을 때 currentSpawn의 수를 줄이는 함수 실행
                    slime.transform.position = subMapManager.GridToWorld(randomPos);   // 위치 변경
                    slime.Initialize(this);
                    spawnMonsters.Add(slime);

                    Vector2Int randomTarget = subMapManager.RandomMovablePotion();
                    slime.Move(randomTarget);    // 생성되면 같은 맵의 적당한 위치로 이동
                }

                delayCount = 0.0f; // 딜레이용 카운트 다운 초기화
            }
        }
    }

    public Vector2Int RandomMovablePotion()
    {
        return subMapManager.RandomMovablePotion();
    }

    List<Vector2Int> ShufflePositions()
    {
        List<Vector2Int> temp = new List<Vector2Int>(spawnPositions); // spawnPositions안에 들어있는 값들을 가지는 리스트를 새로 만든다.

        foreach (var monster in spawnMonsters)
        {
            temp.Remove(monster.Position);
        }

        //// 리스트 크기 만큼 랜덤으로 인덱스 구해서 리무브앳(인덱스)해서 다른 리스트에 모으기
        //List<Vector2Int> result = new List<Vector2Int>();
        //while( temp.Count > 0 )
        //{
        //    int index = Random.Range(0, temp.Count - 1);
        //    result.Add(temp[index]);
        //    temp.RemoveAt(index);
        //}

        Vector2Int[] tempArray = temp.ToArray();

        // 피셔-예이츠 알고리즘
        for (int i = 0; i < tempArray.Length - 1; i++)
        {
            int index = Random.Range(i + 1, tempArray.Length);
            (tempArray[i], tempArray[index]) = (tempArray[index], tempArray[i]);
        }

        List<Vector2Int> result = new List<Vector2Int>(tempArray);
        
        return result;
    }

    // pos 그리드 위치에 다른 슬라임이 없으면 true, 있으면 false
    bool IsEmptyPostion(Vector2Int pos)
    {
        Slime[] slimes = GetComponentsInChildren<Slime>();
        foreach (Slime slime in slimes)
        {
            if (pos == slime.Position)
            {
                return false;
            }
        }
        return true;
    }

    private void MonsterDead(Slime slime)
    {
        spawnMonsters.Remove(slime);
        currentSpawn--; // 몬스터가 죽으면 갯수 감소

    }

    private void OnDrawGizmos()
    {
        if(subMapManager !=null)
        {
            Vector3 min = subMapManager.GridToWorld(spawnAreaMin) - new Vector2(0.5f,0.5f);
            Vector3 max = subMapManager.GridToWorld(spawnAreaMax) + new Vector2(0.5f, 0.5f);
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
