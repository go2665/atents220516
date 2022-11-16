using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;

public class EndPanel : MonoBehaviour
{
    TextMeshProUGUI openTry;    // 클릭횟수
    TextMeshProUGUI find;       // 찾은 지뢰수
    TextMeshProUGUI notFind;    // 못찾은 지뢰수
    CanvasGroup canvasGroup;    // on/off용 캔버스 그룹

    private void Awake()
    {
        // 컴포넌트 찾기
        canvasGroup = GetComponent<CanvasGroup>();
        openTry = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        find = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        notFind = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0;                      // 시작하면 안보이게 만들기
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameOver += Refresh;          // 게임 오버나 게임 클리어 시 화면 갱신
        gameManager.onGameClear += Refresh;
    }

    /// <summary>
    /// 출력할 클릭 횟수 설정하는 함수
    /// </summary>
    /// <param name="count">클릭 회수</param>
    private void SetOpenTryCount(int count)
    {
        openTry.text = $"클릭 : {count}회";
    }

    /// <summary>
    /// 출력할 지뢰갯수 설정하는 함수
    /// </summary>
    /// <param name="findCount">찾은 지뢰 갯수</param>
    /// <param name="notFindCount">못찾은 지뢰 갯수</param>
    private void SetMineCount(int findCount, int notFindCount)
    {
        find.text = $"발견한 지뢰 : {findCount} 개";
        notFind.text = $"발견못한 지뢰 : {notFindCount}개";
    }

    /// <summary>
    /// 화면 갱신용 함수
    /// </summary>
    private void Refresh()
    {
        GameManager gameManager = GameManager.Inst;
        Stage stage = gameManager.Stage;                // 스테이지를 참조해서 값을 가져옴
        SetOpenTryCount(stage.OpenTryCount);
        SetMineCount(stage.FoundMineCount, stage.NotFoundMineCount);
        canvasGroup.alpha = 1;                          // 화면에 보이게 만들기
    }
}
