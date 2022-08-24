using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneMonsterManager : MonoBehaviour
{
    // 몬스터 위치 정보(같은 칸에 몬스터 중복 방지용)   
    //  - spawner는 몬스터를 생성할 때 SceneMonsterManager에게 생성된 몬스터를 알린다.
    //  - 몬스터는 죽을 때 spawner와 SceneMonsterManager에게 사망 사실을 알린다.

    GridMap gridMap;
    Tilemap background;
    Tilemap obstacle;

    List<Slime> monsterList;

    private void Start()
    {
        Transform gridTransform = transform.parent;
        background = gridTransform.Find("Background").GetComponent<Tilemap>();
        obstacle = gridTransform.Find("Obstacle").GetComponent<Tilemap>();

        gridMap = new(background, obstacle);
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
}
