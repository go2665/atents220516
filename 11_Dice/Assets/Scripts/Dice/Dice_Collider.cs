using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Collider : DiceBase
{
    Dice_Collider_Face[] faces;
    bool sendResult = false;

    const float VelocityMinimum = 0.0001f;

    public Action<int> onDiceRollEnd;


    public int DiceResult => (rigid.velocity.sqrMagnitude < VelocityMinimum) ? diceResult : 0;

    protected override void Awake()
    {
        base.Awake();    
        faces = GetComponentsInChildren<Dice_Collider_Face>();
    }

    private void OnEnable()
    {
        foreach(var face in faces)
        {
            face.onFaceTouch += OnFaceTouch;
        }
    }

    private void OnDisable()
    {
        foreach (var face in faces)
        {
            face.onFaceTouch -= OnFaceTouch;
        }
    }

    private void OnFaceTouch(int face)
    {
        diceResult = face;
        //Debug.Log($"Touch : {diceResult}");
    }

    private void Update()
    {
        if (!sendResult && diceResult != 0 && rigid.velocity.sqrMagnitude < VelocityMinimum)
        {
            //Debug.Log($"Result : {diceResult}");
            sendResult = true;
            onDiceRollEnd?.Invoke(diceResult);
        }
    }

    public override void Roll()
    {
        base.Roll();
        diceResult = 0;
        sendResult = false;
    }
}
