using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 함선 배치 버튼 + 토글기능 추가하기 위한 용도
/// </summary>
public class DeploymentToggle : MonoBehaviour
{
    // 외부 데이터 ---------------------------------------------------------------------------------
    /// <summary>
    /// 이 버튼을 눌렀을 때 배치할 배의 종류
    /// </summary>
    public ShipType shipType = ShipType.None;

    /// <summary>
    /// 게임 메니저가 가지고 있는 플레이어
    /// </summary>
    UserPlayer player;
    // --------------------------------------------------------------------------------------------

    // 컴포넌트 들 ---------------------------------------------------------------------------------
    /// <summary>
    /// 이 버튼이 있는 부모 패널
    /// </summary>
    ShipDeploymentPanel panel;

    Image image;        // 버튼에 달려있는 이미지
    Button button;      // 버튼
    // --------------------------------------------------------------------------------------------

    // 버튼 상태 표시용 데이터 ----------------------------------------------------------------------
    /// <summary>
    /// 이 종류의 배가 배치되었는지 or 버튼이 눌러졌는지를 알려주는 변수
    /// true면 배가 배치되었고 버튼이 눌러져 있는 상태
    /// </summary>
    bool isDeployed = false;

    /// <summary>
    /// 버튼이 눌러졌을 때 보일 색상
    /// </summary>
    readonly Color selectedColor = new(1, 1, 1, 0.2f);
    // --------------------------------------------------------------------------------------------

    // 프로퍼티 ------------------------------------------------------------------------------------
    /// <summary>
    /// 이 종류의 배가 배치되었는지 or 버튼이 눌러졌는지를 알려주는 프로퍼티.
    /// 배치됬다고 표시되면 버튼의 색상 변경(현재 투명하게 처리)
    /// </summary>
    public bool IsDeployed
    {
        get => isDeployed;
        private set
        {
            isDeployed = value;
            if(isDeployed)
            {
                image.color = selectedColor;    // 색상 변경
            }
            else
            {
                image.color = Color.white;      // 원상 복구
            }
        }
    }
    // --------------------------------------------------------------------------------------------

    // 델리게이트 ----------------------------------------------------------------------------------    
    // --------------------------------------------------------------------------------------------

    private void Awake()
    {
        // 컴포넌트 찾기
        panel = GetComponentInParent<ShipDeploymentPanel>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);    // 버튼 클릭되었을 때 실행될 함수 등록        
    }

    private void Start()
    {
        player = GameManager.Inst.UserPlayer;   // 게임 메니저에서 플레이어 가져오기
    }

    /// <summary>
    /// 토글 버튼 눌렀을 때 실행될 함수
    /// </summary>
    private void OnClick()
    {
        if(IsDeployed)
        {
            // 함선이 이미 배치되어 있으면 함선 배치를 취소
            player.UndoShipDeploy(shipType);
        }
        else
        {
            // 다른 버튼이 눌러져 있는 상황인지 확인
            if (player.SelectedShipType != ShipType.None)
            {
                // 이미 다른 버튼을 눌러서 선택 중인 상황이기 때문에 이전 버튼 복구
                panel.UnToggleButton(player.SelectedShipType);
            }
            // 함선 배치하기 위해 들고 있는 상태로 만들기
            player.SelectShipToDeploy(shipType);
        }
        IsDeployed = !IsDeployed;   // 버튼 토글 상태 변경
    }

    /// <summary>
    /// 눌러져 있는 버튼을 취소하기(버튼 상태만 변경. 다른 부가작업은 없음)
    /// </summary>
    public void UndoToggle()
    {
        IsDeployed = false; // 버튼 색상 변경
    }

    /// <summary>
    /// 버튼을 선택한(누른) 상태로 변경
    /// </summary>
    public void SetToggleSelect()
    {
        IsDeployed = true;
    }
}
