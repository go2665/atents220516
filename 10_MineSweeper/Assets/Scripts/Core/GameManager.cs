using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{    
    /// <summary>
    /// 게임 상태표시용 enum
    /// </summary>
    public enum GameState
    {
        Ready = 0,      // 시작 전(새로 스테이지 만들어 진 후 첫번째 셀을 오픈하지 않은 상황)
        Play,           // 게임 중(첫째 셀을 오픈 한 이후)
        GameClear,      // 모든 지뢰를 다 찾았을 때
        GameOver        // 지뢰를 밟았을 때
    }

    /// <summary>
    /// 현재 게임 상태
    /// </summary>
    GameState state = GameState.Ready;

    /// <summary>
    /// 셀 이미지 매니저 접근용 변수
    /// </summary>
    CellImageManager cellImage;

    /// <summary>
    /// 리셋 버튼
    /// </summary>
    ResetButton resetButton;


    // 프로퍼티 ------------------------------------------------------------------------------------

    /// <summary>
    /// 셀 이미지 매니저에 접근하기 위한 프로퍼티
    /// </summary>
    public CellImageManager CellImage => cellImage;

    /// <summary>
    /// 리셋 버튼에 접근하기 위한 프로퍼티
    /// </summary>
    public ResetButton ResetBtn => resetButton;

    /// <summary>
    /// 플레이 중인지 확인하는 프로퍼티
    /// </summary>
    public bool IsPlaying => state == GameState.Play;

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
        resetButton = FindObjectOfType<ResetButton>();
    }

    /// <summary>
    /// 게임을 시작을 알리는 함수
    /// </summary>
    public void GameStart()
    {
        if(state == GameState.Ready)
        {
            state = GameState.Play;     // 처음 한번만 실행되게 설정
            onGameStart?.Invoke();      // 델리게이트로 알림
        }
    }

    /// <summary>
    /// 게임 재시작을 알리는 함수
    /// </summary>
    public void GameReset()
    {
        // 씬을 다시 부를 필요는 없음
        state = GameState.Ready;
        onGameReset?.Invoke();
    }

    /// <summary>
    /// 게임 오버를 알리는 함수
    /// </summary>
    public void GameOver()
    {
        Debug.Log("게임 오버");
        state = GameState.GameOver;
        onGameOver?.Invoke();        
    }

    /// <summary>
    /// 게임 클리어를 알리는 함수
    /// </summary>
    public void GameClear()
    {
        Debug.Log("게임 클리어");
        state = GameState.GameClear;
        onGameClear?.Invoke();

    }
}
