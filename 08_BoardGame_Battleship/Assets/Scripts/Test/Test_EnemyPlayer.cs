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

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        List<int> list = new List<int>(5);
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Add(4);
        list.Add(5);

        int ret = list.Find((x) => x == 5);
        Debug.Log(ret);
        ret = list.Find((x) => x == 7);
        Debug.Log(ret);
        ret = list.Find((x) => x == 0);
        Debug.Log(ret);

    }
}
