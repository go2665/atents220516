using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TurnManager : TestBase
{
    public UserPlayer userPlayer;
    public EnemyPlayer enemyPlayer;

    private void Start()
    {
        GameManager.Inst.Test_SetState(GameState.Battle);
    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        userPlayer.AutoAttack();
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        enemyPlayer.AutoAttack();
    }
}
