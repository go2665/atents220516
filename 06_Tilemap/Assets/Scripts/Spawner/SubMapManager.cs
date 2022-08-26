using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 서브맵에서 스포너와 타일맵 길찾기 관리하는 클래스
/// </summary>
public class SubMapManager : MonoBehaviour
{
    // 몬스터 위치 정보(같은 칸에 몬스터 중복 방지용)   
    //  - spawner는 몬스터를 생성할 때 SceneMonsterManager에게 생성된 몬스터를 알린다.
    //  - 몬스터는 죽을 때 spawner와 SceneMonsterManager에게 사망 사실을 알린다.

    GridMap gridMap;        // 그리드맵(A* 길찾기 + 스폰 위치 찾는 용도)
    Tilemap background;     // 배경용 타일맵(기본 배경)
    Tilemap obstacle;       // 장애물용 타일맵(이동 및 스폰 불가 지역)

    Spawner[] spawners;     // 몬스터 스포너

    public GridMap GridMap => gridMap;  // 그리드맵 읽기전용 프로퍼티

    private void Awake()
    {
        Transform gridTransform = transform.parent;
        background = gridTransform.Find("Background").GetComponent<Tilemap>();
        obstacle = gridTransform.Find("Obstacle").GetComponent<Tilemap>();

        gridMap = new(background, obstacle);    // 타일맵 찾아서 그리드맵 생성

        spawners = GetComponentsInChildren<Spawner>();  // 모든 스포너 찾아놓기
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
    public List<Vector2Int> SpawnablePostions(Vector2Int min, Vector2Int max)
    {
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
}
