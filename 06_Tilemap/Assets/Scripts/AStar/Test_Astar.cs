using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class Test_Astar : MonoBehaviour
{
    public Tilemap background;
    public Tilemap obstacle;
    Mouse mouse = Mouse.current;
    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        Debug.Log($"Background size : {background.size}");
        Debug.Log($"Obstacle size : {obstacle.size}");


        //TestMap1();
        //TestMap2();
        //TestMap3();
        //TestMap4_NoPath();

        int i = 0;
    }

    void Update()
    {
        if(mouse.leftButton.wasPressedThisFrame)
        {
            Vector2 screenPos = mouse.position.ReadValue();
            Vector3 worldPos = mainCam.ScreenToWorldPoint(screenPos);
            Vector3Int cellPos = background.WorldToCell(worldPos);
            Debug.Log($"Cell Pos : {cellPos}");
        }
    }


    private static void TestMap4_NoPath()
    {
        GridMap gridMap = new GridMap(3, 3);
        Node node;
        node = gridMap.GetNode(0, 1);
        node.moveable = false;
        node = gridMap.GetNode(1, 1);
        node.moveable = false;
        node = gridMap.GetNode(2, 1);
        node.moveable = false;

        List<Vector2Int> path = AStar.PathFind(gridMap, new(0, 0), new(0, 2));
    }

    private static void TestMap3()
    {
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

    
}
