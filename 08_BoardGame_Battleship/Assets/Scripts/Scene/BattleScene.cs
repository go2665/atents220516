using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    private void Start()
    {
        UserPlayer player = GameManager.Inst.UserPlayer;
        if( !GameManager.Inst.LoadShipDeplyData(player) )   // 저장된 배 배치정보가 없으면 
        {
            player.AutoShipDeployment(true);                // 자동 배치
        }

        GameManager.Inst.State = GameState.Battle;          // 상태 설정
    }
}
