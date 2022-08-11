using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float speed = 3.0f;
    
    /// <summary>
    /// 미리 캐싱해놓을 컴포넌트들
    /// </summary>
    Rigidbody2D rigid;
    Animator anim;
    
    /// <summary>
    /// 인풋 액션 클래스
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 마지막으로 입력 된 방향
    /// </summary>
    Vector2 dir;

    Light2D spotLight;

    private void Awake()
    {
        // 컴포넌트 가져오고
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spotLight = transform.GetChild(1).GetComponent<Light2D>();

        // 인풋 액션 만들기
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // 인풋 액션과 함수 연결
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        
        // 인풋 액션의 Player 액션맵 활성화
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        // 연결했던 것 해제
        inputActions.Player.Move.canceled -= OnStop;
        inputActions.Player.Move.performed -= OnMove;

        // 액션맵 비활성화
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        // 입력 받은 방향에 따라 리지드바드를 이용해 움직이기
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * speed * dir);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // dir 갱신
        dir = context.ReadValue<Vector2>();
        List<ContactPoint2D> temp = new List<ContactPoint2D>();
        if(rigid.GetContacts(temp) == 0 )   // 충돌한 다른 물체가 없으면 노멀라이즈
        {
            dir = dir.normalized;
        }

        // 바라보는 방향으로 스포트라이트 회전시키기
        //float angle = Vector3.SignedAngle(Vector3.up, (Vector3)dir, Vector3.forward);
        //spotLight.transform.rotation = Quaternion.Euler(0, 0, angle);
        spotLight.transform.up = dir;

        // 애니메이터의 파라메터도 갱신
        anim.SetBool("IsMove", true);   // Move로 트랜지션이 일어나게 만들기
        anim.SetFloat("InputX", dir.x); // 방향도 기록
        anim.SetFloat("InputY", dir.y);
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        //dir = context.ReadValue<Vector2>();
        dir = Vector2.zero;             // 방향을 제거
        anim.SetBool("IsMove", false);  // Idle로 트랜지션이 일어나게 만들기
    }
}
