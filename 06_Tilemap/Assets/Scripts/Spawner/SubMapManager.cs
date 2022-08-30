using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 서브맵에서 스포너와 타일맵 길찾기 관리하는 클래스
/// </summary>
public class SubMapManager : MonoBehaviour
{
    GridMap gridMap;        // 그리드맵(A* 길찾기 + 스폰 위치 찾는 용도)
    Tilemap background;     // 배경용 타일맵(기본 배경)
    Tilemap obstacle;       // 장애물용 타일맵(이동 및 스폰 불가 지역)

    Spawner[] spawners;     // 서브맵의 전체 몬스터 스포너
    List<Slime> monsterList;// 이 씬이 가지는 모든 몬스터 스포너에서 생성된 모든 적
    Queue<Spawner> spawnRequests;   // 몬스터 스폰을 요청한 스포너 큐(노드 하나당 스폰 1회)

    public GridMap GridMap => gridMap;  // 그리드맵 읽기전용 프로퍼티

    private void Awake()
    {
        Transform gridTransform = transform.parent;
        background = gridTransform.Find("Background").GetComponent<Tilemap>();
        obstacle = gridTransform.Find("Obstacle").GetComponent<Tilemap>();

        gridMap = new(background, obstacle);    // 타일맵 찾아서 그리드맵 생성

        monsterList = new List<Slime>();
        spawnRequests = new Queue<Spawner>();
        spawners = GetComponentsInChildren<Spawner>();  // 모든 스포너 찾아놓기

        foreach(var spawner in spawners)
        {
            spawner.onRequestSpawn += () => spawnRequests.Enqueue(spawner);
        }
    }

    private void Update()
    {
        List<Vector2Int> enemyPosList = new List<Vector2Int>();
        List<Vector2Int> enemyOldPosList = new List<Vector2Int>();
        foreach(var slime in monsterList)
        {
            enemyOldPosList.Add(WorldToGrid(slime.transform.position));
            slime.MoveUpdate();
            enemyPosList.Add(WorldToGrid(slime.transform.position));
        }

        gridMap.UpdateMonsters(enemyOldPosList, enemyPosList);

        //spawnRequests에 있는 것들 생성
        while(spawnRequests.Count > 0)
        {
            Spawner spawner = spawnRequests.Dequeue();
            List<Vector2Int> posList = SpawnablePostions(spawner);
            if (posList.Count > 0)
            {
                posList = ShuffleList(posList);
                Slime monster = spawner.Spawn();
                monsterList.Add(monster);
                monster.transform.position = GridToWorld(posList[0]);
            }
        }

    }







    /// <summary>
    /// 월드좌표를 그리드맵의 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="postion">변환할 월드좌표</param>
    /// <returns>변환된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 postion)
    {
        return (Vector2Int)background.WorldToCell(postion);
    }

    /// <summary>
    /// 그리드맵의 그리드 좌표를 월드좌표로 변경해주는 함수
    /// </summary>
    /// <param name="gridPos">그리드맵에서의 위치</param>
    /// <returns>변환된 월드좌표</returns>
    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        return background.CellToWorld((Vector3Int)gridPos) + new Vector3(0.5f, 0.5f);   // 0.5는 그리드의 가운데 위치
    }

    /// <summary>
    /// 스폰 가능한 지역 목록 만들기(Gridmap 랩핑함수)
    /// </summary>
    /// <param name="min">사각형의 최소위치</param>
    /// <param name="max">사각형의 최대위치</param>
    /// <returns>스폰 가능한 그리드 좌표의 목록</returns>
    private List<Vector2Int> SpawnablePostions(Vector2Int min, Vector2Int max)
    {
        return gridMap.SpawnablePostions(min, max);
    }

    private List<Vector2Int> SpawnablePostions(Spawner spawner)
    {
        Vector2Int min = WorldToGrid(transform.position);
        Vector2Int max = WorldToGrid(transform.position + (Vector3)spawner.spawnArea);

        return gridMap.SpawnablePostions(min, max);
    }

    /// <summary>
    /// 이동 가능한 랜덤 위치찾기(Gridmap 랩핑함수)
    /// </summary>
    /// <returns>이동 가능한 랜덤 위치</returns>
    private Vector2Int RandomMovablePotion()
    {
        return gridMap.RandomMovablePostion();
    }

    List<Vector2Int> ShuffleList(List<Vector2Int> list)
    {
        Vector2Int[] tempArray = list.ToArray();

        // 피셔-예이츠 알고리즘(셔플)
        for (int i = 0; i < tempArray.Length - 1; i++)
        {
            int index = Random.Range(i + 1, tempArray.Length);
            (tempArray[i], tempArray[index]) = (tempArray[index], tempArray[i]);
        }

        // 최종 결과를 리스트로 만들어서 리턴
        List<Vector2Int> result = new List<Vector2Int>(tempArray);
        return result;
    }
}
