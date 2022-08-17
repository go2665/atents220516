using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    public static List<Vector2Int> PathFind(GridMap gridMap, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        if( gridMap.IsValidPosition(start) && gridMap.IsValidPosition(end) )
        {
            List<Node> open = new List<Node>();
            List<Node> close = new List<Node>();
            open.Add(gridMap.GetNode(start));
            
            Node current;

            while (true)
            {
                open.Sort();
                current = open[0];
                open.RemoveAt(0);

                if (current != end)
                {
                    // open에 들어갈 것들 추가
                    // 이웃 찾아서 갱신
                }
                else
                {
                    // 도착점이니 끝
                    break;
                }
            }
            // 리턴할 path 만들기
        }

        return path;
    }
}
