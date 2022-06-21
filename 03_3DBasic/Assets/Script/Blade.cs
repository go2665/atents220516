using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour, IWaypointUser   // IWaypointUser를 상속해서 웨이포인트를 사용 가능하다고 표시
{
    public float spinSpeed = 1000.0f;   // 회전 속도
    public float moveSpeed = 2.0f;      // 이동 속도
    
    Transform target = null;            // 이동할 곳
    Vector3 dir;                        // 이동 방향(칼날이 회전하기 때문에 월드 좌표로 가지고 있어야 함)

    private void FixedUpdate()
    {
        transform.Rotate(spinSpeed * Time.fixedDeltaTime, 0, 0);                    // 회전
        transform.Translate(moveSpeed * Time.fixedDeltaTime * dir, Space.World);    // 이동
    }

    /// <summary>
    /// 웨이포인트에서 다음 목표 지점 설정하도록 만들어진 델리게이트 등록용 함수.
    /// </summary>
    /// <param name="newTarget">다음 웨이포인트 지점</param>
    public void SetNextWayPoint(Transform newTarget)
    {
        target = newTarget;             // 다음 이동할 곳을 지정
        transform.LookAt(target);       // 다음 이동할 곳을 바라보게 만듬
        dir = target.position - transform.position; // 이동할 방향 계산
        dir = dir.normalized;                       // 크기를 1로 만들어서 방향만 남기기
    }
}
