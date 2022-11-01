using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    /// <summary>
    /// 걷는 속도
    /// </summary>
    public float walkSpeed = 3.5f;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 3.5f;

    /// <summary>
    /// 인풋 액션맵
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 캐릭터 컨트롤러 컴포넌트
    /// </summary>
    CharacterController controller;

    Animator anim;

    // NetworkVariable
    // Netcode for GameObjects에서 네트워크를 통해 데이터를 공유하기 위해 사용하는 타입
    // 공유 가능한 데이터 타입은 unmanaged 타입만 가능(대략적으로 값타입만 가능)
    // 생성할 때 읽기/쓰기 권한 설정 가능

    /// <summary>
    /// 이동 정도를 기록할 네트워크 변수
    /// </summary>
    NetworkVariable<Vector3> networkMoveDelta = new NetworkVariable<Vector3>();

    /// <summary>
    /// 회전 정도를 기록할 네트워크 변수
    /// </summary>
    NetworkVariable<float> networkRotateDelta = new NetworkVariable<float>();

    private void Awake()
    {
        inputActions = new PlayerInputActions();            // 인풋 액션 만들고
        controller = GetComponent<CharacterController>();   // 컴포넌트 가져오기
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();                       // 입력 활성화하고 함수 연결
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        ClientMoveAndRotate();  // 이동과 회전 처리
    }

    /// <summary>
    /// 이동 입력이 들어왔을 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (IsClient && IsOwner)    // NetworkBehaviour라서 사용 가능. 이 네트워크 게임 오브젝트의 소유자이면서 클라이언트일 때만 실행.
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector3 moveDelta = moveInput.y * walkSpeed * transform.forward;    // 앞뒤로 이동 정도
            float rotateDelta = moveInput.x * rotateSpeed;                      // 좌우로 회전 정도

            UpdateClientMoveAndRotateServerRpc(moveDelta, rotateDelta);         // ServerRpc를 통해 서버에 변경 사항을 알림

            if( moveInput.y > 0)
            {
                anim.SetTrigger("Walk");
            }
            else if( moveInput.y < 0 )
            {
                anim.SetTrigger("BackWalk");
            }
            else
            {
                anim.SetTrigger("Idle");
            }
        }
    }

    /// <summary>
    /// 클라이언트 캐릭터를 실제로 움직이고 회전시키는 함수
    /// </summary>
    void ClientMoveAndRotate()
    {
        if (networkMoveDelta.Value != Vector3.zero)
        {
            // 이동에 변화가 있으면 이동 처리
            controller.SimpleMove(networkMoveDelta.Value);
        }
        if (networkRotateDelta.Value != 0.0f)
        {
            // 회전에 변화가 있으면 회전 처리
            transform.Rotate(0, networkRotateDelta.Value * Time.deltaTime, 0, Space.World);
        }
    }

    // [ServerRpc] : 서버가 실행하는 함수라는 표시

    /// <summary>
    /// 네트워크 변수를 서버에서 변경 처리하는 함수
    /// </summary>
    /// <param name="moveDelta">이동 정도</param>
    /// <param name="rotateDelta">회전 정도</param>
    [ServerRpc]
    public void UpdateClientMoveAndRotateServerRpc(Vector3 moveDelta, float rotateDelta)
    {
        // 네트워크 변수의 경우 기본적으로 쓰기 권한은 서버만 가진다.
        // 그래서 ServerRpc를 통해 서버가 네트워크 수정하도록 요청.
        networkMoveDelta.Value = moveDelta;
        networkRotateDelta.Value = rotateDelta;
    }
    
}
