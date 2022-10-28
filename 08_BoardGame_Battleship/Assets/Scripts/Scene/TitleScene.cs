using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    PlayerInputActions inputActions;    // 다음 씬으로 넘어가기 위한 입력 받기

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Title.Enable();
        inputActions.Title.AnyKey.performed += OnPressAnyKey;
    }

    private void OnDisable()
    {
        inputActions.Title.AnyKey.performed -= OnPressAnyKey;
        inputActions.Title.Disable();
    }

    private void OnPressAnyKey(InputAction.CallbackContext _)
    {
        // 다음 씬으로 넘어가기 위해 아무 키보드나 마우스 좌우클릭이 되면 실행됨
        //Debug.Log("함선 배치 씬으로 넘어가기");
        SceneManager.LoadScene(1);
    }
}
