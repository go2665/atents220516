using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestBase : MonoBehaviour
{
    PlayerInputActions inputActions;

    protected virtual void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Click.performed += OnTestClick;
        inputActions.Test.Test1.performed += OnTest1;
        inputActions.Test.Test2.performed += OnTest2;
        inputActions.Test.Test3.performed += OnTest3;
        inputActions.Test.Test4.performed += OnTest4;
        inputActions.Test.Test5.performed += OnTest5;
        inputActions.Test.Test0.performed += OnTest0;
    }

    private void OnDisable()
    {
        inputActions.Test.Disable();
        inputActions.Test.Test0.performed -= OnTest0;
        inputActions.Test.Test5.performed -= OnTest5;
        inputActions.Test.Test4.performed -= OnTest4;
        inputActions.Test.Test3.performed -= OnTest3;
        inputActions.Test.Test2.performed -= OnTest2;
        inputActions.Test.Test1.performed -= OnTest1;
        inputActions.Test.Click.performed -= OnTestClick;
    }

    protected virtual void OnTest1(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest2(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest3(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest4(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest5(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest0(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTestClick(InputAction.CallbackContext context)
    {
    }
}
