using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomDeploymentButton : MonoBehaviour
{
    /// <summary>
    /// 함선 배치 버튼이 있는 패널
    /// </summary>
    ShipDeploymentPanel deploymentPanel;

    /// <summary>
    /// 배를 배치하는 플레이어
    /// </summary>
    UserPlayer player;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        deploymentPanel = FindObjectOfType<ShipDeploymentPanel>();
    }

    private void Start()
    {
        player = GameManager.Inst.UserPlayer;
    }

    void OnClick()
    {
        // 배가 전부 배치되어 있는지 확인
        if( deploymentPanel.IsAllDeployed )
        {
            // 전부 배치되어있는 상황이면 전부 배치 취소
            player.UndoAllShipDeployment();
        }

        // 아직 배치되지 않은 배만 자동 배치
        player.AutoShipDeployment(true);

        // 모든 함선 배치 버튼을 눌러진 것으로 표시
        deploymentPanel.SetToggleSelectAll();
    }
}
