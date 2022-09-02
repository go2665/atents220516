using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 서브맵에서 스포너와 타일맵 길찾기, 몬스터 관리하는 클래스
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
        // 타일맵 가져오기
        Transform gridTransform = transform.parent;
        background = gridTransform.Find("Background").GetComponent<Tilemap>();
        obstacle = gridTransform.Find("Obstacle").GetComponent<Tilemap>();

        // 타일맵 찾아서 그리드맵 생성
        gridMap = new(background, obstacle);    

        monsterList = new List<Slime>();        // 전체 몬스터 이동 업데이트 용도
        spawnRequests = new Queue<Spawner>();   // 몬스터 생성 요청 기록용 큐
        spawners = GetComponentsInChildren<Spawner>();  // 모든 스포너 찾아놓기

        foreach(var spawner in spawners)
        {
            // 스포너가 생성을 요청할 때 실행되는 onRequestSpawn 델리게이트에 함수 등록하기(큐에 추가 용도)
            spawner.onRequestSpawn += () => spawnRequests.Enqueue(spawner);
        }
    }

    /// <summary>
    /// 몬스터 위치 업데이트와 몬스터 생성 처리
    /// </summary>
    private void Update()
    {
        List<Vector2Int> enemyPosList = new List<Vector2Int>();     // 적의 현재 위치
        List<Vector2Int> enemyOldPosList = new List<Vector2Int>();  // 적의 옛날 위치
        foreach(var monster in monsterList)
        {
            enemyOldPosList.Add(WorldToGrid(monster.transform.position));   // 몬스터의 현재 위치를 그리드로 바꿔 enemyOldPosList에 추가
            monster.MoveUpdate();   // 실제 이동
            enemyPosList.Add(WorldToGrid(monster.transform.position));  // 몬스터가 이동한 위치를 기준으로 enemyPosList에 기록
        }

        gridMap.UpdateMonsters(enemyOldPosList, enemyPosList);  // 기록한 위치를 기반으로 그리드에 몬스터 위치 업데이트

        //spawnRequests에 있는 것들 생성
        while(spawnRequests.Count > 0)
        {
            Spawner spawner = spawnRequests.Dequeue();  // 하나씩 꺼내서
            List<Vector2Int> posList = SpawnablePostions(spawner);  // 생성 가능한 위치 다 계산하기
            if (posList.Count > 0)  // 생성 가능한 위치가 하나라도 있으면
            {
                posList = ShuffleList(posList);     // 리스트 섞기
                Slime monster = spawner.Spawn();    // 몬스터 실제 생성
                monster.Initialize(this);           // 몬스터 초기화
                monster.transform.position = GridToWorld(posList[0]);   // 몬스터의 위치를 변경
                monster.onDead += MonsterDead;
                monsterList.Add(monster);           // 몬스터 목록에 추가
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

    /// <summary>
    /// 스폰 가능한 지역 목록 만들기(Gridmap 랩핑함수)
    /// </summary>
    /// <param name="spawner">몬스터를 생성할 스포너</param>
    /// <returns>스폰 가능한 그리드 좌표의 목록</returns>
    private List<Vector2Int> SpawnablePostions(Spawner spawner)
    {
        // ist<Vector2Int> SpawnablePostions(Vector2Int min, Vector2Int max)를 사용하기 위해 최소값 최대값 구하기
        Vector2Int min = WorldToGrid(spawner.transform.position);
        Vector2Int max = WorldToGrid(spawner.transform.position + ((Vector3)spawner.spawnArea - (Vector3)Vector2.one));
        // -1을 한 이유는 월드 좌표를 그리드 좌표로 변경할 때 가로 세로가 1개씩 더 포함이 되기 때문에

        return gridMap.SpawnablePostions(min, max);
    }

    /// <summary>
    /// 이동 가능한 랜덤 위치찾기(Gridmap 랩핑함수)
    /// </summary>
    /// <returns>이동 가능한 랜덤 위치</returns>
    public Vector2Int RandomMovablePotion()
    {
        return gridMap.RandomMovablePostion();
    }

    /// <summary>
    /// 해당 지역에 몬스터가 있는지 확인
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>몬스터가 있으면 true</returns>
    public bool IsMonsterThere(Vector2Int pos)
    {
        return gridMap.IsMonsterThere(pos);
    }

    /// <summary>
    /// 리스트를 섞는 함수
    /// </summary>
    /// <param name="list">섞을 리스트</param>
    /// <returns>다 섞은 리스트</returns>
    List<Vector2Int> ShuffleList(List<Vector2Int> list)
    {
        Vector2Int[] tempArray = list.ToArray();    // 배열이 랜덤인덱스로 접근할 때는 훨씬 더 빠르기 때문에

        // 피셔-예이츠 알고리즘(셔플)
        // 제일 왼쪽의 노드를 교환 대상 노드로 지정한다.
        // 교환 대상 노드의 오른쪽에 있는 나머지 노드들 중 임의로 하나를 골라 교환 대상 노드의 위치를 교환한다.
        // 교환 대상 노드의 오른쪽 옆에 있는 노드를 새로운 교환 대상 노드로 지정한다.
        // 교환 대상 노드가 제일 오른쪽 노드가 될 때까지 반복한다.
        for (int i = 0; i < tempArray.Length - 1; i++)
        {
            int index = Random.Range(i + 1, tempArray.Length);
            (tempArray[i], tempArray[index]) = (tempArray[index], tempArray[i]);
        }

        // 최종 결과를 리스트로 만들어서 리턴
        List<Vector2Int> result = new List<Vector2Int>(tempArray);
        return result;
    }

    /// <summary>
    /// 몬스터가 죽을 때 처리
    /// </summary>
    /// <param name="monster">죽은 몬스터</param>
    private void MonsterDead(Slime monster)
    {
        Vector2Int gridPos = WorldToGrid(monster.transform.position);
        Node node = gridMap.GetNode(gridPos);   
        node.gridType = Node.GridType.Plain;    // 그리드 맵에서 몬스터가 있던 위치를 빈곳으로 만들기
        monsterList.Remove(monster);            // 몬스터 목록에서도 제거
    }

    /// <summary>
    /// 몬스터 죽이는 테스트 함수
    /// </summary>
    public void Test_KillMonster()
    {
        if (monsterList.Count > 0)
        {
            int index = Random.Range(0, monsterList.Count);
            monsterList[index].Die();
        }
    }
}
