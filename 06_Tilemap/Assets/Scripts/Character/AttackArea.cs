using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public System.Action<float> onMonsterKill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Debug.Log("적을 공격!");
            Slime slime = collision.gameObject.GetComponent<Slime>();            
            onMonsterKill?.Invoke(slime.RewardLife);
            slime.Die();
        }
    }
}
