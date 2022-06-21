using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 웨이포인트를 사용할 수 있는 오브젝트라는 것을 표시
/// </summary>
public interface IWaypointUser
{
    void SetNextWayPoint(Transform newTarget);    // 다음 웨이포인트 지점을 설정하기 위한 함수
}
