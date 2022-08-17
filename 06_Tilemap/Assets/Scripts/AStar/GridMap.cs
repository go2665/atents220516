using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    Node[,] nodes;
    int width;
    int height;

    public GridMap(int width, int height)
    {
        nodes = new Node[height, width];    // 항상 뒤에서부터 처리
        this.width = width;
        this.height = height;

        for (int y = 0; y < height ; y++)
        {
            for (int x = 0; x < width; x++)
            {
                nodes[(height - 1) - y, x] = new Node(x, y);
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        if (IsValidPosition(x, y))
        {
            return nodes[height - 1 - y, x];
        }
        return null;
    }

    public Node GetNode(Vector2Int pos)
    {
        return GetNode(pos.x, pos.y);
    }

    public bool IsValidPosition(int x, int y)
    {
        return x < width && x >= 0 && y < height && y >= 0;
    }

    public bool IsValidPosition(Vector2Int pos)
    {
        return IsValidPosition(pos.x, pos.y);
    }
}
