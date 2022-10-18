using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글톤으로 구성된 게임 관리용 클래스
/// </summary>
public class GameManager : Singleton<GameManager>
{
    // 입력용 --------------------------------------------------------------------------------------
    /// <summary>
    /// 입력 처리용 컴포넌트
    /// </summary>
    InputController input;

    /// <summary>
    /// 인풋 컨트롤러에 접근하기 위한 프로퍼티
    /// </summary>
    public InputController Input { get => input; }
    // --------------------------------------------------------------------------------------------

    // 플레이어들 ----------------------------------------------------------------------------------
    private UserPlayer userPlayer;
    private EnemyPlayer enemyPlayer;

    public UserPlayer UserPlayer => userPlayer;
    public EnemyPlayer EnemyPlayer => enemyPlayer;
    // --------------------------------------------------------------------------------------------

    // 게임 상태 -----------------------------------------------------------------------------------
    /// <summary>
    /// 게임 상태를 나타내는 변수
    /// </summary>
    private GameState state = GameState.Title;

    /// <summary>
    /// 상태 설정 및 확인용 프로퍼티(설정은 자신만 가능)
    /// </summary>
    public GameState State
    {
        get => state;
        private set
        {
            state = value;                  // 상태 변경하고
            onStateChange?.Invoke(state);   // 델리게이트에 연결된 함수들 실행
        }
    }

    /// <summary>
    /// 상태가 변경되면 실행될 델리게이트
    /// </summary>
    Action<GameState> onStateChange;
    // --------------------------------------------------------------------------------------------


    protected override void Awake()
    {
        base.Awake();

        input = GetComponent<InputController>();    // 인풋 컨트롤러 찾기
    }

    protected override void Initialize()
    {
        userPlayer = FindObjectOfType<UserPlayer>();
        enemyPlayer = FindObjectOfType<EnemyPlayer>();

        onStateChange = null;   // 씬이 다시 로드되었을 때 이전에 연결된 함수들을 제거

        onStateChange += userPlayer.OnStateChange;  // 플레이어의 상태처리 함수 연결
        onStateChange += enemyPlayer.OnStateChange;
    }


    /// <summary>
    /// (테스트)게임 상태 설정
    /// </summary>
    /// <param name="state">설정할 상태</param>
    public void Test_SetState(GameState state)
    {
        State = state;
    }
}
