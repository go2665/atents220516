using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Astar : MonoBehaviour
{    
    void Start()
    {
        //TestMap1();
        //TestMap2();

        GridMap gridMap = new GridMap(11, 6);
        Node node;
        node = gridMap.GetNode(2, 4);
        node.moveable = false;
        node = gridMap.GetNode(3, 4);
        node.moveable = false;
        node = gridMap.GetNode(3, 3);
        node.moveable = false;
        node = gridMap.GetNode(3, 2);
        node.moveable = false;

        node = gridMap.GetNode(5, 2);
        node.moveable = false;
        node = gridMap.GetNode(5, 1);
        node.moveable = false;
        node = gridMap.GetNode(6, 1);
        node.moveable = false;
        node = gridMap.GetNode(7, 1);
        node.moveable = false;

        node = gridMap.GetNode(7, 5);
        node.moveable = false;
        node = gridMap.GetNode(7, 4);
        node.moveable = false;

        List<Vector2Int> path = AStar.PathFind(gridMap, new(1, 1), new(9, 4));

        int i = 0;

    }

    private static void TestMap2()
    {
        GridMap gridMap = new GridMap(10, 6);
        Node node;
        node = gridMap.GetNode(4, 1);
        node.moveable = false;
        node = gridMap.GetNode(4, 2);
        node.moveable = false;
        node = gridMap.GetNode(4, 3);
        node.moveable = false;
        node = gridMap.GetNode(4, 4);
        node.moveable = false;

        List<Vector2Int> path = AStar.PathFind(gridMap, new(1, 2), new(7, 4));
    }

    private static void TestMap1()
    {
        GridMap gridMap = new GridMap(3, 3);
        Node node;
        node = gridMap.GetNode(0, 1);
        node.moveable = false;
        node = gridMap.GetNode(1, 1);
        node.moveable = false;

        List<Vector2Int> path = AStar.PathFind(gridMap, new(0, 0), new(0, 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
