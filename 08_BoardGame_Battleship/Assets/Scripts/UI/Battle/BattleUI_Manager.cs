using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Battle 씬의 UI를 관리하는 클래스. Canvas에 들어있다.
public class BattleUI_Manager : Singleton<BattleUI_Manager>
{
    TurnPrinter turnPrinter;    // 턴을 표시하는 UI
    ShipsInfo leftShipsInfo;    // UserPlayer의 함선 현황
    ShipsInfo rightShipsInfo;   // EnemyPlayer의 함선 현황
    BattleLogger battleLogger;  // 현재 진행상황 출력용 텍스트 패널

    public BattleLogger Logger => battleLogger;

    protected override void Initialize()
    {
        // 자식 컴포넌트들 가져오기
        turnPrinter = GetComponentInChildren<TurnPrinter>();
        battleLogger = GetComponentInChildren<BattleLogger>();

        ShipsInfo[] shipsInfo = GetComponentsInChildren<ShipsInfo>();
        if (((RectTransform)(shipsInfo[0].transform)).anchorMin.x < ((RectTransform)(shipsInfo[1].transform)).anchorMin.x)
        {
            leftShipsInfo = shipsInfo[0];
            rightShipsInfo = shipsInfo[1];
        }
        else
        {
            leftShipsInfo = shipsInfo[1];
            rightShipsInfo = shipsInfo[0];
        }
    }

    public void PrintLog(string text)
    {
        battleLogger.Log(text);
    }

}
