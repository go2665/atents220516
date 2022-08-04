using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터를 저장하는 데이터 파일을 만들게 해주는 스크립트
/// </summary>
[CreateAssetMenu(fileName = "New Weapon Item Data", menuName = "Scriptable Object/Item Data - Weapon", order = 5)]
public class ItemData_Weapon : ItemData, IEquipItem
{
    public void EquipItem(IEquipTarget target)
    {
        target.EquipWeapon(this);
    }

    public void ToggleEquipItem(IEquipTarget target)
    {
        if(target.IsWeaponEquiped)
        {
            target.UnEquipWeapon();
        }
        else
        {
            target.EquipWeapon(this);
        }
    }

    public void UnEquipItem(IEquipTarget target)
    {
        target.UnEquipWeapon();
    }
}
