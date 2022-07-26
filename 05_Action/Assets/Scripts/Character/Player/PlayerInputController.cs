using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// 입력에 따른 플레이어의 행동을 처리할 클래스
/// </summary>
public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// 달릴때 속도
    /// </summary>
    public float runSpeed = 6.0f;

    /// <summary>
    /// 걸을때 속도
    /// </summary>
    public float walkSpeed = 3.0f;

    /// <summary>
    /// 이동 모드 지정용 enum
    /// </summary>
    enum MoveMode
    {
        Walk = 0,
        Run
    }
    /// <summary>
    /// 기본 이동모드로 Run 선택
    /// </summary>
    MoveMode moveMode = MoveMode.Run;

    /// <summary>
    /// 회전할 때 속도. 1/turnSpeed초에 걸쳐 회전
    /// </summary>
    public float turnSpeed = 10.0f;

    /// <summary>
    /// 액션맵 객체
    /// </summary>
    PlayerInputActions actions;

    /// <summary>
    /// kinematic으로 사용하는 리지드바디보다 가벼운 이동용 컴포넌트
    /// </summary>
    CharacterController controller;

    /// <summary>
    /// 애니메이터 컴포넌트
    /// </summary>
    Animator anim;
    
    /// <summary>
    /// 입력 받은 방향을 가공하여 최종적으로 움직일 방향
    /// </summary>
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// 바라볼 방향을 만드는 회전
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;


    /// <summary>
    /// 같은 오브젝트에 들어있는 Player 컴포넌트
    /// </summary>
    Player player;


    /// <summary>
    /// 오브젝트의 생성 직후 호출
    /// </summary>
    private void Awake()
    {
        actions = new();    // 액션맵 객체 생성
        controller = GetComponent<CharacterController>();   //캐릭터 컨트롤러 컴포넌트 가져오기
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    /// <summary>
    /// awake 이후 오브젝트가 활성화 될 때 실행
    /// </summary>
    private void OnEnable()
    {
        actions.Player.Enable();                    // "Player" 액션맵 켜기
        actions.Player.Move.performed += OnMove;    // "Player" 액션맵에 함수 등록
        actions.Player.Move.canceled += OnMove;
        actions.Player.MoveModeChange.performed += OnMoveModeChage;
        actions.Player.Attack.performed += OnAttack;
        actions.Player.LockOn.performed += OnLockOn;
        actions.Player.Pickup.performed += OnPickup;
        actions.ShortCut.Enable();                  // "ShorCut" 액션맵 켜기
        actions.ShortCut.InventoryOnOff.performed += OnInventoryShortcut;
    }

    /// <summary>
    /// 오브젝트가 비활성화 될 때 실행
    /// </summary>
    private void OnDisable()
    {
        actions.ShortCut.InventoryOnOff.performed -= OnInventoryShortcut;
        actions.ShortCut.Disable();                 // "ShorCut" 액션맵 끄기
        actions.Player.Pickup.performed -= OnPickup;
        actions.Player.LockOn.performed -= OnLockOn;
        actions.Player.Attack.performed -= OnAttack;
        actions.Player.MoveModeChange.performed -= OnMoveModeChage;
        actions.Player.Move.canceled -= OnMove;
        actions.Player.Move.performed -= OnMove;    // 등록해 놓았던 함수 해제
        actions.Player.Disable();                   // "Player" 액션맵 끄기
    }

    private void Start()
    {
        // InventoryUI에 있는 델리게이트에 람다 함수 등록

        // OnInventoryOpen 델리게이트가 실행될 때 "actions.Player.Disable()"이런 기능이 있는 함수를 실행시킨다.
        GameManager.Inst.InvenUI.OnInventoryOpen += () => actions.Player.Disable();

        // OnInventoryClose 실행될 때 "actions.Player.Enable()"이런 기능이 있는 함수를 실행시킨다.
        GameManager.Inst.InvenUI.OnInventoryClose += () => actions.Player.Enable();
    }

    private void OnInventoryShortcut(InputAction.CallbackContext _)
    {
        GameManager.Inst.InvenUI.InventoryOnOffSwitch();
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        anim.SetFloat("ComboState", Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1.0f));
        anim.ResetTrigger("Attack");
        anim.SetTrigger("Attack");
    }

    /// <summary>
    /// 키 입력 들어오면 모드 변경 (Run <=> Walk)
    /// </summary>    
    private void OnMoveModeChage(InputAction.CallbackContext _)
    {
        if( moveMode == MoveMode.Walk )
        {
            moveMode = MoveMode.Run;
        }
        else
        {
            moveMode = MoveMode.Walk;
        }
    }

    /// <summary>
    /// WASD가 눌러지거나 땠을 때 실행될 함수
    /// </summary>
    /// <param name="context">입력 관련 정보</param>
    private void OnMove(InputAction.CallbackContext context)
    {
        // 입력 받은 값 저장
        Vector2 input = context.ReadValue<Vector2>();
        //Debug.Log(input);

        // 입력 받은 값을 3차원 벡터로 변경. (xz평면으로 변환)
        inputDir.x = input.x;   // 오른쪽 왼쪽
        inputDir.y = 0.0f;
        inputDir.z = input.y;   // 앞 뒤
        //inputDir.Normalize();

        //입력으로 들어온 값이 있는지 확인
        if(inputDir.sqrMagnitude > 0.0f)
        {
            // 카메라의 y축 회전만 따로 분리해서 inputDir에 적용
            inputDir = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * inputDir;
            // 이동하는 방향을 바라보는 회전을 생성
            targetRotation = Quaternion.LookRotation(inputDir);
            // 항상 바닥에 붙도록 처리
            inputDir.y = -2f;
        }
    }

    /// <summary>
    /// 매 프레임마다 호출
    /// </summary>
    private void Update()
    {
        // 지금 플레이어가 락온 중이면
        if( player.LockOnTarget != null )
        {
            // 락온 대상을 바라보게 목표 회전 설정
            targetRotation = Quaternion.LookRotation(player.LockOnTarget.position - transform.position);    
        }

        // 이동 입력 확인
        if (inputDir.sqrMagnitude > 0.0f)
        {
            float speed = 1.0f;
            if( moveMode == MoveMode.Run )
            {
                // 런 모드면 달리는 애니메이션과 6의 이동 속도 설정
                anim.SetFloat("Speed", 1.0f);
                speed = runSpeed;
            }
            else if( moveMode == MoveMode.Walk)
            {
                // 걷기 모드면 걷는 애니메이션과 3의 이동 속도 설정
                anim.SetFloat("Speed", 0.3f);
                speed = walkSpeed;
            }

            // 설정한 이동속도에 맞춰 캐릭터 이동
            controller.Move(speed * Time.deltaTime * inputDir);
        }
        else
        {
            // 입력이 없으면 idle 애니메이션으로 변경
            anim.SetFloat("Speed", 0.0f);
        }
        // 목표지점을 바라보도록 회전하며 보간
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 락온 버튼이 눌러졌을 때 실행될 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnLockOn(InputAction.CallbackContext _)
    {
        player.LockOnToggle();
    }

    /// <summary>
    /// 아이템 줍기 버튼이 눌러졌을 때 실행될 함수
    /// </summary>
    /// <param name="obj"></param>
    private void OnPickup(InputAction.CallbackContext _)
    {
        player.ItemPickup();
    }
}
