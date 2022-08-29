using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 몬스터 자동 생성용 클래스
/// </summary>
public class Spawner : MonoBehaviour
{
    public GameObject monsterPrefab;    // 생성할 몬스터의 프리팹
    public int maxSpawn = 1;            // 이 스포너에서 동시한 유지 가능한 최대 몬스터 수
    public float spawnDelay = 1.0f;     // 몬스터 생성 간격

    public Vector2 spawnArea;           // 스폰 영역의 크기(원점은 tranform의 position)

    int currentSpawn = 0;               // 현재 생성된 몬스터 수
    float delayCount = 0.0f;            // 생성용 시간 카운터(spawnDelay보다 커지면 몬스터 생성)

    public System.Action<Spawner> onRequestSpawn;   // 서브맵 메니저에게 스폰 요청을 보내는 델리게이트
    
    public Slime Spawn()
    {
        currentSpawn++;
        GameObject obj = Instantiate(monsterPrefab, transform);
        Slime slime = obj.GetComponent<Slime>();
        slime.onDead += MonsterDead;

        return slime;
    }

    // 몬스터 사망시 해야 할 일
    private void MonsterDead(Slime _)
    {
        currentSpawn--; // 몬스터가 죽으면 갯수 감소
    }

    private void RequestSpawn()
    {
        onRequestSpawn?.Invoke(this);
    }

    private void Update()
    {
        if (currentSpawn < maxSpawn)         // 최대 스폰 수보다 생성되어 있는 슬라임의 수가 적으면
        {
            delayCount += Time.deltaTime;   // 카운트다운
            if (delayCount > spawnDelay)    // 원래 딜레이시간보다 커지면
            {
                RequestSpawn();             // SubmapManager에게 생성 요청
                delayCount = 0.0f; // 딜레이용 카운트 다운 초기화
            }
        }
    }

    private void OnDrawGizmos()
    {
        // 스폰 위치 씬창에 그리기
        Vector3 pos = transform.position;
        pos.x = Mathf.Floor(pos.x);
        pos.y = Mathf.Floor(pos.y);

        Vector3 p0 = pos;
        Vector3 p1 = pos + new Vector3(spawnArea.x, 0);
        Vector3 p2 = pos + new Vector3(spawnArea.x, spawnArea.y);
        Vector3 p3 = pos + new Vector3(0, spawnArea.y);

        Handles.color = Color.red;
        Handles.DrawLine(p0, p1, 5.0f);
        Handles.DrawLine(p1, p2, 5.0f);
        Handles.DrawLine(p2, p3, 5.0f);
        Handles.DrawLine(p3, p0, 5.0f);
        
    }



    //public GameObject monsterPrefab;    // 생성할 몬스터의 프리팹
    //public int maxSpawn = 1;            // 이 스포너에서 동시한 유지 가능한 최대 몬스터 수
    //public float spawnDelay = 1.0f;     // 몬스터 생성 간격

    //public Vector2Int spawnAreaMin;     // 몬스터 스폰 범위(최소). grid 기준
    //public Vector2Int spawnAreaMax;     // 몬스터 스폰 범위(최대). grid 기준

    //int currentSpawn = 0;               // 현재 생성된 몬스터 수
    //float delayCount = 0.0f;            // 생성용 시간 카운터(spawnDelay보다 커지면 몬스터 생성)

    //SubMapManager subMapManager;        // 이 스포너가 붙어있는 서브맵의 매니저
    //List<Vector2Int> spawnPositions;    // 몬스터가 스폰 가능한 지역의 목록(타일맵 기반으로 만든 원본)
    //List<Slime> spawnMonsters;          // 현재 스폰된 몬스터 목록(몬스터 생성할 때 중복 생성 방지용)   

    //public GridMap GridMap => subMapManager.GridMap;    // 서브맵 매니저에서 그리드맵 가져오기

    ///// <summary>
    ///// 월드 좌표를 그리드 좌표로 변경
    ///// </summary>
    ///// <param name="postion"></param>
    ///// <returns></returns>
    //public Vector2Int WorldToGrid(Vector3 postion)
    //{
    //    return subMapManager.WorldToGrid(postion);
    //}

    ///// <summary>
    ///// 그리드 좌표를 월드 좌표로 변경
    ///// </summary>
    ///// <param name="gridPos"></param>
    ///// <returns></returns>
    //public Vector2 GridToWorld(Vector2Int gridPos)
    //{
    //    return subMapManager.GridToWorld(gridPos);
    //}

    //private void Start()
    //{
    //    subMapManager = transform.GetComponentInParent<SubMapManager>();    // 보모 오브젝트에 있는 SubMapManager 가져오기
    //    spawnPositions = subMapManager.SpawnablePostions(spawnAreaMin, spawnAreaMax);   // 스폰 가능한 지역 미리 계산해 놓기
    //    maxSpawn = Mathf.Min(maxSpawn, spawnPositions.Count);   // 스폰 최대 갯수 재계산(가능한 범위로)
    //    spawnMonsters = new List<Slime>();  // 스폰한 몬스터들을 가지고 있을 리스트
    //}

