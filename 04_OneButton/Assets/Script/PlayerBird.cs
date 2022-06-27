using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBird : MonoBehaviour
{
    public float power = 5.0f;              // 위로 올리는 힘
    public float pitchMaxAngle = 45.0f;     // 위아래 최대 회전 각도

    BirdActions birdInputActions = null;    // 액션맵 파일을 기반으로 자동 생성된 클래스(BirdActions.cs)
    Rigidbody2D rigid = null;               // 2D용 리지드바디

    private void Awake()
    {
        birdInputActions = new();               // BirdActions를 새롭게 new
        rigid = GetComponent<Rigidbody2D>();    // 리지드바디 캐싱해놓기
    }

    private void OnEnable()
    {
        birdInputActions.Player.Enable();   // 스크립트로 InputSystem을 제어할 때는 활성화/비활성화를 수동으로 추가해야함.
        birdInputActions.Player.Tab.performed += OnTab; // Tab 액션이 발동될 때 실행될 함수 등록
    }

    private void OnDisable()
    {
        birdInputActions.Player.Tab.performed -= OnTab; // Tab 액션이 발동될 때 실행될 함수 등록해제
        birdInputActions.Player.Disable();
    }

    private void OnTab(InputAction.CallbackContext _)
    {
        //Debug.Log("Tab");
        rigid.velocity = Vector2.zero;      // 이전에 영향받고 있던 움직임 초기화
        rigid.AddForce(Vector2.up * power, ForceMode2D.Impulse);    // 위쪽으로 힘을 추가
    }

    private void FixedUpdate()
    {
        // velocity y는 +7(1) ~ -7(0)
        // 각도 +45(1) ~ -45(0)
        // 시작값과 끝값이 있다. => 보간
        // 정규화 : 0 ~ 1

        float vel = Mathf.Clamp(rigid.velocity.y, -power, power);               // -7 ~ 7
        //float velNormalized = (vel + power) / (power * 2.0f);                   // 0 ~ 1
        //float angle = (velNormalized * pitchMaxAngle * 2.0f) - pitchMaxAngle;   // -45 ~ +45

        float angle = vel * pitchMaxAngle / power;  // velocity.y * 45도 / 7

        rigid.MoveRotation(angle);
    }
}
