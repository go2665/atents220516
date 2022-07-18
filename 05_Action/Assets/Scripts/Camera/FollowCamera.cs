using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// target과의 처음 거리를 유지하며 쫒아다니는 클래스
/// </summary>
public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// 따라다닐 대상의 트랜스폼.
    /// </summary>
    public Transform target = null;

    /// <summary>
    /// 따라다니는 속도(1/speed초에 걸쳐 따라다님)
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 처음에 타겟과 떨어져 있던 거리
    /// </summary>
    Vector3 offset = Vector3.zero;

    private void Start()
    {
        // target이 없으면 플레이어를 target으로 지정
        if(target == null)
        {
            target = FindObjectOfType<PlayerInputController>().transform;
        }
        offset = transform.position - target.position;  // 타겟의 위치에서 이 오브젝트로 이동하는 방향 벡터
    }

    /// <summary>
    /// 모든 Update 함수가 실행되고 난 후에 실행
    /// </summary>
    private void LateUpdate()
    {
        // 보간을 통해 위치를 조정
        transform.position = Vector3.Lerp(
            transform.position,         // 현재 내 위치에서
            target.position + offset,   // (target에 위치)에서 (내가 처음 떨어져 있던 간격만큼 떨어져 있는 위치)로 이동
            speed * Time.deltaTime);    // 초당 1/speed 만큼 변화
    }
}
