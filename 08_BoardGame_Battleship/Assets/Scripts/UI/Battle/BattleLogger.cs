using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleLogger : MonoBehaviour
{
    /// <summary>
    /// 유저 플레이어 이름 출력할 때 사용할 색상
    /// </summary>
    public Color userColor;

    /// <summary>
    /// 적 플레이어 이름 출력할 때 사용할 색상
    /// </summary>
    public Color enemyColor;

    /// <summary>
    /// 배 이름 출력할 때 사용할 색상
    /// </summary>
    public Color shipColor;

    /// <summary>
    /// 턴 숫자 출력할 때 사용할 색상
    /// </summary>
    public Color turnColor;

    /// <summary>
    /// 글자가 출력될 텍스트 메시 프로
    /// </summary>
    TextMeshProUGUI logText;

    private void Awake()
    {
        // 컴포넌트 찾기
        logText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        // 턴 시작할 때 턴 번호 출력하기 위해 델리게이트에 함수 등록
        TurnManager turnManager = TurnManager.Inst;
        turnManager.onTurnStart += Log_Turn_Start;

        // 배가 공격 당하고 침몰 할 때 상황을 출력하기 위해서 델리게이트에 함수 등록
        GameManager gameManager = GameManager.Inst;
        foreach (var ship in gameManager.UserPlayer.Ships)
        {
            ship.onHit += Log_Attack_Success;
            ship.onSinking += Log_Ship_Destroy;
        }
        foreach (var ship in gameManager.EnemyPlayer.Ships)
        {
            ship.onHit += Log_Attack_Success;
            ship.onSinking += Log_Ship_Destroy;
        }

        // 플레이어가 공격을 실패했을 때 상황을 출력하기 위해서 델리게이트에 함수 등록
        gameManager.UserPlayer.onAttackFail += Log_Attack_Fail;
        gameManager.EnemyPlayer.onAttackFail += Log_Attack_Fail;
        //gameManager.UserPlayer.onAttackFail += () => Log_Attack_Fail(gameManager.UserPlayer);
    }

    /// <summary>
    /// 로거에 입력받은 글자를 출력하는 함수.
    /// </summary>
    /// <param name="text">출력할 글자</param>
    public void Log(string text)
    {
        logText.text = text;
    }

    /// <summary>
    /// 공격이 성공했을 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="ship">공격을 당한 배</param>
    private void Log_Attack_Success(Ship ship)
    {
        //"{Enemy}의 {배종류}에 포탄이 명중했습니다."        
        
        string hexColor = ColorUtility.ToHtmlStringRGB(shipColor);  // shipColor를 16진수 표시양식으로 변경
        string playerColor; // 배의 소유주가 UserPlayer면 userColor, EnemyPlayer면 enemyColor로 출력하기
        string playerName;
        if ( ship.Owner is UserPlayer )     // 배의 소유주가 UserPlayer 인지 아닌지 확인
        {
            playerColor = ColorUtility.ToHtmlStringRGB(userColor);  // 색상 지정
            playerName = "당신";            // 이름 지정
        }
        else
        {
            playerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            playerName = "적";
        }

        Log($"<#{playerColor}>{playerName}</color>의 <#{hexColor}>{ship.Name}</color>에 포탄이 명중했습니다.");
    }

    /// <summary>
    /// 공격이 실패했을 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="attacker"></param>
    private void Log_Attack_Fail(PlayerBase attacker)
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

    /// <summary>
    /// 배가 침몰 당하는 상황을 출력하는 함수
    /// </summary>
    /// <param name="ship">침몰한 배</param>
    private void Log_Ship_Destroy(Ship ship)
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

    /// <summary>
    /// 턴을 시작할 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="number">현재 턴 수</param>
    private void Log_Turn_Start(int number)
    {
        //"{턴숫자} 번째 턴이 시작했습니다."
        string color = ColorUtility.ToHtmlStringRGB(turnColor);
        Log($"<#{color}>{number}</color> 번째 턴이 시작했습니다.");
    }
}
