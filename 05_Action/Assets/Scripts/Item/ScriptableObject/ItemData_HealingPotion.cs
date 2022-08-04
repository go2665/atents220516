using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터를 저장하는 데이터 파일을 만들게 해주는 스크립트
/// </summary>
[CreateAssetMenu(fileName = "New Healing Potion", menuName = "Scriptable Object/Item Data - HealingPotion", order = 3)]
public class ItemData_HealingPotion : ItemData, IUsable
{
    [Header("힐링포션 데이터")]
    public float healPoint = 20.0f;

    public void Use(GameObject target = null)
    {
        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
            health.HP += healPoint;
            Debug.Log($"{itemName}을 사용했습니다. HP가 {healPoint}만큼 회복됩니다. 현재 HP는 {health.HP}입니다.");
        }
    }
}
