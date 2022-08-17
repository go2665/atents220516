using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Astar : MonoBehaviour
{    
    void Start()
    {
        GridMap gridMap = new GridMap(3, 3);
        Node node;
        node = gridMap.GetNode(0, 1);
        node.moveable = false;
        node = gridMap.GetNode(1, 1);
        node.moveable = false;

        //GridMap gridMap = new GridMap(2, 3);
        //Node node = gridMap.GetNode(0, 0);
        //node.moveable = false;

        Node a = new Node(1, 0);
        a.G = 15.0f;
        Node b = new Node(0, 0);
        b.G = 25.0f;
        Node c = new Node(0, 1);
        c.G = 5.0f;

        List<Node> list = new List<Node>();
        list.Add(a);
        list.Add(b);
        list.Add(c);

        list.Sort();

        int i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
