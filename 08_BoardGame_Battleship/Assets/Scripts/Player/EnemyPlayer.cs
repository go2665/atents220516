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
        opponent = GameManager.Inst.UserPlayer;

        AutoShipDeployment();
    }

    public override void OnTurnStart()
    {
    }

    public override void OnTurnEnd()
    {
    }

}
