using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipTarget
{
    ItemData_Weapon EquipItem { get; }
    bool IsWeaponEquiped { get; }

    void EquipWeapon(ItemData_Weapon weapon);
    void UnEquipWeapon();    
}
