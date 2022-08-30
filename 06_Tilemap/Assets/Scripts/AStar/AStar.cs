using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A* 알고리즘으로 길찾기를 수행할 수 있는 클래스
/// </summary>
public static class AStar
{
    /// <summary>
    /// 경로를 찾는 함수
    /// </summary>
    /// <param name="gridMap">경로를 찾을 그리드 맵</param>
    /// <param name="start">시작 위치</param>
    /// <param name="end">도착 위치</param>
    /// <returns>시작위치에서 도착위치로 가는 경로. 리스트가 비어있으면 경로를 찾지 못한 상황</returns>
    public static List<Vector2Int> PathFind(GridMap gridMap, Vector2Int start, Vector2Int end)
    {
        gridMap.ClearAStarData();   // 이전에 길찾기를 했을 때를 대비해 초기화
        List<Vector2Int> path = new List<Vector2Int>(); // 최종 경로가 저장될 리스트

        // 시작지점과 도착지점이 맵 안에 있는지 확인
        if( gridMap.IsValidPosition(start) && gridMap.IsValidPosition(end) )
        {
            // A* 알고리즘 용 변수들
            List<Node> open = new List<Node>();     // open리스트(경로를 계산할 후보 노드들)
            List<Node> close = new List<Node>();    // close리스트(경로 계산이 끝난 노드들)
            Node current = gridMap.GetNode(start);  // 지금 자기 주변을 재계산할 노드. 처음이라 start위치의 노드를 대입
            current.G = 0;                          // 시작 위치니까 G는 0
            current.H = Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y);    // 휴리스틱 값 계산.
            open.Add(current);                      // open 리스트에 current노드 추가

            while (open.Count > 0)  // open 리스트에 찾을 후보가 남아있으면 계속 반복
            {
                open.Sort();        // open 리스트 정렬하기(f값이 작은 순서대로 정렬됨)
                current = open[0];  // 가장 f값이 적은 노드를 current로 설정
                open.RemoveAt(0);   // open 리스트에서 제거

                //Debug.Log($"current : {current.x}, {current.y}");

                if (current != end) // current가 도착지점인지 확인
                {
                    // current가 도착지점이 아니면 이웃 갱신 작업 처리

                    close.Add(current); // current는 close 리스트에 추가

                    // 이웃 갱신을 갱신하기 위해 이웃 찾기(current의 주변 8방향 확인)
                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            Node node = gridMap.GetNode(x + current.x, y + current.y);  //current의 주변 노드 하나 가져오기
                            if (node == null)       // 그리드 맵 밖이면 스킵
                                continue;
                            if (node == current)    // 노드가 current면 스킵
                                continue;
                            if( node.gridType == Node.GridType.Wall)  // 벽이면 스킵 
                                continue;
                            if (close.Exists( iter => iter == node )) // close 리스트에 들어있으면 스킵
                                continue;
                            bool isDiagonal = (Mathf.Abs(x) == Mathf.Abs(y));   // 대각선 방향인지 확인. true면 대각선                            
                            if( isDiagonal &&       // 대각선 이동시 벽에 걸리면 스킵
                                (gridMap.GetNode(x + current.x, current.y).gridType == Node.GridType.Wall
                                || gridMap.GetNode(current.x, y + current.y).gridType == Node.GridType.Wall) )
                                continue;

                            float distance;
                            if(isDiagonal)  // 대각선 이동인지 여부에 따라 비용 설정
                            {
                                distance = 1.4f;    // 대각선으로 이동하는 경우 비용은 1.4
                            }
                            else
                            {
                                distance = 1.0f;    // 옆으로 이동하는 경우 비용은 1
                            }

                            if ( node.G > current.G + distance) // 원래 가지고 있던 G값이 더 크면 current 노드를 통해 이동하는 경로로 갱신
                            {
                                node.G = current.G + distance;  // G값 갱신
                                if (node.parent == null)        // open 리스트에 들어있지 않았을 경우(parent는 open리스트에 들어갈 때 설정함)
                                {
                                    node.H = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y); // 휴리스틱값 계산(x, y차이로 설정)
                                    open.Add(node);             // open리스트에 추가
                                }
                                node.parent = current;  // current를 부모로 설정
                            }
                        }
                    }
                }
                else
                {
                    // 도착점이니 루프 끝내기
                    break;
                }
            }

            if (current == end)     // 계산이 끝났을 때 도착지점에 도착했는지 확인
            {
                // 도착지점에 도착했으면 리턴할 path 만들기
                Node result = current;
                while (result != null)  // 도착지점에서 parent를 따라 시작지점까지 찾아가기
                {
                    path.Add(new Vector2Int(result.x, result.y));   // 중간 경로를 리스트에 저장
                    result = result.parent; // 다음 노드로
                }
                path.Reverse();     // 중간 경로를 다 넣었으면 뒤집기
            }
        }

        return path;    // 최종 경로 리턴
    }
}
