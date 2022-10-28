using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_BattleScene : TestBase
{
    UserPlayer player;
    EnemyPlayer enemy;

    private void Start()
    {
        player = GameManager.Inst.UserPlayer;
        enemy = GameManager.Inst.EnemyPlayer;
    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        //BattleUI_Manager.Inst.Logger.Log_Attack_Success(player, ShipType.Carrier);
        //BattleUI_Manager.Inst.Logger.Log_Attack_Fail(player);
        //BattleUI_Manager.Inst.Logger.Log_Ship_Destroy(player.Ships[0]);
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        //BattleUI_Manager.Inst.Logger.Log_Attack_Success(enemy, ShipType.Destroyer);
        //BattleUI_Manager.Inst.Logger.Log_Attack_Fail(enemy);
        //BattleUI_Manager.Inst.Logger.Log_Ship_Destroy(enemy.Ships[1]);
    }

    protected override void OnTest3(InputAction.CallbackContext obj)
    {
        //ResultPanel resultPanel = FindObjectOfType<ResultPanel>();
        //resultPanel.userAnalysis.AllAttackCount = 10;
        //resultPanel.userAnalysis.SuccessAttackCount = 7;
        //resultPanel.userAnalysis.FailAttackCount = 3;
        //resultPanel.userAnalysis.SuccessAttackRatio = (float)7 / 10;
        //resultPanel.enemyAnalysis.AllAttackCount = 11;
        //resultPanel.enemyAnalysis.SuccessAttackCount = 8;
        //resultPanel.enemyAnalysis.FailAttackCount = 3;
        //resultPanel.enemyAnalysis.SuccessAttackRatio = (float)8 / 11;
    }
}
