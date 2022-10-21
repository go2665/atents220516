using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployData
{
    public ShipType shipType;           // 타입
    public ShipDirection direction;     // 방향
    public int size;                    // 크기
    public Vector2Int position;         // 위치(그리드좌표)
}
