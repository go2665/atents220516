using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터를 저장하는 데이터 파일을 만들게 해주는 스크립트
/// </summary>
[CreateAssetMenu(fileName = "New Weapon Item Data", menuName = "Scriptable Object/Item Data - Weapon", order = 5)]
public class ItemData_Weapon : ItemData, IEquipItem
{
    [Header("무기 데이터")]
    public float attackPower = 10.0f;
    public float attackSpeed = 1.0f;

    /// <summary>
    /// 아이템 장비
    /// </summary>
    /// <param name="target">아이템을 장비할 대상</param>
    public void EquipItem(IEquipTarget target)
    {
        //target.EquipWeapon(this);
    }

    /// <summary>
    /// 아이템 장비/해제 토글
    /// </summary>
    /// <param name="target">아이템을 토글할 대상</param>
    public void ToggleEquipItem(IEquipTarget target)
    {
        //if(target.IsWeaponEquiped)
        //{
        //    target.UnEquipWeapon();     // 장비되어있으면 해제하고
        //}
        //else
        //{
        //    target.EquipWeapon(this);   // 장비안되어있으면 장비
        //}
    }

    /// <summary>
    /// 아이템 해제
    /// </summary>
    /// <param name="target">아이템을 해제할 대상</param>
    public void UnEquipItem(IEquipTarget target)
    {
        //target.UnEquipWeapon();
    }
}
