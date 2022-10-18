using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_UserPlayer : TestBase
{
    UserPlayer userPlayer;
    EnemyPlayer enemyPlayer;


    private void Start()
    {
        userPlayer = FindObjectOfType<UserPlayer>();
        userPlayer.AutoShipDeployment();
        GameManager.Inst.Test_SetState(GameState.Battle);

        enemyPlayer = FindObjectOfType<EnemyPlayer>();

    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        userPlayer.UndoAllShipDeployment();
        userPlayer.AutoShipDeployment();
        enemyPlayer.UndoAllShipDeployment();
        enemyPlayer.AutoShipDeployment();
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        userPlayer.AutoAttack();
    }
}
