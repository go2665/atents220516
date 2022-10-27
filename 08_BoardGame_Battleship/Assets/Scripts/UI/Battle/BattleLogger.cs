using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

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

    /// <summary>
    /// 자주 사용하는 텍스트 재사용용 상수
    /// </summary>
    const string YOU = "당신";
    const string ENEMY = "적";

    /// <summary>
    /// 최대 표현 가능한 줄 수
    /// </summary>
    const int MaxLineCount = 20;

    /// <summary>
    /// 로거에서 출력할 문자열들을 가지고 있는 리스트
    /// </summary>
    List<string> logLines;

    /// <summary>
    /// 문자열 조합용 스트링빌더
    /// </summary>
    StringBuilder builder;

    private void Awake()
    {
        // 컴포넌트 찾기
        logText = GetComponentInChildren<TextMeshProUGUI>();
        logLines = new List<string>(MaxLineCount + 5);  // capacity를 5개 여유있게 확보
        builder = new StringBuilder(logLines.Capacity); // StringBuilder의 크기를 logLines의 capacity 만큼 확보
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
            ship.onHit += (targetShip) => { Log_Attack_Success(false, targetShip); };   // 람다식을 이용해서 파라메터 갯수가 달라도 실행되게 설정
            ship.onSinking = (targetShip) => { Log_Ship_Destroy(false, targetShip); } + ship.onSinking;
        }
        foreach (var ship in gameManager.EnemyPlayer.Ships)
        {
            ship.onHit += (targetShip) => { Log_Attack_Success(true, targetShip); };
            ship.onSinking = (targetShip) => { Log_Ship_Destroy(true, targetShip); } + ship.onSinking;
        }

        // 플레이어가 공격을 실패했을 때 상황을 출력하기 위해서 델리게이트에 함수 등록
        gameManager.UserPlayer.onAttackFail += Log_Attack_Fail;
        gameManager.EnemyPlayer.onAttackFail += Log_Attack_Fail;

        // 플레이어의 모든 배가 파괴 되었을 때의 상황을 출력하기 위해서 델리게이트에 함수 등록
        gameManager.UserPlayer.onDefeat += Log_Defeat;
        gameManager.EnemyPlayer.onDefeat += Log_Defeat;

        Clear();  // 시작할 때 로거 비우기
    }

    /// <summary>
    /// 로거에 입력받은 글자를 출력하는 함수.
    /// </summary>
    /// <param name="text">출력할 글자</param>
    public void Log(string text)
    {
        logLines.Add(text);                 // 입력 받은 문자열을 리스트에 추가
        if(logLines.Count > MaxLineCount)   // 최대 줄 수가 넘어갔을 경우
        {
            logLines.RemoveAt(0);           // 첫번째 줄 삭제
        }
        
        builder.Clear();                    // 문자열 조합용 빌더 초기화
        foreach (var line in logLines)
        {
            builder.AppendLine(line);       // 빌더에 문자열 추가
        }

        logText.text = builder.ToString();  // 빌더에서 합친 문자열을 Text에 넣기
    }

    /// <summary>
    /// 로거의 내용을 다 지우는 함수
    /// </summary>
    public void Clear()
    {
        logLines.Clear();   // 기록된 스트링 초기화
        logText.text = "";  // 표시되어있는 텍스트 지우기
    }


    /// <summary>
    /// 공격이 성공했을 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="isPlayerAttack">true면 플레이어 공격, false면 적의 공격</param>
    /// <param name="ship">공격을 당한 배</param>        
    private void Log_Attack_Success(bool isPlayerAttack, Ship ship)
    {
        //"{Enemy}의 {배종류}에 포탄이 명중했습니다."        
        
        string attackerColor;   // 공격자 색깔
        string attackerName;    // 공격자 이름
        if( isPlayerAttack )
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            attackerName = YOU;
        }
        else
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            attackerName = ENEMY;
        }

        string shipColor = ColorUtility.ToHtmlStringRGB(this.shipColor);  // shipColor를 16진수 표시양식으로 변경
        string playerColor; // 배의 소유주가 UserPlayer면 userColor, EnemyPlayer면 enemyColor로 출력하기
        string playerName;
        if ( ship.Owner is UserPlayer )     // 배의 소유주가 UserPlayer 인지 아닌지 확인
        {
            playerColor = ColorUtility.ToHtmlStringRGB(userColor);  // 색상 지정
            playerName = YOU;            // 이름 지정
        }
        else
        {
            playerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            playerName = ENEMY;
        }

        Log($"<#{attackerColor}>{attackerName}의 공격</color>\t: <#{playerColor}>{playerName}</color>의 <#{shipColor}>{ship.Name}</color>에 포탄이 명중했습니다.");
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
            playerName = YOU;
        }
        else
        {
            playerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            playerName = ENEMY;
        }
        Log($"<#{playerColor}>{playerName}의 공격</color>\t: <#{playerColor}>{playerName}</color>의 포탄이 빗나갔습니다.");
    }

    /// <summary>
    /// 배가 침몰 당하는 상황을 출력하는 함수
    /// </summary>
    /// <param name="isPlayerAttack">true면 플레이어 공격, false면 적의 공격</param>
    /// <param name="ship">침몰한 배</param>
    private void Log_Ship_Destroy(bool isPlayerAttack, Ship ship)
    {
        //"Enemy의 {배종류}를 침몰 시켰습니다."
        //"당신의 {배종류}가 침몰 했습니다."

        string attackerColor;   // 공격자 색상
        string attackerName;    // 공격자 이름
        if (isPlayerAttack)
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            attackerName = YOU;
        }
        else
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            attackerName = ENEMY;
        }

        string hexColor = ColorUtility.ToHtmlStringRGB(shipColor);  // shipColor를 16진수 표시양식으로 변경
        string playerColor; //배의 소유자가 UserPlayer면 userColor, EnemyPlayer면 enemyColor로 출력하기
        string playerName;
        if (ship.Owner is UserPlayer) // Owner이 UserPlayer 인지 아닌지 확인
        {
            playerColor = ColorUtility.ToHtmlStringRGB(userColor);
            playerName = YOU;
        }
        else
        {
            playerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            playerName = ENEMY;
        }

        Log($"<#{attackerColor}>{attackerName}의 공격</color>\t: <#{playerColor}>{playerName}</color>의 <#{hexColor}>{ship.Name}</color>이 침몰했습니다.");
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


    /// <summary>
    /// 게임이 끝날 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="player">패배한 플레이어</param>
    private void Log_Defeat(PlayerBase player)
    {
        if( player is UserPlayer )
        {
            // 사람이 졌다.
            Log($"당신의 <#ff0000>패배</color>입니다.");
        }
        else
        {
            // 컴퓨터가 졌다.
            Log($"당신의 <#00ff00>승리</color>입니다.");
        }
    }
}
