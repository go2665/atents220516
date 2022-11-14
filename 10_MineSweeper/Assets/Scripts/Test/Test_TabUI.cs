using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TabUI : TestBase
{
    public TabPanel tab;
    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        tab.ResetAllTabs();
    }
}
