using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Rotation : DiceBase
{
    public Action<int> onRollEnd;

    bool rollEnd = false;

    Quaternion[] quaternions;

    protected override void Awake()
    {
        base.Awake();

        quaternions = new Quaternion[6];
        quaternions[0] = Quaternion.Euler(-90, 0, 0);
        quaternions[1] = Quaternion.Euler(0, 0, 0);
        quaternions[2] = Quaternion.Euler(0, 0, -90);
        quaternions[3] = Quaternion.Euler(0, 0, 90);
        quaternions[4] = Quaternion.Euler(0, 0, 180);
        quaternions[5] = Quaternion.Euler(90, 0, 0);
    }


    private void Update()
    {
        //if ( !rollEnd && rigid.velocity.sqrMagnitude < 0.1f ) 
        {
            RollFinish();
        }
    }

    public override void Roll()
    {
        base.Roll();
        rollEnd = false;
    }

    int GetDiceResult()
    {
        int result = 0;
        float threshold = 0.001f;

        //for(int i=0;i<6;i++)
        //{
        //    if (transform.rotation == quaternions[i])
        //    {
        //        result = i + 1;
        //        break;
        //    }
        //}

        if (transform.rotation.eulerAngles.x > (-90.0f - threshold) && transform.rotation.eulerAngles.x < (-90.0f + threshold)
            && transform.rotation.eulerAngles.z > (0.0f - threshold) && transform.rotation.eulerAngles.z < (0.0f + threshold))
        {
            result = 1;
        }
        else if (transform.rotation.eulerAngles.x > (0.0f - threshold) && transform.rotation.eulerAngles.x < (0.0f + threshold)
            && transform.rotation.eulerAngles.z > (0.0f - threshold) && transform.rotation.eulerAngles.z < (0.0f + threshold))
        {
            result = 2;
        }
        else if (transform.rotation.eulerAngles.x > (0.0f - threshold) && transform.rotation.eulerAngles.x < (0.0f + threshold)
            && transform.rotation.eulerAngles.z > (-90.0f - threshold) && transform.rotation.eulerAngles.z < (-90.0f + threshold))
        {
            result = 3;
        }
        else if (transform.rotation.eulerAngles.x > (0.0f - threshold) && transform.rotation.eulerAngles.x < (0.0f + threshold)
            && transform.rotation.eulerAngles.z > (90.0f - threshold) && transform.rotation.eulerAngles.z < (90.0f + threshold))
        {
            result = 4;
        }
        else if (transform.rotation.eulerAngles.x > (0.0f - threshold) && transform.rotation.eulerAngles.x < (0.0f + threshold)
            && transform.rotation.eulerAngles.z > (180.0f - threshold) && transform.rotation.eulerAngles.z < (180.0f + threshold))
        {
            result = 5;
        }
        else if (transform.rotation.eulerAngles.x > (90.0f - threshold) && transform.rotation.eulerAngles.x < (90.0f + threshold)
            && transform.rotation.eulerAngles.z > (0.0f - threshold) && transform.rotation.eulerAngles.z < (0.0f + threshold))
        {
            result = 6;
        }

        return result;
    }

    void RollFinish()
    {
        int result = GetDiceResult();
        if (result != 0)
        {
            Debug.Log($"Result : {result}");
            rollEnd = true;
            onRollEnd?.Invoke(result);
        }
    }
}