    //// 현재 이 스포너에서 생성되고 살아남은 몬스터의 수가 maxSpawn보다 작으면 spawnDelay초 후에 몬스터를 생성한다.
    //private void Update()
    //{
    //    if(currentSpawn < maxSpawn)         // 최대 스폰 수보다 생성되어 있는 슬라임의 수가 적으면
    //    {
    //        delayCount += Time.deltaTime;   // 카운트다운
    //        if( delayCount > spawnDelay )   // 원래 딜레이시간보다 커지면
    //        {
    //            currentSpawn++; // 이 스포너가 생성한 슬라임 수

    //            Vector2Int randomPos;
    //            List<Vector2Int> shuffleList = ShufflePositions();  // 스폰될 위치 결정
    //            if (shuffleList.Count > 0)
    //            {
    //                randomPos = shuffleList[0];

    //                GameObject obj = Instantiate(monsterPrefab, this.transform);        // 생성하고
    //                Slime slime = obj.GetComponent<Slime>();
    //                slime.onDead += MonsterDead;    // 죽었을 때 currentSpawn의 수를 줄이는 함수 실행
    //                slime.transform.position = subMapManager.GridToWorld(randomPos);   // 위치 변경
    //                slime.Initialize(this);         // 스포너 넘기기 용도
    //                spawnMonsters.Add(slime);       // 리스트에 슬라임 추가

    //                Vector2Int randomTarget = subMapManager.RandomMovablePotion();  // 랜덤한 위치 구하기
    //                slime.Move(randomTarget);    // 생성되면 같은 맵의 적당한 위치로 이동
    //            }

    //            delayCount = 0.0f; // 딜레이용 카운트 다운 초기화
    //        }
    //    }
    //}

    //// 이동 가능한 랜덤한 위치 구하기
    //public Vector2Int RandomMovablePotion()
    //{
    //    return subMapManager.RandomMovablePotion();
    //}

    //// 생성 가능한 위치들의 순서를 섞어서 리턴
    //List<Vector2Int> ShufflePositions()
    //{
    //    List<Vector2Int> temp = new List<Vector2Int>(spawnPositions); // spawnPositions안에 들어있는 값들을 가지는 리스트를 새로 만든다.

    //    // 몬스터가 있는 위치 제거
    //    foreach (var monster in spawnMonsters)
    //    {
    //        temp.Remove(monster.Position);
    //    }

    //    //// 리스트 크기 만큼 랜덤으로 인덱스 구해서 리무브앳(인덱스)해서 다른 리스트에 모으기
    //    //List<Vector2Int> result = new List<Vector2Int>();
    //    //while( temp.Count > 0 )
    //    //{
    //    //    int index = Random.Range(0, temp.Count - 1);
    //    //    result.Add(temp[index]);
    //    //    temp.RemoveAt(index);
    //    //}

    //    Vector2Int[] tempArray = temp.ToArray();

    //    // 피셔-예이츠 알고리즘(셔플)
    //    for (int i = 0; i < tempArray.Length - 1; i++)
    //    {
    //        int index = Random.Range(i + 1, tempArray.Length);
    //        (tempArray[i], tempArray[index]) = (tempArray[index], tempArray[i]);
    //    }

    //    // 최종 결과를 리스트로 만들어서 리턴
    //    List<Vector2Int> result = new List<Vector2Int>(tempArray);        
    //    return result;
    //}

    //// pos 그리드 위치에 다른 슬라임이 없으면 true, 있으면 false
    //bool IsEmptyPostion(Vector2Int pos)
    //{
    //    Slime[] slimes = GetComponentsInChildren<Slime>();
    //    foreach (Slime slime in slimes)
    //    {
    //        if (pos == slime.Position)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //// 몬스터 사망시 해야 할 일
    //private void MonsterDead(Slime slime)
    //{
    //    spawnMonsters.Remove(slime);
    //    currentSpawn--; // 몬스터가 죽으면 갯수 감소
    //}

    //private void OnDrawGizmos()
    //{
    //    // 스폰 위치 씬창에 그리기
    //    if(subMapManager !=null)
    //    {
    //        Vector3 min = subMapManager.GridToWorld(spawnAreaMin) - new Vector2(0.5f,0.5f);
    //        Vector3 max = subMapManager.GridToWorld(spawnAreaMax) + new Vector2(0.5f, 0.5f);
    //        Vector3 p2 = new(max.x, min.y);
    //        Vector3 p3 = new(min.x, max.y);

    //        Handles.color = Color.red;
    //        Handles.DrawLine(min, p2, 5.0f);
    //        Handles.DrawLine(p2, max, 5.0f);
    //        Handles.DrawLine(max, p3, 5.0f);
    //        Handles.DrawLine(p3, min, 5.0f);
    //    }
    //}
}
