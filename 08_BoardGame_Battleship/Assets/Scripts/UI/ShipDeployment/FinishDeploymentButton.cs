using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishDeploymentButton : MonoBehaviour
{
    /// <summary>
    /// 함선 배치 버튼이 있는 패널
    /// </summary>
    ShipDeploymentPanel deploymentPanel;

    /// <summary>
    /// 자기 자신의 버튼
    /// </summary>
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        deploymentPanel = FindObjectOfType<ShipDeploymentPanel>();
        deploymentPanel.onDeploymentStateChange += Ready;   // ShipDeploymentPanel에서 함선 배치 상태 변화에 따라 실행시킴

        button.interactable = true; // 빌드 버전에서 정상적으로 수행이 되지 않아 임시조치로 추가
    }

    private void OnClick()
    {
        GameManager.Inst.SaveShipDeployData(GameManager.Inst.UserPlayer);
        //Debug.Log("다음 씬으로 넘어가기");
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// 배의 배치가 상태에 따라 버튼 사용가능/불가능 설정
    /// </summary>
    /// <param name="isReady">true면 버튼 사용가능. fals면 버튼 사용 불가능</param>
    void Ready(bool isReady)
    {
        button.interactable = isReady;
    }
}
