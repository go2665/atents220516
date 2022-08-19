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

    // 맵의 시작 좌표
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
                    Node node = GetNode(x, y);  // 노드 가져와서
                    node.moveable = false;      // 못가는 지역이라고 표시
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
}
