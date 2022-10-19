using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeploymentScene : MonoBehaviour
{
    void Start()
    {
        GameManager.Inst.State = GameState.ShipDeployment;  // 게임 메니저의 게임 상태를 변경        
    }
}
