using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnemyPlayer : TestBase
{
    public EnemyPlayer enemy;

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        // 배 배치 테스트
        enemy.UndoAllShipDeployment();
        enemy.AutoShipDeployment();
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        //enemy.AutoAttack();
        GameManager.Inst.Test_SetState(GameState.Battle);
    }

    protected override void OnTest3(InputAction.CallbackContext obj)
    {
    }

    private static void Test_ListFind()
    {
        // 리스트에서 없는 숫자 찾을 때 결과 확인
        List<int> list = new List<int>(5);
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Add(4);
        list.Add(5);

        int ret = list.Find((x) => x == 5);
        Debug.Log(ret);
        ret = list.Find((x) => x == 7);
        Debug.Log(ret); // 0이 나옴
        ret = list.Find((x) => x == 0);
        Debug.Log(ret);
    }

    private static void Test_Suffle()
    {
        // 셔플 테스트
        int[] temp = { 1, 2, 3, 4, 5 };
        Utils.Shuffle<int>(temp);
        Debug.Log(temp);

        //List<int> temp2 = new List<int>() { 1, 2, 3, 4, 5 };
        //Utils.Shuffle<int>(temp2.ToArray());    // 이것은 안됨
        //Debug.Log(temp2);
    }
}
