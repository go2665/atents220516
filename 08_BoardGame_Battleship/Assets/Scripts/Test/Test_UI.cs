using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_UI : TestBase
{
    public BattleLogger battleLogger;

    int counter = 0;

    private void Start()
    {
        counter = 0;
    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        battleLogger.Log($"{counter++} : Hello~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        battleLogger.Clear();
    }
}
