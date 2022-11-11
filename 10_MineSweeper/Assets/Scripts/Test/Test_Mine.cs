using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Mine : TestBase
{
    public Stage stage;

    private void Start()
    {        
    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        //stage.Test_SetMines();
        //int i = 0;
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        //stage.ResetAll();
    }
}
