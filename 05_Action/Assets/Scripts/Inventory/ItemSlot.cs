using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    // 변수 ---------------------------------------------------------------------------------------
    // 슬롯에 있는 아이템(ItemData)
    ItemData slotItemData;

    // 아이템 갯수(int)
    //uint itemCount = 0;

    // 프로퍼티 ------------------------------------------------------------------------------------

    /// <summary>
    /// 슬롯에 있는 아이템(ItemData)
    /// </summary>
    public ItemData SlotItemData
    {
        get => slotItemData;
        private set
        {
            if( slotItemData != value )
            {
                slotItemData = value;
                onSlotItemChage?.Invoke();
            }
        }
    }

    // 아이템 갯수(int)

    // 델리게이트 ----------------------------------------------------------------------------------
    public System.Action onSlotItemChage;


    // 함수 ---------------------------------------------------------------------------------------
    
    /// <summary>
    /// 슬롯에 아이템을 설정하는 함수 
    /// </summary>
    /// <param name="itemData">슬롯에 설정할 ItemData</param>
    public void AssignSlotItem(ItemData itemData)
    {
        SlotItemData = itemData;
    }

    /// <summary>
    /// 슬롯을 비우는 함수
    /// </summary>
    public void ClearSlotItem()
    {
        SlotItemData = null;
    }


    // 아이템 갯수를 증가/감소시키는 함수
    // 아이템을 사용하는 함수
    // 아이템을 장비하는 함수

    // 함수(백엔드) --------------------------------------------------------------------------------

    /// <summary>
    /// 슬롯이 비었는지 확인해주는 함수
    /// </summary>
    /// <returns>true면 비어있는 함수</returns>
    public bool IsEmpty()
    {
        return slotItemData == null;
    }
}