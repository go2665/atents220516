using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 남은 시간 표시용 클래스
/// </summary>
public class TimeCounter : MonoBehaviour
{
    /// <summary>
    /// 일시 정지 여부
    /// </summary>
    bool isPause = true;

    /// <summary>
    /// 현재 진행 시간
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// 시간을 표시할 이미지 넘버
    /// </summary>
    ImageNumber imageNumber;

    public int CountTime => imageNumber.Number;

    private void Awake()
    {
        // 컴포넌트 찾기
        imageNumber = GetComponent<ImageNumber>();
    }

    private void Start()
    {
        // 게임 매니저의 델리게이트에 함수 연결
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameStart += TimerReset;
        gameManager.onGameStart += TimerStart;
        gameManager.onGameReset += TimerReset;
        gameManager.onGameClear += TimerPause;
        gameManager.onGameOver += TimerPause;
    }

    private void Update()
    {
        if (!isPause)   // 일시 정지 상태가 아니면
        {
            elapsedTime += Time.deltaTime;          // 시간 계속 누적하고
            imageNumber.Number = (int)elapsedTime;  // 이미지 넘버에 반영
        }
    }

    /// <summary>
    /// 타이머 정지시키는 함수. 더이상 시간이 증가되지 않게 만든다.
    /// </summary>
    private void TimerPause()
    {
        isPause = true;
    }

    /// <summary>
    /// 타이머 누적시간을 다시 0초로 만드는 함수
    /// </summary>
    private void TimerReset()
    {
        isPause = true;
        elapsedTime = 0.0f;
        imageNumber.Number = 0;
    }

    /// <summary>
    /// 타이머 시작시키는 함수
    /// </summary>
    private void TimerStart()
    {
        isPause = false;
    }
}
