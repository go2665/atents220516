using System;
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
            Node current = gridMap.GetNode(start);
            current.G = 0;
            current.H = Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y);
            open.Add(current);            

            while (true)
            {
                open.Sort();
                current = open[0];
                open.RemoveAt(0);

                Debug.Log($"current : {current.x}, {current.y}");

                if (current != end)
                {
                    close.Add(current);

                    // 이웃 갱신
                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            Node node = gridMap.GetNode(x + current.x, y + current.y);
                            if (node == null)       // 그리드 맵 밖이다.
                                continue;
                            if (node == current)    // 노드가 current다
                                continue;
                            if (!node.moveable)     // 벽이다.
                                continue;
                            if (close.Exists( iter => iter == node )) // 클로즈 리스트에 들어있다.
                                continue;

                            bool isDiagonal = (Mathf.Abs(x) == Mathf.Abs(y));   // true면 대각선
                            if ( isDiagonal && 
                                (!gridMap.GetNode(x + current.x, current.y).moveable 
                                || !gridMap.GetNode(current.x, y + current.y).moveable) )   // 대각선 이동할 때 걸린다.
                                continue;

                            float distance = 1.4f;
                            if(!isDiagonal)
                            {
                                distance = 1.0f;    // 대각선으로 이동한 것이 아닐 경우
                            }

                            if ( node.G > current.G + distance)
                            {
                                node.G = current.G + distance;
                                if (node.parent == null)
                                {
                                    node.H = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
                                    open.Add(node);
                                }
                                node.parent = current;
                            }
                        }
                    }
                }
                else
                {
                    // 도착점이니 끝
                    break;
                }
            }
            // 리턴할 path 만들기
            Node result = current;
            while( result != null )
            {
                path.Add(new Vector2Int(result.x, result.y));
                result = result.parent;
            }
            path.Reverse();
        }

        return path;
    }

    private static void NeigoborUpdate(GridMap grid, Node current)
    {
        // open에 들어갈 것들 추가
        // 이웃 찾아서 갱신

        
    }
}
