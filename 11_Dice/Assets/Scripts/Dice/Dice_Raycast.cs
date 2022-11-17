using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Raycast : DiceBase
{
    bool rollStart = false;

    Vector3[] directions;

    protected override void Awake()
    {
        base.Awake();
        directions = new Vector3[6];        
    }

    private void Start()
    {
        directions[0] = transform.forward;
        directions[1] = transform.up;
        directions[2] = -transform.right;
        directions[3] = transform.right;
        directions[4] = -transform.up;
        directions[5] = -transform.forward;

    }

    private void Update()
    {
        for(int i=0;i<6;i++)
        {
            diceResult = 0;
            Ray ray = new Ray(transform.position, transform.rotation * directions[i]);
            if( Physics.Raycast(ray, 20.0f, LayerMask.GetMask("Board")))
            {
                diceResult = 7 - (i + 1);
                Debug.Log($"Result : {diceResult}");
            }
        }
    }

    public override void Roll()
    {
        base.Roll();
        rollStart = true;
    }
}
