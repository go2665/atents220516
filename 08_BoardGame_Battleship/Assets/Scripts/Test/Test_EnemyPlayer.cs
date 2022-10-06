using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnemyPlayer : TestBase
{
    public EnemyPlayer enemy;

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        enemy.UndoAllShipDeployment();
        enemy.AutoShipDeployment();
    }
}
