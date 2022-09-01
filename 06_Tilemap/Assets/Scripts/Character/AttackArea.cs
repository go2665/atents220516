using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    // 몬스터가 죽을 때 실행될 델리게이트
    public System.Action<float> onMonsterKill;  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            // 애니메이션으로 트리거가 켜져서 적이 범위 안에 들어왔을 때
            //Debug.Log("적을 공격!");
            Slime slime = collision.gameObject.GetComponent<Slime>();            
            slime.Die();    // 슬라임 죽이기
            onMonsterKill?.Invoke(slime.RewardLife);    // 보상 수명을 델리게이트에 넘겨주기
        }
    }
}
