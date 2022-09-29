using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : Singleton<ShipManager>
{
    public GameObject shipPrefab;

    int shipTypeCount;
    public int ShipTypeCount { get => shipTypeCount; }

    protected override void Awake()
    {
        base.Awake();
        shipTypeCount = Enum.GetValues(typeof(ShipType)).Length - 1;
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
}
