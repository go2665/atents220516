using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    // 변수 ---------------------------------------------------------------------------------------
    // 슬롯에 있는 아이템(ItemData)
    ItemData slotItemData;

    // 아이템 갯수(int)
    uint itemCount = 0;

    // 아이템 장비여부
    bool itemEquiped = false;

    // 프로퍼티 ------------------------------------------------------------------------------------

    /// <summary>
    /// 슬롯에 있는 아이템(ItemData)
    /// </summary>
    public ItemData SlotItemData
    {
        get => slotItemData;
        private set
        {
            if (slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();  // 변경이 일어나면 델리게이트 실행(주로 화면 갱신용)
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
            onSlotItemChange?.Invoke();  // 변경이 일어나면 델리게이트 실행(주로 화면 갱신용)
        }
    }

    public bool ItemEquiped
    {
        get => itemEquiped;
        set
        {
            itemEquiped = value;
            onSlotItemChange?.Invoke();
        }
    }

    // 델리게이트 ----------------------------------------------------------------------------------
    /// <summary>
    /// 슬롯에 들어있는 아이템의 종류나 갯수가 변경될 때 실행되는 델리게이트
    /// </summary>
    public System.Action onSlotItemChange;

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
        ItemEquiped = false;
    }

    /// <summary>
    /// 아이템을 사용하는 함수
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 대상(보통 플레이어)</param>
    public void UseSlotItem(GameObject target = null)
    {
        IUsable usable = SlotItemData as IUsable;   // 이 아이템이 사용가능한 아이템인지 확인
        if (usable != null)
        {
            // 아이템이 사용가능하면
            usable.Use(target); // 아이템 사용하고
            DecreaseSlotItem(); // 갯수 하나 감소
        }
    }

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="target">아이템을 장비하는 대상</param>
    public bool EquipSlotItem(GameObject target = null)
    {
        bool result = false;
        IEquipItem equipItem = SlotItemData as IEquipItem;  // 이 슬롯의 아이템이 장비 가능한 아이템인지 확인
        if(equipItem != null)
        {
            // 아이템은 장비가능하다.

            ItemData_Weapon weaponData = SlotItemData as ItemData_Weapon;   // 아이템 데이터 따로 보관
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>(); // 아이템을 장비할 대상이 아이템을 장비할 수 있는지 확인
            if (equipTarget != null)
            {
                // 대상은 특정 슬롯의 아이템을 장비하고 있다. 그리고 아이템이 장비되어 있다.
                if (equipTarget.EquipItemSlot != null )    // 무기를 장비하고 잇는지 확인
                {
                    // 무기를 장비하고 있다.

                    if (equipTarget.EquipItemSlot != this)      // 장비하고 있는 아이템의 슬롯을 클릭했는지 확인
                    {
                        // 다른 슬롯을 장비하고 있다.
                        equipTarget.UnEquipWeapon();            // 일단 무기를 벗는다.
                        equipTarget.EquipWeapon(this);    // 다른 무기를 장비한다.
                        result = true;
                    }
                    else
                    {
                        equipTarget.UnEquipWeapon();            // 같은 무기를 장비한 상황이면 벗기만 한다.
                    }
                }
                else
                {
                    // 무기를 장비하고 있지 않다. => 그냥 장비
                    equipTarget.EquipWeapon(this);
                    result = true;
                }
            }
        }
        return result;
    }


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