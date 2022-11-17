using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Dice : TestBase
{
    public DiceBase dice;

    private void Start()
    {
        dice = FindObjectOfType<DiceBase>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        dice.Roll();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Dice_Collider diceCollider = dice as Dice_Collider;
        Debug.Log(diceCollider.DiceResult);
    }
}
