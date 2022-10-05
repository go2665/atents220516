using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyPlayer : PlayerBase
{
    protected override void Start()
    {
        base.Start();
        
        AutoShipDeployment();
    }

    // 배치 기능

    void AutoShipDeployment()
    {
        // board.IsShipDeployment(, );
        // ships[0]

    }
}
