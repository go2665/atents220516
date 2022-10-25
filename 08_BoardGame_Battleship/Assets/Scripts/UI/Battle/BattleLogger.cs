using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleLogger : MonoBehaviour
{
    public Color userColor;
    public Color enemyColor;
    public Color shipColor;
    public Color turnColor;

    TextMeshProUGUI logText;

    private void Awake()
    {
        logText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        TurnManager turnManager = TurnManager.Inst;
        turnManager.onTurnStart += Log_Turn_Start;
    }


    public void Log(string text)
    {
        logText.text = text;
    }

    public void Log_Attack_Success(Ship ship)
    {
        //"{Enemy}의 {배종류}에 포탄이 명중했습니다."        
        
        string hexColor = ColorUtility.ToHtmlStringRGB(shipColor);  // shipColor를 16진수 표시양식으로 변경
        string playerColor; // 배의 소유주가 UserPlayer면 userColor, EnemyPlayer면 enemyColor로 출력하기
        string playerName;
        if ( ship.Owner is UserPlayer ) // 배의 소유주가 UserPlayer 인지 아닌지 확인
        {
            playerColor = ColorUtility.ToHtmlStringRGB(userColor);
            playerName = "당신";
        }
        else
        {
            playerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            playerName = "적";
        }

        Log($"<#{playerColor}>{playerName}</color>의 <#{hexColor}>{ship.Name}</color>에 포탄이 명중했습니다.");
    }

    public void Log_Attack_Fail(PlayerBase attacker)
    {
        //"{당신}의 포탄이 빗나갔습니다."
        //"{적}의 포탄이 빗나갔습니다."
        string playerColor; //victim이 UserPlayer면 userColor, EnemyPlayer면 enemyColor로 출력하기
        string playerName;
        if (attacker is UserPlayer) // victim이 UserPlayer 인지 아닌지 확인
        {
            playerColor = ColorUtility.ToHtmlStringRGB(userColor);
            playerName = "당신";
        }
        else
        {
            playerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            playerName = "적";
        }
        Log($"<#{playerColor}>{playerName}</color>의 포탄이 빗나갔습니다.");
    }

    public void Log_Ship_Destroy(Ship ship)
    {
        //"Enemy의 {배종류}를 침몰 시켰습니다."
        //"당신의 {배종류}가 침몰 했습니다."

        string hexColor = ColorUtility.ToHtmlStringRGB(shipColor);  // shipColor를 16진수 표시양식으로 변경
        string playerColor; //배의 소유자가 UserPlayer면 userColor, EnemyPlayer면 enemyColor로 출력하기
        string playerName;
        if (ship.Owner is UserPlayer) // Owner이 UserPlayer 인지 아닌지 확인
        {
            playerColor = ColorUtility.ToHtmlStringRGB(userColor);
            playerName = "당신";
        }
        else
        {
            playerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            playerName = "적";
        }

        Log($"<#{playerColor}>{playerName}</color>의 <#{hexColor}>{ship.Name}</color>이 침몰했습니다.");
    }     

    private void Log_Turn_Start(int number)
    {
        //"{턴숫자} 번째 턴이 시작했습니다."
        string color = ColorUtility.ToHtmlStringRGB(turnColor);
        Log($"<#{color}>{number}</color> 번째 턴이 시작했습니다.");
    }
}
