using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : System.IComparable<Node>
{
    // 위치값
    public int x;
    public int y;

    public bool moveable;   // 이동할 수 있는 지역인지 여부(true면 이동 가능)

    public float G;
    public float H;
    public float F => G + H;

    public Node parent;     // 어느 노드에서 이 노드로 왔는지에 대한 정보

    public Node(int x, int y, bool moveable = true)
    {
        this.x = x;
        this.y = y;
        this.moveable = moveable;
        G = float.MaxValue;
        H = float.MaxValue;
        parent = null;
    }

    // 정렬을 위해 필수
    // 리턴이 0보다 작다. => 이 인스턴스가 other보다 앞에 있다.
    // 리턴이 0이다 => 이 인스턴스가 other와 크기가 같다.
    // 리턴이 0보다 크다. => 이 인스턴스가 other보다 뒤에 있다.
    public int CompareTo(Node other)
    {
        if (other == null)
            return 1;

        return F.CompareTo(other.F);
    }

    public override bool Equals(object obj)
    {
        return obj is Node node &&
               x == node.x &&
               y == node.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public static bool operator ==(Node op1, Vector2Int op2)
    {
        return (op1.x == op2.x) && (op1.y == op2.y);
    }

    public static bool operator !=(Node op1, Vector2Int op2)
    {
        return (op1.x != op2.x) || (op1.y != op2.y);
    }

    //operator의 종류 : + - * / > < >= <= == != && ||
}
