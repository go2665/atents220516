using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeploymentPanel : MonoBehaviour
{
    /// <summary>
    /// 이 패널이 가지고 있는 토글버튼들
    /// </summary>
    DeploymentToggle[] toggles;

    private void Awake()
    {
        toggles = GetComponentsInChildren<DeploymentToggle>();  // 자식으로 붙어있는 토글버튼 가져오기
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
}
