using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 웨이포인트 하나
/// </summary>
public class Waypoint : MonoBehaviour
{
    public System.Action<Transform> OnWaypointArrive { get; set; }  // 이 웨이포인트에 도착했을 때 실행될 델리게이트

    Transform next = null;  // 다음 웨이포인트
    public Transform Next { set => next = value; }  // next(쓰기 전용 프로퍼티)


    private void OnTriggerEnter(Collider other)
    {
        IWaypointUser user = other.GetComponent<IWaypointUser>();   // 웨이포인트 유저면
        if (user != null)
        {
            OnWaypointArrive(next); // 다음 웨이포인트로 이동
        }
    }
}
