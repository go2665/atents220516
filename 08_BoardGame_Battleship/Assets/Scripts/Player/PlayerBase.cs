using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public Board board;

    protected PlayerState state = PlayerState.Title;
    protected Ship[] ships;

    protected virtual void Start()
    {
        int shipTypeCount = ShipManager.Inst.ShipTypeCount;
        ships = new Ship[shipTypeCount];
        for ( int i=0;i< shipTypeCount; i++)
        {
            ships[i] = ShipManager.Inst.MakeShip((ShipType)(i + 1), this);
        }
    }

    // 테스트 용도
    public void Test_SetState(PlayerState state)
    {
        this.state = state;
    }
}
