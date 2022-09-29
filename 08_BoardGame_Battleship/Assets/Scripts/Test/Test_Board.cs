using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Board : MonoBehaviour
{
    public Board board;
    public GameObject shipPrefab;

    void Start()
    {
        //GameObject shipObj = new()
        //{
        //    name = "Test Ship"
        //};
        //Ship ship = shipObj.AddComponent<Ship>();
        //ship.Initialize(ShipType.Carrier);
        //ship.SetDirection(ShipDirection.EAST);

        GameObject shipObj = Instantiate(shipPrefab);
        Ship ship = shipObj.GetComponent<Ship>();
        ship.Initialize(ShipType.Carrier);

        bool result = board.ShipDeployment(ship, new Vector2Int(0, 0));
        Debug.Log(result);
        result = board.ShipDeployment(ship, new Vector2Int(4, 1));
        Debug.Log(result);
    }
}
