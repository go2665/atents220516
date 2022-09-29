using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Ship : MonoBehaviour
{
    public GameObject[] shipModels;

    ShipType type;
    ShipDirection direction;
    int size = 2;
    Transform model;

    public ShipType Type { get => type; }
    public ShipDirection Direction 
    { 
        get => direction; 
        set
        {
            direction = value;
            model.rotation = Quaternion.Euler(0, (int)direction * 90.0f, 0);
        }            
    }
    public int Size { get => size; }

    /// <summary>
    /// 배가 만들어질 때 실행될 함수
    /// </summary>
    /// <param name="newType"></param>
    public void Initialize(ShipType newType)
    {
        type = newType;
        switch (type)
        {
            case ShipType.Carrier:
                size = 5;
                break;
            case ShipType.Battleship:
                size = 4;
                break;
            case ShipType.Destroyer:
            case ShipType.Submarine:
                size = 3;
                break;
            case ShipType.PatrolBoat:
                size = 2;
                break;
            case ShipType.None:
            default:
                break;
        }

        GameObject obj = Instantiate(shipModels[(int)type - 1], transform);
        model = obj.transform;
        Direction = ShipDirection.NORTH;
    }

}
