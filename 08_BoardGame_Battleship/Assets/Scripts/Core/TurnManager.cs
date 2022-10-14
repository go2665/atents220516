using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    int turnNumber = 0;
    UserPlayer userPlayer;
    EnemyPlayer enemyPlayer;

    Action onTurnStart;
    Action onTurnEnd;

    /// <summary>
    /// 어번턴에 유저의 행동이 끝났는지 표시하는 플래그
    /// </summary>
    bool isUserEnd = false;

    /// <summary>
    /// 이번턴에 적의 행동이 끝났는지 표시하는 플래스
    /// </summary>
    bool isEnemyEnd = false;

    /// <summary>
    /// 턴이 끝났는지 확인하기 위한 플래그
    /// </summary>
    bool isTurnEnd = true;

    protected override void Initialize()
    {
        userPlayer = FindObjectOfType<UserPlayer>();
        userPlayer.onActionEnd += () =>
        {
            isUserEnd = true;
            if (isEnemyEnd)
            {
                OnTurnEnd();
            }
        };
        enemyPlayer = FindObjectOfType<EnemyPlayer>();
        enemyPlayer.onActionEnd += () =>
        {
            isEnemyEnd = true;
            if (isUserEnd)
            {
                OnTurnEnd();
            }
        };

        // 각종 초기화

        onTurnStart = null; // 씬이 다시 로드되었을 때 누적되는 문제 방지용
        onTurnEnd = null;

        turnNumber = 1;
        isTurnEnd = true;

        onTurnStart += userPlayer.OnTurnStart;
        onTurnStart += enemyPlayer.OnTurnStart;
        onTurnEnd += userPlayer.OnTurnEnd;
        onTurnEnd += enemyPlayer.OnTurnEnd;
    }

    void OnTurnStart()
    {
        Debug.Log($"{turnNumber} 턴 시작");
        isTurnEnd = false;
        isUserEnd = false;
        isEnemyEnd = false;
        onTurnStart?.Invoke();
    }

    void OnTurnEnd()
    {
        onTurnEnd?.Invoke();
        isTurnEnd = true;
        Debug.Log($"{turnNumber} 턴 종료");
        turnNumber++;
    }

    private void Update()
    {
        if( isTurnEnd )
        {
            OnTurnStart();
        }
    }
}
