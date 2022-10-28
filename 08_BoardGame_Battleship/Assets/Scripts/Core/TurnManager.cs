using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    // 턴 정보 -------------------------------------------------------------------------------------
    /// <summary>
    /// 현재 턴의 수(몇턴째인지)
    /// </summary>
    int turnNumber = 0;

    /// <summary>
    /// 턴이 끝났는지 확인하기 위한 플래그
    /// </summary>
    bool isTurnEnd = true;
    // --------------------------------------------------------------------------------------------


    // 플레이어 정보 -------------------------------------------------------------------------------
    /// <summary>
    /// 유저측 플레이어(실제 사람)
    /// </summary>
    UserPlayer userPlayer;

    /// <summary>
    /// 컴퓨터가 조정하는 적 플레이어
    /// </summary>
    EnemyPlayer enemyPlayer;
    
    /// <summary>
    /// 이번턴에 유저의 행동이 끝났는지 표시하는 플래그
    /// </summary>
    bool isUserEnd = false;

    /// <summary>
    /// 이번턴에 적의 행동이 끝났는지 표시하는 플래스
    /// </summary>
    bool isEnemyEnd = false;
    // --------------------------------------------------------------------------------------------
    

    // 델리게이트 ----------------------------------------------------------------------------------
    /// <summary>
    /// 턴이 시작될 때 실행될 델리게이트
    /// </summary>
    public Action<int> onTurnStart;

    /// <summary>
    /// 턴이 끝날 때 실행될 델리게이트
    /// </summary>
    Action onTurnEnd;
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// 씬이 로드 완료되었을 때 실행되는 초기화 함수
    /// </summary>
    protected override void Initialize()
    {
        // 각종 초기화(씬이 로드 되었을 때 초기화 해야 할 것들을 초기화)
        turnNumber = 1;     // 턴 번호 초기화
        isTurnEnd = true;   // 턴 종료 플래그 초기화

        onTurnStart = null; // 씬이 다시 로드되었을 때 누적되는 문제 방지용
        onTurnEnd = null;

        // 델리게이트 연결
        userPlayer = FindObjectOfType<UserPlayer>();    // 유저 플레이어 찾고
        enemyPlayer = FindObjectOfType<EnemyPlayer>();  // 적 플레이어 찾고
                
        userPlayer.onActionEnd += () =>                 // 공격이 끝났을 때 실행될 델리게이트에 함수 연결
        {
            isUserEnd = true;   // 자기 행동 끝났음을 표시
            if (!enemyPlayer.IsDepeat && isEnemyEnd)     // 적이 아직 지지 않았고 적의 행동이 끝났는지 확인
            {
                OnTurnEnd();    // 둘 다 행동이 끝났으면 턴 종료
            }
        };
        onTurnStart += userPlayer.OnPlayerTurnStart;  // 플레이어의 턴 시작과 종료시 해야할 함수 등록
        onTurnEnd += userPlayer.OnPlayerTurnEnd;
               
        if (enemyPlayer != null)
        {
            enemyPlayer.onActionEnd += () =>                // 적의 공격이 끝났을 때 실행될 델리게이트에 함수 연결
            {
                isEnemyEnd = true;  // 적의 행동이 끝났음을 표시
                if (!userPlayer.IsDepeat && isUserEnd)      // 유저가 아직 지지 않았고 유저의 행동이 끝났는지 확인
                {
                    OnTurnEnd();    // 둘 다 행동이 끝났으면 턴 종료
                }
            };            
            onTurnStart += enemyPlayer.OnPlayerTurnStart;   // 플레이어의 턴 시작과 종료시 해야할 함수 등록
            onTurnEnd += enemyPlayer.OnPlayerTurnEnd;
        }
    }

    /// <summary>
    /// 턴 매니저가 턴을 시작할 때 실행되는 함수
    /// </summary>
    void OnTurnStart()
    {
        Debug.Log($"{turnNumber} 턴 시작");
        isTurnEnd = false;      // 턴이 시작되었으니 isTurnEnd를 false로 변경
        isUserEnd = false;      // 플레이어들도 턴 종료 표시를 false로 변경
        isEnemyEnd = false;
        onTurnStart?.Invoke(turnNumber);  // 델리게이트를 실행시켜서 등록되어있는 플레이어의 OnPlayerTurnStart도 실행 시킴
    }

    /// <summary>
    /// 턴 매니저가 턴이 종료될 때 실행되는 함수.(모든 플레이어가 동작을 다하면 실행)
    /// </summary>
    void OnTurnEnd()
    {
        onTurnEnd?.Invoke();    // 델리게이트를 실행 시켜서 등록되어 있는 플레이어의 OnPlayerTurnEnd도 실행
        isTurnEnd = true;       // 턴 종료 표시
        Debug.Log($"{turnNumber} 턴 종료");
        turnNumber++;           // 턴 수 증가
    }

    private void Update()
    {
        if( isTurnEnd )         // 현재 프레임에 턴이 종료된 상태면
        {
            OnTurnStart();      // 새 턴을 시작
        }
    }
}
