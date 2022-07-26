using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    // 변수 ---------------------------------------------------------------------------------------
    // 슬롯에 있는 아이템(ItemData)
    ItemData slotItemData;

    // 아이템 갯수(int)
    uint itemCount = 0;

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

    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            itemCount = value;
            onSlotItemChage?.Invoke();
        }
    }

    // 델리게이트 ----------------------------------------------------------------------------------

    // 아이템 갯수(int)

    // 델리게이트 ----------------------------------------------------------------------------------
    public System.Action onSlotItemChage;

    // 함수 ---------------------------------------------------------------------------------------
    
    /// <summary>
    /// 슬롯에 아이템을 설정하는 함수 
    /// </summary>
    /// <param name="itemData">슬롯에 설정할 ItemData</param>
    public void AssignSlotItem(ItemData itemData, uint count = 1)
    {
        ItemCount = count;
        SlotItemData = itemData;
    }

    /// <summary>
    /// 같은 종류의 아이템이 추가되 아이템 갯수가 증가하는 상황에 사용
    /// </summary>
    /// <param name="count">증가시킬 갯수</param>
    /// <returns>최대치를 넘어선 갯수. 0이면 다 증가시킨 상황</returns>
    public uint IncreaseSlotItem(uint count = 1)
    {
        uint newCount = ItemCount + count;
        int overCount = (int)newCount - (int)SlotItemData.maxStackCount;
        if(overCount > 0)
        {
            // 넘쳤다.
            ItemCount = SlotItemData.maxStackCount;
        }
        else
        {
            // 충분히 추가 가능하다.
            ItemCount = newCount;
            overCount = 0;
        }
        return (uint)overCount;
    }

    public void DecreaseSlotItem(uint count = 1)
    {
        int newCount = (int)ItemCount - (int)count;
        if( newCount < 1)
        {
            // 다 뺀다.
            ClearSlotItem();
        }
        else
        {
            ItemCount = (uint)newCount;
        }
    }

    /// <summary>
    /// 슬롯을 비우는 함수
    /// </summary>
    public void ClearSlotItem()
    {
        SlotItemData = null;
        ItemCount = 0;
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