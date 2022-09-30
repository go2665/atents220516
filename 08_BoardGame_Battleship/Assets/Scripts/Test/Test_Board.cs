using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Board : MonoBehaviour
{
    public Board board;
    public GameObject shipPrefab;
    public UserPlayer userPlayer;

    PlayerInputActions inputActions;


    private void Awake()
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
    }

    private void OnDisable()
    {
        inputActions.Test.Disable();
        inputActions.Test.Test5.performed -= OnTest5;
        inputActions.Test.Test4.performed -= OnTest4;
        inputActions.Test.Test3.performed -= OnTest3;
        inputActions.Test.Test2.performed -= OnTest2;
        inputActions.Test.Test1.performed -= OnTest1;
        inputActions.Test.Click.performed -= OnTestClick;
    }

    private void OnTest1(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Carrier);
    }

    private void OnTest2(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Battleship);
    }

    private void OnTest3(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Destroyer);
    }

    private void OnTest4(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Submarine);
    }

    private void OnTest5(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.PatrolBoat);
    }

    private void OnTestClick(InputAction.CallbackContext obj)
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
        userPlayer.Test_SetState(PlayerState.ShipDeployment);   // 테스트 유저의 상태를 함선 배치모드로 설정
        userPlayer.SelectShipToDeploy(ShipType.Carrier);      // 현재 배치할 배를 잠수함으로 설정

        userPlayer.Test_SetState(PlayerState.Battle);


    }

}
