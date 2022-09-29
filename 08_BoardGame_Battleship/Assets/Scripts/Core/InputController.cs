using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 입력 처리 전용 클래스
/// </summary>
public class InputController : MonoBehaviour
{
    /// <summary>
    /// 클릭 했을 때 실행될 델리게이트
    /// </summary>
    public Action<Vector2> onClick;

    public Action<Vector2> onMouseMove;


    /// <summary>
    /// 액션 맵
    /// </summary>
    PlayerInputActions inputActions;

    private void Awake()
    {
        // 액션 맵 생성
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Click.performed += OnClick;
        inputActions.Player.MouseMove.performed += OnMouseMove;

    }

    private void OnDisable()
    {
        inputActions.Player.MouseMove.performed -= OnMouseMove;
        inputActions.Player.Click.performed -= OnClick;
        inputActions.Player.Disable();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        // 클릭했을 때의 마우스 위치를 델리게이트로 전달
        onClick?.Invoke(Mouse.current.position.ReadValue());
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        onMouseMove?.Invoke(context.ReadValue<Vector2>());
    }
}
