using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyPlayer : PlayerBase
{
    public float thinkingTimeMin = 2.0f;
    public float thinkingTimeMax = 5.0f;

    protected override void Start()
    {
        base.Start();
        opponent = GameManager.Inst.UserPlayer;

        AutoShipDeployment();
    }

    public override void OnPlayerTurnStart(int turnNumber)
    {
        base.OnPlayerTurnStart(turnNumber);   // 턴 시작 설정

        // 적은 턴이 시작되면 몇초후에 자동으로 공격
        StartCoroutine(TurnStartDelay());
    }

    public override void OnPlayerTurnEnd()
    {
    }

    IEnumerator TurnStartDelay()
    {
        yield return new WaitForSeconds(Random.Range(thinkingTimeMin, thinkingTimeMax));
        AutoAttack();
    }

}
