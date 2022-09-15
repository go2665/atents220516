using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTank : MonoBehaviour, IHit
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

    public GameObject[] shellPrefabs;       // 포탄용 프리팹
    public Transform firePosition;          // 발사 위치

    public float moveSpeed = 3.0f;          // 이동 속도
    public float turnSpeed = 3.0f;          // 회전 속도
    public float turretTurnSpeed = 0.05f;   // 포탑 회전 속도

    private Transform turret;               // 포탑의 트랜스폼
    private Quaternion turretTargetRotation = Quaternion.identity;  // 포탑의 회전

    FireData[] fireDatas;

    TankInputActions inputActions;          // 액션맵
    Action<InputAction.CallbackContext> onShortcut1;
    Action<InputAction.CallbackContext> onShortcut2;
    ShellType specialShell = ShellType.BadZone;

    Vector2 inputDir = Vector2.zero;        // 이력받은 이동 방향

    Rigidbody rigid;

    float hp;
    public float maxHP = 100.0f;
    bool isDead = false;

    public float HP 
    { 
        get => hp;
        set
        {
            hp = value;
            //Debug.Log(hp);
            if (hp < 0)
            {
                hp = 0;
                if (!isDead) // HP가 0보다 작아지면 Dead함수 실행
                    Dead();
            }
            hp = Mathf.Min(hp, maxHP);
        }  
    }

    public float MaxHP { get => maxHP; }

    public Action onHealthChange { get; set; }
    public Action onDead { get; set; }

    private void Awake()
    {
        inputActions = new TankInputActions();  // 액션맵 생성
        rigid = GetComponent<Rigidbody>();      // 리지드바디 가져오기
        turret = transform.Find("TankRenderers").Find("TankTurret");    // 포탑 찾고
        firePosition = turret.GetChild(0);      // 발사 위치 찾기
        
        fireDatas = new FireData[shellPrefabs.Length];
        for(int i=0;i<shellPrefabs.Length;i++)
        {
            Shell shell = shellPrefabs[i].GetComponent<Shell>();
            fireDatas[i] = new FireData(shell.data);
        }

        onShortcut1 = (_) => ShortCut(ShortCutType.Key1);
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
    }

    private void OnDisable()
    {
        // 입력 액션과 함수 연결 해제
        inputActions.Tank.ShortCut2.performed -= onShortcut2;
        inputActions.Tank.ShortCut1.performed -= onShortcut1;
        inputActions.Tank.SpecialFire.performed -= OnSpecialFire;
        inputActions.Tank.NormalFire.performed -= OnNormalFire;
        inputActions.Tank.Look.performed -= OnMouseMove;
        inputActions.Tank.Move.canceled -= OnMove;
        inputActions.Tank.Move.performed -= OnMove;
        inputActions.Tank.Disable();
    }

    private void Update()
    {
        foreach (var data in fireDatas)
        {
            data.CurrentCoolTime -= Time.deltaTime;
        }

        TurretTurn();   // 포탑만 돌리기
    }

    private void FixedUpdate()
    {
        // 이동 처리        
        rigid.AddForce(inputDir.y * moveSpeed * transform.forward);
        rigid.AddTorque(inputDir.x * turnSpeed * transform.up);
    }

    void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();    // 이동 입력 받은 것 전달.
    }

    void OnMouseMove(InputAction.CallbackContext context)
    {
        // 마우스가 움직일 때만 실행
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
        Fire(ShellType.Normal);
    }

    private void OnSpecialFire(InputAction.CallbackContext _)
    {
        Fire(specialShell);
    }

    private void Fire(ShellType type)
    {
        if (fireDatas[(int)type].IsFireReady)
        {
            Instantiate(shellPrefabs[(int)type], firePosition.position, firePosition.rotation); // 지정된 포탄 발사
            fireDatas[(int)type].ResetCoolTime();
        }
    }

    void TurretTurn()
    {
        // Update에서 계속 실행
        // turretTargetRotation이 될때까지 회전
        turret.rotation = Quaternion.Slerp(turret.rotation, turretTargetRotation, turretTurnSpeed * Time.deltaTime);
    }

    void ShortCut(ShortCutType key)
    {
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
    }

    public void Dead()
    {
        isDead = true;
        GetComponent<Collider>().enabled = false;
        inputActions.Tank.Disable();
    }
}
