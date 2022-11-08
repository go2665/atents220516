using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Main : TestBase
{
    public TimeCounter timeCounter;

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        //timeCounter.TimerStart();
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
