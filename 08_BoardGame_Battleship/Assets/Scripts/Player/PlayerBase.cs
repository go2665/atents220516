using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected PlayerState state = PlayerState.Title;
    protected Ship[] ships;

    Action<Vector2>[] onClick;
    Action<Vector2>[] onMouseMove;

    private void Awake()
    {
        onClick = new Action<Vector2>[Enum.GetValues(typeof(PlayerState)).Length];
        onClick[(int)PlayerState.Title] = OnClick_Title;
        onClick[(int)PlayerState.ShipDeployment] = OnClick_ShipDeployment;
        onClick[(int)PlayerState.Battle] = OnClick_Battle;
        onClick[(int)PlayerState.GameEnd] = OnClick_GameEnd;

        onMouseMove = new Action<Vector2>[Enum.GetValues(typeof(PlayerState)).Length];
        onMouseMove[(int)PlayerState.Title] = OnMouseMove_Title;
        onMouseMove[(int)PlayerState.ShipDeployment] = OnMouseMove_ShipDeployment;
        onMouseMove[(int)PlayerState.Battle] = OnMouseMove_Battle;
        onMouseMove[(int)PlayerState.GameEnd] = OnMouseMove_GameEnd;
    }

    private void Start()
    {
        GameManager.Inst.Input.onClick += OnClick;
        GameManager.Inst.Input.onMouseMove += OnMouseMove;
    }

    private void OnClick(Vector2 screenPos)
    {
        onClick[(int)state]?.Invoke(screenPos);
    }

    protected virtual void OnClick_Title(Vector2 screenPos)
    {
    }

    protected virtual void OnClick_ShipDeployment(Vector2 screenPos)
    {
    }

    protected virtual void OnClick_Battle(Vector2 screenPos)
    {
    }

    protected virtual void OnClick_GameEnd(Vector2 screenPos)
    {
    }

    private void OnMouseMove(Vector2 screenPos)
    {
        onMouseMove[(int)state]?.Invoke(screenPos);
    }

    protected virtual void OnMouseMove_Title(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_ShipDeployment(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_Battle(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_GameEnd(Vector2 screenPos)
    {
    }

    // 테스트 용도
    public void Test_SetState(PlayerState state)
    {
        this.state = state;
    }
}
