using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Mine : TestBase
{
    public Cell cell;

    private void Start()
    {        
    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        cell.SetMine();
    }
}
