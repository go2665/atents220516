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
    /// 입력 받은 방향을 가공하여 최종적으로 움직일 방향
    /// </summary>
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// 바라볼 방향을 만드는 회전
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;

    /// <summary>
    /// 오브젝트의 생성 직후 호출
    /// </summary>
    private void Awake()
    {
        actions = new();    // 액션맵 객체 생성
        controller = GetComponent<CharacterController>();   //캐릭터 컨트롤러 컴포넌트 가져오기
    }

    /// <summary>
    /// awake 이후 오브젝트가 활성화 될 때 실행
    /// </summary>
    private void OnEnable()
    {
        actions.Player.Enable();                    // "Player" 액션맵 켜기
        actions.Player.Move.performed += OnMove;    // "Player" 액션맵에 함수 등록
        actions.Player.Move.canceled += OnMove;
    }

    /// <summary>
    /// 오브젝트가 비활성화 될 때 실행
    /// </summary>
    private void OnDisable()
    {
        actions.Player.Move.canceled -= OnMove;
        actions.Player.Move.performed -= OnMove;    // 등록해 놓았던 함수 해제
        actions.Player.Disable();                   // "Player" 액션맵 끄기
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
        }
    }

    /// <summary>
    /// 매 프레임마다 호출
    /// </summary>
    private void Update()
    {
        // 캐릭터 이동
        controller.Move(runSpeed * Time.deltaTime * inputDir);

        // 목표지점을 바라보도록 회전하며 보간
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
