using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Board : TestBase
{
    public Board board;
    public GameObject shipPrefab;
    public UserPlayer userPlayer;
      
    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Carrier);
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Battleship);
    }

    protected override void OnTest3(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Destroyer);
    }

    protected override void OnTest4(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Submarine);
    }

    protected override void OnTest5(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.PatrolBoat);
    }

    protected override void OnTest0(InputAction.CallbackContext obj)
    {
        GameManager.Inst.Test_SetState(GameState.Battle);
    }

    protected override void OnTestClick(InputAction.CallbackContext obj)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screen);

        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, LayerMask.GetMask("Sea")))
        {
            Vector2Int gridPos = board.WorldToGrid(hit.point);
            bool result = board.Attacked(gridPos);
            if(result)
            {
                Debug.Log("공격 성공");
            }
            else
            {
                Debug.Log("공격 실패");
            }
        }

    }

    void Start()
    {
        GameManager.Inst.Test_SetState(GameState.ShipDeployment);   // 테스트 유저의 상태를 함선 배치모드로 설정
        userPlayer.SelectShipToDeploy(ShipType.Carrier);      // 현재 배치할 배를 잠수함으로 설정

        GameManager.Inst.Test_SetState(GameState.Battle);
    }

}
