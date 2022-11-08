using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{    
    /// <summary>
    /// 셀 이미지 매니저 접근용 변수
    /// </summary>
    CellImageManager cellImage;

    /// <summary>
    /// 게임이 시작되었는지 여부(게임 시작은 첫번째 셀을 열었을 때 시작됨)
    /// </summary>
    bool isGameStart = false;

    // 프로퍼티 ------------------------------------------------------------------------------------

    /// <summary>
    /// 셀 이미지 매니저에 접근하기 위한 프로퍼티
    /// </summary>
    public CellImageManager CellImage => cellImage;

    // 델리게이트 ----------------------------------------------------------------------------------

    /// <summary>
    /// 게임이 시작될 때 실행될 델리게이트
    /// </summary>
    public Action onGameStart;

    /// <summary>
    /// 게임이 재시작될 때 실행될 델리게이트(리셋버튼 눌렀을 때)
    /// </summary>
    public Action onGameReset;

    /// <summary>
    /// 게임 오버가 되었을 때(지뢰를 밟았을 때) 실행될 델리게이트
    /// </summary>
    public Action onGameOver;

    /// <summary>
    /// 게임 클리어를 했을 때(모든 지뢰를 다 표시하고 남은 모든 셀을 열었을 때) 실행될 델리게이트
    /// </summary>
    public Action onGameClear;

    // 함수들 --------------------------------------------------------------------------------------

    /// <summary>
    /// Awake 때 실행되는 초기화 함수
    /// </summary>
    protected override void Initialize()
    {
        // 컴포넌트 찾기
        cellImage = GetComponent<CellImageManager>();
    }

    /// <summary>
    /// 게임을 시작을 알리는 함수
    /// </summary>
    public void GameStart()
    {
        if(!isGameStart)
        {
            isGameStart = true;     // 처음 한번만 실행되게 설정
            onGameStart?.Invoke();  // 델리게이트로 알림
        }
    }

    /// <summary>
    /// 게임 재시작을 알리는 함수
    /// </summary>
    public void GameReset()
    {
        // 씬을 다시 부를 필요는 없음
        isGameStart = false;
        onGameReset?.Invoke();
    }

    /// <summary>
    /// 게임 오버를 알리는 함수
    /// </summary>
    public void GameOver()
    {
        onGameOver?.Invoke();        
    }

    /// <summary>
    /// 게임 클리어를 알리는 함수
    /// </summary>
    public void GameClear()
    {
        Debug.Log("게임 클리어");

        onGameClear?.Invoke();

    }
}
