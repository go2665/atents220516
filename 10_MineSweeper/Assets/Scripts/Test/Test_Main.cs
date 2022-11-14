using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Main : TestBase
{
    public TimeCounter timeCounter;

    Func<int> testFunc;

    private void Start()
    {
        testFunc += () => -1;
        testFunc += () => 3;
    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        //timeCounter.TimerStart();
        Debug.Log( testFunc() );

    }
    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        //timeCounter.TimerPause();
    }
    protected override void OnTest3(InputAction.CallbackContext obj)
    {
        //timeCounter.TimerReset();
    }
}
