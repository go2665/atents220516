using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 스테이지의 지뢰 배치 타이밍을 조절하기 위한 클래스(게임 실행했을 때 단 한번만 존재)
// (스테이지를 덮고 있다가 이 커버를 클릭하면 지뢰 배치하고 사라짐)
public class StageCover : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 게임 시작 알림 용도
    /// </summary>
    public Action onStartClick;     // Stage 클래스에서 사용.

    private void Start()
    {
        // 제일 뒤로 보내서 맨 위에 그려지게끔 배치
        transform.SetAsLastSibling();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭이 발생하면
        onStartClick?.Invoke();     // 신호보내고
        Destroy(this.gameObject);   // 사라지기
    }
}
