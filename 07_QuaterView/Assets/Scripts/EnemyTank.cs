using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : MonoBehaviour, IHit
{
    float hp = 0.0f;
    float maxHP = 100.0f;
    bool isDead = false;

    public float HP 
    { 
        get => hp; 
        set
        {
            hp = value;
            if (hp < 0)
            {
                hp = 0;
                if(!isDead)
                    Dead();
            }
            hp = Mathf.Min(hp, maxHP);
        }
    }

    public float MaxHP { get => maxHP; }

    public Action onHealthChange { get; set; }
    public Action onDead { get; set; }

    public void Dead()
    {
        isDead = true;

        Debug.Log("사망");
    }

    private void Start()
    {
        hp = maxHP;
    }
}
