using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_BattleScene : TestBase
{
    UserPlayer player;

    private void Start()
    {
        player = GameManager.Inst.UserPlayer;
    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        BattleUI_Manager.Inst.Logger.Log_Attack_Success(player, ShipType.Carrier);
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        BattleUI_Manager.Inst.PrintLog("Hello Unity");
    }

    protected override void OnTest3(InputAction.CallbackContext obj)
    {
        base.OnTest3(obj);
    }
}
