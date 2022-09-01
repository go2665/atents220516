using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 그리드 한칸을 나타낼 클래스
/// </summary>
public class Node : System.IComparable<Node>
{
    // 위치값(기준은 왼쪽 아래가 원점. 오른쪽 x+ , 위쪽 y+)
    public int x;
    public int y;

    // 노드의 종류
    public enum GridType
    {
        Plain = 0,  // 바닥
        Wall,       // 벽
        Monster     // 몬스터
    }
    public GridType gridType = GridType.Plain;  // 기본값은 평지

    // A* 알고리즘을 위한 G,H,F
    public float G;             // 시작점에서 이 노드까지 오는데 걸린 거리(부모를 따라 움직였을 때의 거리)
    public float H;             // 이 노드에서 도착점까지 가는데 걸릴 것으로 예상하는 거리(x축 이동거리 + y축 이동거리)
    public float F => G + H;    // G와 H의 합.

    public Node parent;         // 어느 노드에서 이 노드로 왔는지에 대한 정보.

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="x">위치</param>
    /// <param name="y">위치</param>
    /// <param name="gridType">그리드의 종류. 디폴트로 평지설정.</param>
    public Node(int x, int y, GridType gridType = GridType.Plain)
    {
        this.x = x;
        this.y = y;
        this.gridType = gridType;   // 노드 값 초기화
        ClearAStarData();           // g, h, parent 초기화
    }

    /// <summary>
    /// A*용 데이터 초기화
    /// </summary>
    public void ClearAStarData()
    {
        G = float.MaxValue;     // 초기값으로 가장 큰 수
        H = float.MaxValue;
        parent = null;          // 부모는 일단 없음
    }

    // 정렬을 위해 필수인 함수. IComparable을 상속받아서 만들어야 함.
    // 리턴이 0보다 작다. => 이 인스턴스가 other보다 앞에 있다.
    // 리턴이 0이다 => 이 인스턴스가 other와 크기가 같다.
    // 리턴이 0보다 크다. => 이 인스턴스가 other보다 뒤에 있다.
    public int CompareTo(Node other)
    {
        if (other == null)
            return 1;

        return F.CompareTo(other.F);    // 기준을 F로 설정
    }

    // obj와 이 인스턴스가 같은 오브젝트인지 확인하는 함수
    public override bool Equals(object obj)
    {
        return obj is Node node &&
               x == node.x &&
               y == node.y;
    }

    // 해시코드 생성해주는 함수
    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
        
    // == 명령어 오버로딩( ==과 !=은 쌍으로 구현해야 한다.)
    public static bool operator ==(Node op1, Vector2Int op2)
    {
        return (op1.x == op2.x) && (op1.y == op2.y);
    }

    // != 명령어 오버로딩
    public static bool operator !=(Node op1, Vector2Int op2)
    {
        return (op1.x != op2.x) || (op1.y != op2.y);
    }

    //operator의 종류 : + - * / > < >= <= == != && ||
}
