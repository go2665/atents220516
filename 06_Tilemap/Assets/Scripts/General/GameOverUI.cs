using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 오버 표시용 클래스
/// </summary>
public class GameOverUI : MonoBehaviour
{
    CanvasGroup canvasGroup;            
    TextMeshProUGUI totalLifeTimeText;  // 전체 플레이 시간 출력용 텍스트
    Button restartButton;               // 재시작 버튼

    private void Awake()
    {
        // 컴포넌트 찾고 함수 연결하기
        canvasGroup = GetComponent<CanvasGroup>();
        totalLifeTimeText = transform.Find("TotalLifeTimeText").GetComponent<TextMeshProUGUI>();
        restartButton = GetComponentInChildren<Button>();
        restartButton.onClick.AddListener(Restart);
    }

    /// <summary>
    /// 재시작 버튼용 함수
    /// </summary>
    private void Restart()
    {
        // 재시작하면 무조건 로딩씬 열기
        SceneManager.LoadSceneAsync("LoadingScene");
    }

    /// <summary>
    /// 전체 플레이 수명 글자 변경용 함수
    /// </summary>
    /// <param name="total">전체 플레이 시간</param>
    public void SetTotoalLifeTime(float total)
    {
        totalLifeTimeText.text = $"Total life time : {total:f1} sec";
    }

    /// <summary>
    /// 캔버스 그룹 이용해서 알파값 조절하는 함수
    /// </summary>
    /// <param name="isShow">true면 게임오버 UI 띄우고 flase면 안보여 준다.</param>
    public void Show(bool isShow)
    {
        if(isShow)
        {
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
