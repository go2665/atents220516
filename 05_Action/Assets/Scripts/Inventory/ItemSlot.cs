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
    // 슬롯에 있는 아이템(ItemData)
    public ItemData SlotItemData
    {
        get => slotItemData;
    }

    // 아이템 갯수(int)


    // 함수 ---------------------------------------------------------------------------------------
    // 슬롯에 아이템을 설정하는 기능
    public void AssignSlotItem(ItemData itemData)
    {
        slotItemData = itemData;
    }

    // 슬롯을 비우는 기능
    public void ClearSlotItem()
    {
        slotItemData = null;
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