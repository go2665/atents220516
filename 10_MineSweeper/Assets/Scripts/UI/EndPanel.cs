using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;

public class EndPanel : MonoBehaviour
{
    TextMeshProUGUI openTry;
    TextMeshProUGUI find;
    TextMeshProUGUI notFind;

    private void Awake()
    {
        openTry = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        find = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        notFind = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameOver += Refresh;
        gameManager.onGameClear += Refresh;
    }

    private void SetOpenTryCount(int count)
    {
        openTry.text = $"클릭 : {count}회";
    }

    private void SetMineCount(int findCount, int notFindCount)
    {
        find.text = $"발견한 지뢰 : {findCount} 개";
        notFind.text = $"발견못한 지뢰 : {notFindCount}개";
    }

    private void Refresh()
    {
        GameManager gameManager = GameManager.Inst;
        Stage stage = gameManager.Stage;
        SetOpenTryCount(stage.OpenTryCount);
        SetMineCount(stage.FoundMineCount, stage.NotFoundMineCount);
    }
}
