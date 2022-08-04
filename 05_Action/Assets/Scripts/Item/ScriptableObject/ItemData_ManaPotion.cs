using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터를 저장하는 데이터 파일을 만들게 해주는 스크립트
/// </summary>
[CreateAssetMenu(fileName = "New Mana Potion", menuName = "Scriptable Object/Item Data - ManaPotion", order = 4)]
public class ItemData_ManaPotion : ItemData, IUsable
{
    [Header("마나포션 데이터")]
    public float manaPoint = 30.0f;

    public void Use(GameObject target = null)
    {
        IMana mana = target.GetComponent<IMana>();
        if (mana != null)
        {
            mana.MP += manaPoint;
            Debug.Log($"{itemName}을 사용했습니다. MP가 {manaPoint}만큼 회복됩니다. 현재 MP는 {mana.MP}입니다.");
        }
    }
}
