using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target = null;
    public float speed = 3.0f;
    Vector3 offset;

    private void Start()
    {
        if(target == null)
        {
            target = GameManager.Inst.Player.transform; // 시작할 때 타겟이 없으면 플레이어 설정
        }
        offset = transform.position - target.position;  // 카메라와 대상의 위치 차이 저장
    }

    private void FixedUpdate()
    {
        // 원래 대상과 카메라의 간격을 유지하게 하는 코드
        // lerp를 사용하여 부드럽게 이어지도록 설정
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.fixedDeltaTime);        
    }
}
