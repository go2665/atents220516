using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle = 0,
    Patrol,
    Chase,
    Attack,
    Dead
}

/// <summary>
/// 아이템 종류별 ID
/// </summary>
public enum ItemIDCode
{
    Coin_Copper = 0,
    Coin_Silver,
    Coin_Gold,
    Egg,
    Bone,
    HealingPotion,
    ManaPotion,
    OneHandSword1,
    OneHandSword2
}