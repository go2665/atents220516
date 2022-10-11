using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_UserPlayer : TestBase
{
    UserPlayer userPlayer;


    private void Start()
    {
        userPlayer = FindObjectOfType<UserPlayer>();
        userPlayer.AutoShipDeployment();
        userPlayer.Test_SetState(PlayerState.Battle);

    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        userPlayer.UndoAllShipDeployment();
        userPlayer.AutoShipDeployment();
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        userPlayer.AutoAttack();
    }
}
