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
                onSlotItemChage?.Invoke();  // 변경이 일어나면 델리게이트 실행(주로 화면 갱신용)
            }
        }
    }

    /// <summary>
    /// 슬롯에 들어있는 아이템 갯수
    /// </summary>
    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            itemCount = value;
            onSlotItemChage?.Invoke();  // 변경이 일어나면 델리게이트 실행(주로 화면 갱신용)
        }
    }

    // 델리게이트 ----------------------------------------------------------------------------------
    /// <summary>
    /// 슬롯에 들어있는 아이템의 종류나 갯수가 변경될 때 실행되는 델리게이트
    /// </summary>
    public System.Action onSlotItemChage;

    // 함수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 생성자들
    /// </summary>
    public ItemSlot() { }
    public ItemSlot(ItemData data, uint count)
    {
        slotItemData = data;
        itemCount = count;
    }
    public ItemSlot(ItemSlot other)
    {
        slotItemData = other.SlotItemData;
        itemCount = other.ItemCount;
    }

    /// <summary>
    /// 슬롯에 아이템을 설정하는 함수 
    /// </summary>
    /// <param name="itemData">슬롯에 설정할 ItemData</param>
    /// /// <param name="count">슬롯에 설정할 아이템 갯수</param>
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
        int overCount = (int)newCount - (int)SlotItemData.maxStackCount;    // 넘친 갯수 계산
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
        return (uint)overCount; // 넘친 갯수 돌려주기
    }

    /// <summary>
    /// 슬롯에서 아이템 갯수 감소 시키기
    /// </summary>
    /// <param name="count">감소시킬 갯수</param>
    public void DecreaseSlotItem(uint count = 1)
    {
        int newCount = (int)ItemCount - (int)count;
        if( newCount < 1)   // 최종적으로 갯수가 0이되면 완전 비우기
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

    // 아이템을 사용하는 함수
    public void UseSlotItem(GameObject target = null)
    {
        IUsable usable = slotItemData as IUsable;
        if (usable != null)
        {
            usable.Use(target);
            DecreaseSlotItem();            
        }
    }


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