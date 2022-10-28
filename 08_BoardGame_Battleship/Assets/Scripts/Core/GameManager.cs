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
    private ShipDeployData[] shipDeployDatas;

    public UserPlayer UserPlayer => userPlayer;
    public EnemyPlayer EnemyPlayer => enemyPlayer;
    // --------------------------------------------------------------------------------------------

    // 게임 상태 -----------------------------------------------------------------------------------
    /// <summary>
    /// 게임 상태를 나타내는 변수
    /// </summary>
    private GameState state = GameState.Title;

    /// <summary>
    /// 상태 설정 및 확인용 프로퍼티
    /// </summary>
    public GameState State
    {
        get => state;
        set
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

    /// <summary>
    /// 씬이 로드되었을 때 호출되는 초기화 함수
    /// </summary>
    protected override void Initialize()
    {
        userPlayer = FindObjectOfType<UserPlayer>();
        enemyPlayer = FindObjectOfType<EnemyPlayer>();

        onStateChange = null;   // 씬이 다시 로드되었을 때 이전에 연결된 함수들을 제거

        // 씬에 따라 특정 플레이어가 없을 수 있음
        if (userPlayer != null)
        {
            onStateChange += userPlayer.OnStateChange;  // 유저 플레이어의 상태처리 함수 연결
        }
        if (enemyPlayer != null)
        {
            // 적은 함선배치모드에서는 없다.
            onStateChange += enemyPlayer.OnStateChange; // 컴퓨터 플레이어의 상태처리 함수 연결
        }
    }

    /// <summary>
    /// 배치한 배의 위치와 방향 저장하는 함수
    /// </summary>
    /// <param name="targetPlayer">저장할 배치정보를 가지고 있는 플레이어</param>
    public void SaveShipDeployData(PlayerBase targetPlayer)
    {
        shipDeployDatas = new ShipDeployData[targetPlayer.Ships.Length];
        for (int i = 0; i < shipDeployDatas.Length; i++)
        {
            shipDeployDatas[i] = new ShipDeployData();
            shipDeployDatas[i].shipType = targetPlayer.Ships[i].Type;
            shipDeployDatas[i].direction = targetPlayer.Ships[i].Direction;
            shipDeployDatas[i].size = targetPlayer.Ships[i].Size;
            shipDeployDatas[i].position = targetPlayer.Ships[i].Positions[0];
        }
    }

    /// <summary>
    /// 저장된 배 배치정보를 불러오는 함수
    /// </summary>
    /// <param name="targetPlayer">불러올 배치정보를 적용받을 플레이어</param>
    /// <returns>저장된 정보가 있어서 불어왔다면 true, 없어서 읽지 못했다면 false</returns>
    public bool LoadShipDeplyData(PlayerBase targetPlayer)
    {
        bool result = false;
        if (shipDeployDatas != null)
        {
            targetPlayer.UndoAllShipDeployment();               // 기존의 배치 모두 취소

            for (int i = 0; i < shipDeployDatas.Length; i++)
            {
                Ship targetShip = targetPlayer.Ships[i];
                targetShip.Direction = shipDeployDatas[i].direction;
                targetPlayer.Board.ShipDeployment(targetShip, shipDeployDatas[i].position); // 다시 전부 배치
                targetShip.gameObject.SetActive(true);
            }
            result = true;
        }

        return result;
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
