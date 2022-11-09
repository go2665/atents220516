using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ResetButton : TestBase
{
    public ResetButton resetButton;

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        //resetButton.State = ResetButton.ButtonState.Normal;
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        //resetButton.State = ResetButton.ButtonState.Surprise;
    }

    protected override void OnTest3(InputAction.CallbackContext obj)
    {
        //resetButton.State = ResetButton.ButtonState.GameClear;
    }

    protected override void OnTest4(InputAction.CallbackContext obj)
    {
        //resetButton.State = ResetButton.ButtonState.GameOver;
    }
}
