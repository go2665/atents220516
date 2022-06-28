using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 파이프들을 계속 재활용하며 움직이는 클래스
/// </summary>
public class PipeRotator : MonoBehaviour
{
    /// <summary>
    /// 파이프들의 이동 속도
    /// </summary>
    public float moveSpeed = 6.0f;

    /// <summary>
    /// 파이프간의 간격
    /// </summary>
    public float width = 4.0f;

    /// <summary>
    /// 파이프가 밖으로 나간 것을 확인할 x좌표 값
    /// </summary>
    const float X_RANGE_OUT = -10.0f;

    const float X_START = 10.0f;

    /// <summary>
    /// PipeRotator가 가지고 있는 파이프들
    /// </summary>
    Pipe[] children;

    private void Start()
    {
        children = new Pipe[transform.childCount];          // 자식 파이프 수만큼 배열 확보
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).GetComponent<Pipe>();   // 파이프 가져와서 저장하고
            children[i].Set((width * i + X_START) * Vector2.right);     // 초기 위치 설정
        }
    }

    private void FixedUpdate()
    {
        foreach (Pipe pipe in children)
        {
            pipe.Move(Time.fixedDeltaTime * moveSpeed * Vector2.left);      // 우선 파이프를 왼쪽으로 움직임

            if( pipe.transform.position.x < X_RANGE_OUT)                    // 파이프가 X축 기준으로 일정 이상 이동했으면
            {
                pipe.Move(width * transform.childCount * Vector2.right);    // 오른쪽 끝으로 이동시킴
                pipe.ResetHeight();                                         // 높이도 다시 랜덤으로 리셋
            }
        }
    }
}
