using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBase : MonoBehaviour
{
    public float rollPower = 15.0f;
    public float maxSpinPower = 20.0f;

    protected int diceResult = 0;

    protected Rigidbody rigid;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public virtual void Roll()
    {
        rigid.AddForce(Vector3.up * rollPower, ForceMode.Impulse);
        rigid.AddTorque(
            Random.Range(-maxSpinPower, maxSpinPower), 
            Random.Range(-maxSpinPower, maxSpinPower), 
            Random.Range(-maxSpinPower, maxSpinPower), ForceMode.Impulse);
    }
}
