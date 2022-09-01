using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A*로 길찾기를 진행할 맵
/// </summary>
public class GridMap
{
    // 이 맵에 있는 전체 노드
    Node[,] nodes;
    
    // 맵의 가로 크기
    int width;
    // 맵의 세로 크기
    int height;

    // 맵의 시작 좌표 보정용
    Vector2Int offset = Vector2Int.zero;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="width">생성할 맵의 가로 크기</param>
    /// <param name="height">생성할 맵의 세로 크기</param>
    public GridMap(int width, int height)
    {
        // 맵 크기에 맞춰 배열 생성
        nodes = new Node[height, width];    // 항상 뒤에서부터 처리

        // 파라메터를 맴버 변수에 저장
        this.width = width;
        this.height = height;

        // 2중 for를 돌면서 모든 노드 생성
        for (int y = 0; y < height ; y++)
        {
            for (int x = 0; x < width; x++)
            {
                nodes[(height - 1) - y, x] = new Node(x, y);    // 타일맵의 좌표 방향과 일치시키기 위해
            }
        }
    }

    /// <summary>
    /// 타일맵을 이용해서 Gridmap을 생성하는 생성자
    /// </summary>
    /// <param name="background">Gridmap의 전체 크기 확인용</param>
    /// <param name="obstacle">못가는 지역 확인용</param>
    public GridMap(Tilemap background, Tilemap obstacle)
    {
        // 전체 크기 가져오기
        width = background.size.x;
        height = background.size.y;
        nodes = new Node[height, width];    // background 크기 기반으로 노드 배열 생성하기

        offset = (Vector2Int)background.cellBounds.min; // background의 cellBound 최소값(왼쪽아래끝)을 offset으로 지정

        // 전체 노드 생성
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // 생성하는 노드의 위치값을 offset값으로 보정
                nodes[(height - 1) - y, x] = new Node(x + offset.x, y + offset.y);
            }
        }

        // 갈 수 없는 지역 표시
        for (int y = background.cellBounds.yMin; y <= background.cellBounds.yMax; y++)
        {
            for (int x = background.cellBounds.xMin; x <= background.cellBounds.xMax; x++)
            {
                TileBase tile = obstacle.GetTile(new(x, y));    // 장애물용 타일맵의 해당 위치에 타일이 있는지 확인
                if (tile != null)   // 타일이 있으면 그 위치는 못가는 지역
                {
                    Node node = GetNode(x, y);          // 노드 가져와서
                    node.gridType = Node.GridType.Wall; // 못가는 지역이라고 표시
                }
            }
        }
    }

    /// <summary>
    /// 그리드 맵에서 노드 가져오는 함수
    /// </summary>
    /// <param name="x">타일맵 기준X</param>
    /// <param name="y">타일맵 기준Y</param>
    /// <returns>찾은 노드(없으면 null)</returns>
    public Node GetNode(int x, int y)
    {
        if (IsValidPosition(x, y))  // 적절한 위치에 있는지 확인
        {
            return nodes[height - 1 - y + offset.y, x - offset.x];    
        }
        return null;    // 맵 범위 안이 아니면 null
    }

    /// <summary>
    /// 그리드 맵에서 노드 가져오는 함수
    /// </summary>
    /// <param name="pos">타일맵 기준 위치</param>
    /// <returns>찾은 노드(없으면 null)</returns>
    public Node GetNode(Vector2Int pos)
    {
        return GetNode(pos.x, pos.y);
    }

    /// <summary>
    /// 입력으로 받은 좌표가 맵 안쪽인지 확인하는 함수
    /// </summary>
    /// <param name="x">확인할 위치의 X</param>
    /// <param name="y">확인할 위치의 Y</param>
    /// <returns>맵 안쪽이면 true. 아니면 false</returns>
    public bool IsValidPosition(int x, int y)
    {
        return (x < (width + offset.x)) && (x >= offset.x) && (y < (height + offset.y)) && (y >= offset.y);
    }

    /// <summary>
    /// 입력으로 받은 좌표가 맵 안쪽인지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>맵 안쪽이면 true. 아니면 false</returns>
    public bool IsValidPosition(Vector2Int pos)
    {
        return IsValidPosition(pos.x, pos.y);
    }

    /// <summary>
    /// 맵안의 노드들의 A* 데이터 초기화(G,H,parent)
    /// </summary>
    public void ClearAStarData()
    {
        foreach(var node in nodes)
        {
            node.ClearAStarData();
        }
    }

    /// <summary>
    /// 스폰 가능한 위치 찾기
    /// </summary>
    /// <param name="min">최소 그리드 좌표</param>
    /// <param name="max">최대 그리드 좌표</param>
    /// <returns>스폰 가능한 위치의 목록</returns>
    public List<Vector2Int> SpawnablePostions(Vector2Int min, Vector2Int max)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        for (int y = min.y; y < max.y + 1; y++)
        {
            for (int x = min.x; x < max.x + 1; x++)
            {
                Node node = GetNode(x, y);
                if( node.gridType == Node.GridType.Plain)  // 이동 가능한 노드면 결과 리스트에 추가
                {
                    result.Add(new(x, y));
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 그리드맵에서 랜덤한 이동 가능 위치 찾기
    /// </summary>
    /// <returns>랜덤 이동 가능한 위치</returns>
    public Vector2Int RandomMovablePostion()
    {
        Vector2Int randomPos = new Vector2Int();
        do
        {
            randomPos.x = Random.Range(0, width);
            randomPos.y = Random.Range(0, height);
        } while (nodes[randomPos.y, randomPos.x].gridType == Node.GridType.Wall); // 이동 가능한 위치가 나올 때까지 무한 반복(Plain이나 Monster)

        //randomPos = new Vector2Int(10,10);    // 테스트 용도
        return randomPos + offset;  // 랜덤으로 구한 결과를 offset과 더해서 리턴
    }

    /// <summary>
    /// 몬스터들의 위치를 그리드로 업데이트
    /// </summary>
    /// <param name="pre">이전에 몬스터들이 있던 위치</param>
    /// <param name="post">이동 후에 몬스터들이 있는 위치</param>
    public void UpdateMonsters(List<Vector2Int> pre, List<Vector2Int> post)
    {
        // 이전에 몬스터들이 있던 위치는 전부 원상 복구(될 수 있는게 평지밖에 없음)
        foreach(var pos in pre)
        {
            nodes[height - 1 - pos.y + offset.y, pos.x - offset.x].gridType = Node.GridType.Plain;
        }

        // 새롭게 몬스터들이 존재하고 있는 곳을 표시
        foreach (var pos in post)
        {
            nodes[height - 1 - pos.y + offset.y, pos.x - offset.x].gridType = Node.GridType.Monster;
        }
    }

    /// <summary>
    /// 특정 위치에 몬스터가 있는지 없는지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>몬스터 존재 여부. true면 몬스터가 있다.</returns>
    public bool IsMonsterThere(Vector2Int pos)
    {
        return (nodes[height - 1 - pos.y + offset.y, pos.x - offset.x].gridType == Node.GridType.Monster);
    }
}
