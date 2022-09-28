using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipType : byte
{
    None = 0,       // 배 아님
    Carrier,        // 사이즈5
    Battleship,     // 사이즈4
    Destroyer,      // 사이즈3
    Submarine,      // 사이즈3
    PatrolBoat      // 사이즈2
}