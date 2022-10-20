using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeploymentPanel : MonoBehaviour
{
    /// <summary>
    /// 배를 배치하는 플레이어
    /// </summary>
    UserPlayer player;

    /// <summary>
    /// 이 패널이 가지고 있는 토글버튼들
    /// </summary>
    DeploymentToggle[] toggles;

    /// <summary>
    /// 함선 배치 상태
    /// </summary>
    bool[] shipDeployStates;

    /// <summary>
    /// 함선 배치 상태가 변경되면 실행되는 델리게이트
    /// </summary>
    public Action<bool> onDeploymentStateChange;

    /// <summary>
    /// 모든 함선이 배치되었는지 알려주는 프로퍼티
    /// </summary>
    public bool IsAllDeployed
    {
        get
        {
            foreach (var deploy in shipDeployStates)    // 배치 상태 모두 확인해서 처리
            {
                if(!deploy)
                {
                    return false;
                }
            }
            return true;
        }
    }

    private void Awake()
    {        
        toggles = GetComponentsInChildren<DeploymentToggle>();  // 자식으로 붙어있는 토글버튼 가져오기            
    }

    private void Start()
    {
        player = GameManager.Inst.UserPlayer;
        int shipCount = ShipManager.Inst.ShipTypeCount;

        shipDeployStates = new bool[shipCount];
        for (int i = 0; i < shipCount; i++)
        {
            int targetIndex = i;
            // 함선이 배치되거나 배치 취소될때 실행되는 델리게이트에 람다 함수 등록
            player.Ships[i].onDeploy += (x) =>
            {
                shipDeployStates[targetIndex] = x;                  // 함선 배치 상태표시용 변수 변경
                onDeploymentStateChange?.Invoke(IsAllDeployed);     // 변경이 있었다고 델리게이트 실행
            };
        }
    }

    /// <summary>
    /// 특정버튼을 선택 취소하는 함수
    /// </summary>
    /// <param name="type">변경할 버튼에 할당된 배 타입</param>
    public void UnToggleButton(ShipType type)
    {
        int index = (int)type - 1;
        toggles[index].UndoToggle();    // 토글버튼에 있는 UndoToggle 함수를 호출하여 선택 취소
    }

    /// <summary>
    /// 모든 버튼을 눌러진 상태로 변경하는 함수
    /// </summary>
    public void SetToggleSelectAll()
    {
        foreach(var toggle in toggles)
        {
            toggle.SetToggleSelect();
        }

    }
}
