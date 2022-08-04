using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipTarget
{
    ItemData_Weapon EquipItem { get; }      // 장비한 아이템(무기)
    bool IsWeaponEquiped { get; }           // 현재 장비중인지

    void EquipWeapon(ItemData_Weapon weaponData);   // 아이템 장비하기
    void UnEquipWeapon();                   // 아이템 해제하기
}
