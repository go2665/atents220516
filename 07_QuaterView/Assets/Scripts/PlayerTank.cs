using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTank : Tank
{
    enum ShellType
    {
        Normal = 0,
        BadZone,
        Cluster
    }

    enum ShortCutType
    {
        Key1 = 0,
        Key2
    }    

    public float moveSpeed = 3.0f;          // 이동 속도
    public float turnSpeed = 3.0f;          // 회전 속도
    public float turretTurnSpeed = 0.05f;   // 포탑 회전 속도

    public Action<int> onSpecialShellChange;

    private Quaternion turretTargetRotation = Quaternion.identity;  // 포탑의 회전    

    private ShellType specialShell = ShellType.BadZone;         // 특수탄(우클릭발사) 종류

    private Skill_Barrier barrier;

    private TankInputActions inputActions;                      // 액션맵
    private Vector2 inputDir = Vector2.zero;                    // 입력받은 이동 방향
    private Action<InputAction.CallbackContext> onShortcut1;    // 단축키 1번용 함수 저장용 델리게이트
    private Action<InputAction.CallbackContext> onShortcut2;    // 단축키 2번용 함수 저장용 델리게이트


    // 유니티 이벤트 함수 ---------------------------------------------------------------------------------
    protected override void Awake()
    {
        base.Awake();   // 컴포넌트 찾고 FireData 만들기

        barrier = GetComponent<Skill_Barrier>();

        inputActions = new TankInputActions();              // 액션맵 생성
        onShortcut1 = (_) => ShortCut(ShortCutType.Key1);   // 단축키 액션과 연결하고 해재할 람다함수 저장
        onShortcut2 = (_) => ShortCut(ShortCutType.Key2);
    }

    private void OnEnable()
    {
        // 입력 액션과 함수 연결
        inputActions.Tank.Enable();
        inputActions.Tank.Move.performed += OnMove;
        inputActions.Tank.Move.canceled += OnMove;
        inputActions.Tank.Look.performed += OnMouseMove;
        inputActions.Tank.NormalFire.performed += OnNormalFire;
        inputActions.Tank.SpecialFire.performed += OnSpecialFire;
        inputActions.Tank.ShortCut1.performed += onShortcut1;
        inputActions.Tank.ShortCut2.performed += onShortcut2;
        inputActions.Tank.Skill_Barrier.performed += OnBarrierActivate;
    }

    private void OnDisable()
    {
        // 입력 액션과 함수 연결 해제
        inputActions.Tank.Skill_Barrier.performed -= OnBarrierActivate;
        inputActions.Tank.ShortCut2.performed -= onShortcut2;
        inputActions.Tank.ShortCut1.performed -= onShortcut1;
        inputActions.Tank.SpecialFire.performed -= OnSpecialFire;
        inputActions.Tank.NormalFire.performed -= OnNormalFire;
        inputActions.Tank.Look.performed -= OnMouseMove;
        inputActions.Tank.Move.canceled -= OnMove;
        inputActions.Tank.Move.performed -= OnMove;
        inputActions.Tank.Disable();
    }

    protected override void Start()
    {
        CoolTimePanel coolTimePanel = FindObjectOfType<CoolTimePanel>();
        fireDatas[0].onCoolTimeChange += coolTimePanel[0].RefreshUI;
        fireDatas[1].onCoolTimeChange += coolTimePanel[1].RefreshUI;
        fireDatas[2].onCoolTimeChange += coolTimePanel[2].RefreshUI;
        barrier.onCoolTimeChange += coolTimePanel[3].RefreshUI;
        barrier.onDurationTimeChange += coolTimePanel[3].RefreshUI;
        barrier.onDurationMode += coolTimePanel[3].SetDurationMode;
        onSpecialShellChange += coolTimePanel.SetSelected;

        base.Start();
    }

    protected override void Update()
    {
        base.Update();  // 쿨타임 처리
        TurretTurn();   // 포탑만 돌리기
    }

    private void FixedUpdate()
    {
        // 이동 처리        
        rigid.AddForce(inputDir.y * moveSpeed * transform.forward); // 전진 후진
        rigid.AddTorque(inputDir.x * turnSpeed * transform.up);     // 좌회전 우회전
    }

    // 입력 액션 바인딩 함수 -------------------------------------------------------------------------------
    void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();    // 이동 입력 받은 것 전달.
    }

    void OnMouseMove(InputAction.CallbackContext context)
    {
        // 포탑 회전 처리용(마우스가 움직일 때만 실행)
        Vector2 screenPos = context.ReadValue<Vector2>();       // 마우스의 위치 = 마우스의 스크린좌표
        Ray ray = Camera.main.ScreenPointToRay(screenPos);      // 마우스의 스크린좌표를 이용해 레이 계산
        if( Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")) )    // 레이를 이용해 레이케스팅
        {
            //hit.point;    // 마우스가 가리키는 땅 위치
            Vector3 lookDir = hit.point - turret.transform.position;    // 탱크 위치에서 마우스가 가리키는 바닥지점으로 향하는 방향벡터
            lookDir.y = 0.0f;               // y 제거
            lookDir = lookDir.normalized;   // 노멀라이즈.
            turretTargetRotation = Quaternion.LookRotation(lookDir);    // 최종적으로 포탑이 해야할 회전 계산
        }
    }

    private void OnNormalFire(InputAction.CallbackContext _)
    {
        // 좌클릭으로 일반 포탄 발사
        Fire(ShellType.Normal);
    }

    private void OnSpecialFire(InputAction.CallbackContext _)
    {
        // 우클릭으로 특수탄 발사
        Fire(specialShell);
    }

    private void OnBarrierActivate(InputAction.CallbackContext _)
    {
        barrier.UseSkill();
    }

    // 일반 함수들 -------------------------------------------------------------------------------------
    private void Fire(ShellType type)
    {
        if (fireDatas[(int)type].IsFireReady)       // 쿨타임 확인하고 발사 가능하면
        {
            Instantiate(shellPrefabs[(int)type], firePosition.position, firePosition.rotation); // 지정된 포탄 발사
            fireDatas[(int)type].ResetCoolTime();   // 쿨타임 다시 돌리기
        }
    }

    void TurretTurn()
    {
        if (!isDead)
        {
            // Update에서 계속 실행
            // turretTargetRotation이 될때까지 회전        
            turret.rotation = Quaternion.Slerp(turret.rotation, turretTargetRotation, turretTurnSpeed * Time.deltaTime);
        }
    }

    void ShortCut(ShortCutType key)
    {
        // 단축키 처리
        switch (key)
        {
            case ShortCutType.Key1:
                specialShell = ShellType.BadZone;
                break;
            case ShortCutType.Key2:
                specialShell = ShellType.Cluster;
                break;
            default:
                break;
        }
        onSpecialShellChange?.Invoke((int)key);
    }

    public override void TakeDamage(float damage)
    {
        if (!barrier.IsSkillActivate)
        {
            base.TakeDamage(damage);
        }
    }

    public override void Dead()
    {
        base.Dead();        
        inputActions.Tank.Disable();
    }
}
