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

    public override void OnPlayerTurnStart()
    {
        base.OnPlayerTurnStart();   // 턴 시작 설정
    }

    public override void OnPlayerTurnEnd()
    {
    }

}
