using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHit
{
    float HP { get; set; }
    float MaxHP { get; }

    Action<float> onHealthChange { get; set; }
    Action onDead { get; set; }
    
    void TakeDamage(float damage);

    void Dead();
}
