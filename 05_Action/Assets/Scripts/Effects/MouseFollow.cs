using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollow : MonoBehaviour
{
    [Range(1.0f, 20.0f)]
    public float distance = 10.0f;
    [Range(0.01f, 1.0f)]
    public float speed = 0.1f;

    private void Update()
    {
        // 마우스의 위치를 스크린 좌표계로 받아옴(원점은 화면의 왼쪽 아래, 크기는 화면 해상도)
        Vector3 mousePosition = Mouse.current.position.ReadValue(); 
        mousePosition.z = distance;
        Debug.Log(mousePosition);

        // 카메라 클래스를 이용해 스크린 좌표를 월드 좌표로 변경할 수 있다.
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);

        // 천천히 따라가는 효과를 주기
        transform.position = Vector3.Lerp(transform.position, target, speed);
    }
}
