using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipTarget
{
    ItemSlot EquipItemSlot { get; }      // 장비한 아이템(무기)

    void EquipWeapon(ItemSlot weaponSlot);   // 아이템 장비하기
    void UnEquipWeapon();                   // 아이템 해제하기
}
