using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    // 변수 ---------------------------------------------------------------------------------------    
    // ItemSlot[] : 아이템 칸 어러개
    ItemSlot[] slots = null;

    // 상수 ---------------------------------------------------------------------------------------
    // 인벤토리 기본 크기
    const int Default_Inventory_Size = 6;

    // 프로퍼티  -----------------------------------------------------------------------------------
    // 인벤토리의 크기
    public int SlotCount { get => slots.Length; }

    // 함수(주요기능) ------------------------------------------------------------------------------
    public Inventory(int size = Default_Inventory_Size)
    {
        slots = new ItemSlot[size];
        for(int i=0;i<size;i++)
        {
            slots[i] = new ItemSlot();
        }
    }

    /// <summary>
    /// 아이템 추가하기 
    /// </summary>
    /// <param name="id">추가할 아이템의 아이디</param>
    /// <returns>아이템 추가 성공 여부(true면 인벤토리에 아이템이 추가됨)</returns>
    public bool AddItem(uint id)
    {        
        return AddItem(GameManager.Inst.ItemData[id]);
    }

    /// <summary>
    /// 아이템 추가하기 
    /// </summary>
    /// <param name="code">추가할 아이템의 코드</param>
    /// <returns>아이템 추가 성공 여부(true면 인벤토리에 아이템이 추가됨)</returns>
    public bool AddItem(ItemIDCode code)
    {
        return AddItem(GameManager.Inst.ItemData[code]);
    }

    /// <summary>
    /// 아이템 추가하기 
    /// </summary>
    /// <param name="data">추가할 아이템의 아이템 데이터</param>
    /// <returns>아이템 추가 성공 여부(true면 인벤토리에 아이템이 추가됨)</returns>
    public bool AddItem(ItemData data)
    {
        bool result = false;

        Debug.Log($"인벤토리에 {data.itemName}을 추가합니다");
        ItemSlot slot = FindEmptySlot();
        if (slot != null)
        {
            slot.AssignSlotItem(data);
            result = true;
            Debug.Log($"추가에 성공했습니다.");
        }
        else
        {
            // 모든 슬롯에 아이템이 들어있다.(인벤토리가 가득찼다.)
            Debug.Log($"실패 : 인벤토리가 가득찼습니다.");
        }

        return result;
    }

    public bool AddItem(uint id, uint index)
    {
        return AddItem(GameManager.Inst.ItemData[id], index);
    }

    public bool AddItem(ItemIDCode code, uint index)
    {
        return AddItem(GameManager.Inst.ItemData[code], index);
    }

    public bool AddItem(ItemData data, uint index)
    {
        bool result = false;

        Debug.Log($"인벤토리의 {index} 슬롯에  {data.itemName}을 추가합니다");
        ItemSlot slot = slots[index];
        if(slot.IsEmpty())
        {
            slot.AssignSlotItem(data);
            result = true;
            Debug.Log($"추가에 성공했습니다.");
        }
        else
        {
            Debug.Log($"실패 : {index} 슬롯에는 다른 아이템이 들어있습니다.");
        }

        return result;
    }


    // 아이템 버리기(인벤토리 비우기)
    public bool RemoveItem(uint slotIndex)
    {
        bool result = false;

        Debug.Log($"인벤토리에서 {slotIndex} 슬롯을 비웁니다.");
        if (IsValidSlotIndex(slotIndex))        
        {
            ItemSlot slot = slots[slotIndex];
            Debug.Log($"{slot.SlotItemData.itemName}을 삭제합니다.");
            slot.ClearSlotItem();
            Debug.Log($"삭제에 성공했습니다.");
            result = true;
        }
        else
        {
            Debug.Log($"실패 : 잘못된 인덱스입니다.");
        }

        return result;
    }


    // 아이템 이동하기
    // 아이템 나누기
    // 아이템 사용하기
    // 아이템 장비하기
    // 아이템 정렬

    // 함수(백엔드) --------------------------------------------------------------------------------
    // 적절한 빈 슬롯을 찾아주는 함수
    //  비어있는 슬롯 확인하는 함수
    //  보유하고 있는 칸 수에 맞는 인덱스인지 확인하는 변수
    // 특정 종류의 아이템이 들어있는 슬롯을 찾아주는 함수

    private ItemSlot FindEmptySlot()
    {
        ItemSlot result = null;

        foreach(var slot in slots)  // slots를 전부 순회하면서
        {
            if (slot.IsEmpty())     // 빈 슬롯인지 확인
            {
                result = slot;      // 빈 슬롯이면 foreach break하고 리턴
                break;
            }
        }

        return result;
    }

    private bool IsValidSlotIndex(uint index)
    {
        return index < SlotCount;
    }

    public void PrintInventory()
    {
        // 현재 인벤토리 내용을 콘솔창에 출력하는 함수
        // ex) [달걀,달걀,달걀,(빈칸),뼈다귀,뼈다귀]

        string printText = "[";
        for(int i=0; i<SlotCount-1;i++)         // 슬롯이 전체6개일 경우 0~4까지만 일단 추가(5개추가)
        {
            if (slots[i].SlotItemData != null)
            {
                printText += slots[i].SlotItemData.itemName;
            }
            else
            {
                printText += "(빈칸)";
            }
            printText += ",";
        }
        ItemSlot slot = slots[SlotCount - 1];   // 마지막 슬롯만 따로 처리
        if (slot != null)
        {
            printText += $"{slot.SlotItemData.itemName}]";
        }
        else
        {
            printText += "]";
        }
        Debug.Log(printText);
    }
}


// ItemData[]   -> ItemDataManager로 처리
// 아이템 이미지 -> ItemData에 추가될 내용
// 아이템 종류   -> ItemData에 추가될 내용