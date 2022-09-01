using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI totalLifeTimeText;
    Button restartButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        totalLifeTimeText = transform.Find("TotalLifeTimeText").GetComponent<TextMeshProUGUI>();
        restartButton = GetComponentInChildren<Button>();
        restartButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        SceneManager.LoadSceneAsync("LoadingScene");
    }

    public void SetTotoalLifeTime(float total)
    {
        totalLifeTimeText.text = $"Total life time : {total:f1} sec";
    }

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
