using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : Singleton<ShipManager>
{
    /// <summary>
    /// 배의 프리팹
    /// </summary>
    public GameObject shipPrefab;

    /// <summary>
    /// 배의 모델용 프리팹
    /// </summary>
    public GameObject[] shipModels;

    /// <summary>
    /// 배의 종류
    /// </summary>
    int shipTypeCount;

    /// <summary>
    /// 배가 바라보는 방향 갯수
    /// </summary>
    int shipDirectionCount = 0;

    
    public int ShipTypeCount { get => shipTypeCount; }

    public int ShipDirectionCount { get => shipDirectionCount; }

    protected override void Awake()
    {
        base.Awake();
        shipTypeCount = Enum.GetValues(typeof(ShipType)).Length - 1;
        shipDirectionCount = Enum.GetValues(typeof(ShipDirection)).Length;
    }

    public Ship MakeShip(ShipType type, PlayerBase player)
    {
        GameObject shipObj = Instantiate(shipPrefab, player.transform);
        Ship ship = shipObj.GetComponent<Ship>();
        ship.Initialize(type);
        shipObj.name = $"{type}_{ship.Size}";
        shipObj.SetActive(false);

        return ship;
    }

    public GameObject GetShipModel(ShipType type)
    {
        return shipModels[(int)type - 1];
    }
}
