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

public enum ShipDirection : byte
{
    NORTH = 0,
    EAST,
    SOUTH,
    WEST
}

public enum GameState : byte
{
    Title = 0,      // 타이틀 화면 모드(필요없음)
    ShipDeployment, // 함선 배치 모드
    Battle,         // 전투 모드
    GameEnd         // 게임 종료 모드
}