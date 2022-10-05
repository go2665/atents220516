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
    Ship SelectedShip
    {
        get => selectedShip;
        set
        {
            if( selectedShip != value ) // 값이 변경될 때만 적용
            {
                if (selectedShip != null)
                {
                    // 이전에 선택된 배가 있으면 머티리얼을 원상 복귀
                    selectedShip.Renderer.material = ShipManager.Inst.NormalShipMaterial;
                }
                selectedShip = value;
                if (selectedShip != null)
                {
                    // 새롭게 할당된 배는 머티리얼을 배치모드용 머티리얼로 설정
                    selectedShip.Renderer.material = ShipManager.Inst.TempShipMaterial;
                }
            }
        }
    }

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
        if (SelectedShip != null)   // 이미 선택 중인 배가 있다면
        {
            SelectedShip.gameObject.SetActive(false);   // 비활성화 하고
        }
        SelectedShip = ships[(int)(type - 1)];          // 새 배 선택

        if(SelectedShip.IsDeployed) // 이미 배치된 배를 다시 배치하는 경우
        {
            board.UndoShipDeployment(SelectedShip);     // 기존에 배치 되었던 것을 취소
        }

        OnMouseMove(Mouse.current.position.ReadValue());// 마우스 위치로 배 이동
        SelectedShip.gameObject.SetActive(true);        // 배가 보이게 활성화
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
        if (SelectedShip != null)
        {
            if (SelectedShip.IsDeployed)            
            {
                // 이미 배치가 된 배면, 기존 함선배치를 취소
                board.UndoShipDeployment(SelectedShip);
            }

            // 클릭한 위치에 함선 배치 시도
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            bool result = board.ShipDeployment(SelectedShip, worldPos);
            if (result)
            {
                SelectedShip.transform.position -= Vector3.up * 2;
                SelectedShip = null;    // 배치에 성공했으면 SelectedShip 해제
            }
        }
    }

    protected virtual void OnClick_Battle(Vector2 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        bool result = board.Attacked(worldPos);

    }

    protected virtual void OnClick_GameEnd(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_Title(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_ShipDeployment(Vector2 screenPos)
    {
        // 배치할 배를 선택중이고 아직 배치가 안됬을 때
        if (SelectedShip != null && !SelectedShip.IsDeployed)
        {
            // 마우스 포인터가 존재하는 그리드에 따라 함선 이동
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            Vector2Int gridPos = board.WorldToGrid(worldPos);
            worldPos = board.GridToWorld(gridPos);

            SelectedShip.transform.position = worldPos + Vector3.up * 2;

            bool isSuccess = board.IsShipDeployment(SelectedShip, gridPos);
            ShipManager.Inst.SetTempShipColor(isSuccess);
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
        if (SelectedShip != null)
        {
            // 휠버튼 돌렸을 때 돌리는 방향에 따라 배치할 함선 회전
            bool ccw = false;
            if (wheel > 0)
                ccw = true;

            SelectedShip.Rotate(ccw);
        }
    }

    private void OnMouseWheel_Battle(float wheel)
    {
    }

    private void OnMouseWheel_GameEnd(float wheel)
    {
    }
}
