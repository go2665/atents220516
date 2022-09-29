using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerState state = PlayerState.Title;
    Ship[] ships;

    Action<Vector2>[] onClick;

    private void Awake()
    {
        onClick = new Action<Vector2>[Enum.GetValues(typeof(PlayerState)).Length];
        onClick[(int)PlayerState.Title] = OnClick_Title;
        onClick[(int)PlayerState.ShipDeployment] = OnClick_ShipDeployment;
        onClick[(int)PlayerState.Battle] = OnClick_Battle;
        onClick[(int)PlayerState.GameEnd] = OnClick_GameEnd;
    }

    private void Start()
    {
        GameManager.Inst.Input.onClick += OnClick;
        GameManager.Inst.Input.onMouseMove += OnMouseMove;
    }

    private void OnMouseMove(Vector2 screenPos)
    {
        
    }

    private void OnClick(Vector2 screenPos)
    {
        onClick[(int)state]?.Invoke(screenPos);
    }

    private void OnClick_Title(Vector2 screenPos)
    {
    }

    private void OnClick_ShipDeployment(Vector2 screenPos)
    {
    }

    private void OnClick_Battle(Vector2 screenPos)
    {
    }

    private void OnClick_GameEnd(Vector2 screenPos)
    {
    }

    // 테스트 용도
    public void Test_SetState(PlayerState state)
    {
        this.state = state;
    }
}
