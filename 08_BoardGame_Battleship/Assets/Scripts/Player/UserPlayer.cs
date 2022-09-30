using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserPlayer : PlayerBase
{
    /// <summary>
    /// 배치하기 위해 선택한 배
    /// </summary>
    Ship selectedShip = null;

    /// <summary>
    /// 상태별로 클릭했을 때 실행될 함수들
    /// </summary>
    Action<Vector2>[] onClick;

    /// <summary>
    /// 상태별로 마우스를 움직였을 때 실행될 함수들
    /// </summary>
    Action<Vector2>[] onMouseMove;

    /// <summary>
    /// 상태별로 마우스 휠을 돌렸을 때 실행될 함수들
    /// </summary>
    Action<float>[] onMouseWheel;

    private void Awake()
    {
        // 모든 델리게이트 등록
        int length = Enum.GetValues(typeof(PlayerState)).Length;
        onClick = new Action<Vector2>[length];
        onClick[(int)PlayerState.Title] = OnClick_Title;
        onClick[(int)PlayerState.ShipDeployment] = OnClick_ShipDeployment;
        onClick[(int)PlayerState.Battle] = OnClick_Battle;
        onClick[(int)PlayerState.GameEnd] = OnClick_GameEnd;

        onMouseMove = new Action<Vector2>[length];
        onMouseMove[(int)PlayerState.Title] = OnMouseMove_Title;
        onMouseMove[(int)PlayerState.ShipDeployment] = OnMouseMove_ShipDeployment;
        onMouseMove[(int)PlayerState.Battle] = OnMouseMove_Battle;
        onMouseMove[(int)PlayerState.GameEnd] = OnMouseMove_GameEnd;

        onMouseWheel = new Action<float>[length];
        onMouseWheel[(int)PlayerState.Title] = OnMouseWheel_Title;
        onMouseWheel[(int)PlayerState.ShipDeployment] = OnMouseWheel_ShipDeployment;
        onMouseWheel[(int)PlayerState.Battle] = OnMouseWheel_Battle;
        onMouseWheel[(int)PlayerState.GameEnd] = OnMouseWheel_GameEnd;
    }    

    protected override void Start()
    {
        base.Start();

        // 인풋 컨트롤러와 유저 플레이어 연결
        GameManager.Inst.Input.onClick += OnClick;
        GameManager.Inst.Input.onMouseMove += OnMouseMove;
        GameManager.Inst.Input.onMouseWheel += OnMouseWheel;
    }
        
    /// <summary>
    /// 배치할 배를 선택하는 함수
    /// </summary>
    /// <param name="type">선택할 배의 종류</param>
    public void SelectShipToDeploy(ShipType type)
    {
        selectedShip?.gameObject.SetActive(false);
        selectedShip = ships[(int)(type - 1)];
        OnMouseMove(Mouse.current.position.ReadValue());
        selectedShip.gameObject.SetActive(true);
    }

    /// <summary>
    /// 클릭했을 때 상태에따라 다른 함수를 호출하는 함수
    /// </summary>
    /// <param name="screenPos">클릭한 스크린 좌표</param>
    private void OnClick(Vector2 screenPos)
    {
        onClick[(int)state]?.Invoke(screenPos);
    }

    /// <summary>
    /// 마우스 움직였을 때 상태에따라 다른 함수를 호출하는 함수
    /// </summary>
    /// <param name="screenPos">클릭한 스크린 좌표</param>
    private void OnMouseMove(Vector2 screenPos)
    {
        onMouseMove[(int)state]?.Invoke(screenPos);
    }

    /// <summary>
    /// 마우스 휠을 돌렸을 때 상태에 따라 다른 함수를 호출하는 함수
    /// </summary>
    /// <param name="wheel"></param>
    private void OnMouseWheel(float wheel)
    {
        onMouseWheel[(int)state]?.Invoke(wheel);
    }

    // 상태별 입력 처리 함수들 ----------------------------------------------------------------------
    protected virtual void OnClick_Title(Vector2 screenPos)
    {
    }

    protected virtual void OnClick_ShipDeployment(Vector2 screenPos)
    {
        // 배치할 배가 있으면
        if (selectedShip != null)
        {
            // 클릭한 위치에 함선 배치 시도
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            bool result = board.ShipDeployment(selectedShip, worldPos);
            Debug.Log($"Ship deployment : {result}");
        }
    }

    protected virtual void OnClick_Battle(Vector2 screenPos)
    {
    }

    protected virtual void OnClick_GameEnd(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_Title(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_ShipDeployment(Vector2 screenPos)
    {
        // 배치할 배가 있으면
        if (selectedShip != null)
        {
            // 마우스 포인터가 존재하는 그리드에 따라 함선 이동
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            Vector2Int gridPos = board.WorldToGrid(worldPos);
            worldPos = board.GridToWorld(gridPos);

            selectedShip.transform.position = worldPos;
        }
    }

    protected virtual void OnMouseMove_Battle(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_GameEnd(Vector2 screenPos)
    {
    }

    private void OnMouseWheel_Title(float wheel)
    {
    }

    private void OnMouseWheel_ShipDeployment(float wheel)
    {
        // 배치할 배가 있으면
        if (selectedShip != null)
        {
            // 휠버튼 돌렸을 때 돌리는 방향에 따라 배치할 함선 회전
            bool ccw = false;
            if (wheel > 0)
                ccw = true;

            selectedShip.Rotate(ccw);
        }
    }

    private void OnMouseWheel_Battle(float wheel)
    {
    }

    private void OnMouseWheel_GameEnd(float wheel)
    {
    }
}
