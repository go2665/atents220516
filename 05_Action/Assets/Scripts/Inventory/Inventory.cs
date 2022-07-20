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

    /// <summary>
    /// 인벤토리 생성자
    /// </summary>
    /// <param name="size">인벤토리의 슬롯 수. 기본 값으로 Default_Inventory_Size(6) 사용 </param>
    public Inventory(int size = Default_Inventory_Size)
    {
        slots = new ItemSlot[size];     // 입력받은 갯수로 슬롯만들기
        for(int i=0;i<size;i++)
        {
            slots[i] = new ItemSlot();
        }
    }

    // AddItem은 함수 오버로딩(Overloading)을 통해 이름은 같지만 다양한 종류의 파라메터를 입력 받을 수 있게 했다.

    /// <summary>
    /// 아이템 추가하기 (적절한 빈칸에 넣기)
    /// </summary>
    /// <param name="id">추가할 아이템의 아이디</param>
    /// <returns>아이템 추가 성공 여부(true면 인벤토리에 아이템이 추가됨)</returns>
    public bool AddItem(uint id)
    {        
        return AddItem(GameManager.Inst.ItemData[id]);
    }

    /// <summary>
    /// 아이템 추가하기 (적절한 빈칸에 넣기)
    /// </summary>
    /// <param name="code">추가할 아이템의 코드</param>
    /// <returns>아이템 추가 성공 여부(true면 인벤토리에 아이템이 추가됨)</returns>
    public bool AddItem(ItemIDCode code)
    {
        return AddItem(GameManager.Inst.ItemData[code]);
    }

    /// <summary>
    /// 아이템 추가하기 (적절한 빈칸에 넣기)
    /// </summary>
    /// <param name="data">추가할 아이템의 아이템 데이터</param>
    /// <returns>아이템 추가 성공 여부(true면 인벤토리에 아이템이 추가됨)</returns>
    public bool AddItem(ItemData data)
    {
        bool result = false;

        Debug.Log($"인벤토리에 {data.itemName}을 추가합니다");
        ItemSlot slot = FindEmptySlot();    // 적절한 빈 슬롯 찾기
        if (slot != null)
        {
            slot.AssignSlotItem(data);      // 아이템 할당
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

    /// <summary>
    /// 아이템 추가하기 (특정한 슬롯에 넣기)
    /// </summary>
    /// <param name="id">추가할 아이템의 아이디</param>
    /// <param name="index">아이템을 추가할 슬롯의 인덱스</param>
    /// <returns>아이템을 추가하는데 성공하면 true. 아니면 false</returns>
    public bool AddItem(uint id, uint index)
    {
        return AddItem(GameManager.Inst.ItemData[id], index);
    }

    /// <summary>
    /// 아이템 추가하기 (특정한 슬롯에 넣기)
    /// </summary>
    /// <param name="code">추가할 아이템의 아이템코드</param>
    /// <param name="index">아이템을 추가할 슬롯의 인덱스</param>
    /// <returns>아이템을 추가하는데 성공하면 true. 아니면 false</returns>
    public bool AddItem(ItemIDCode code, uint index)
    {
        return AddItem(GameManager.Inst.ItemData[code], index);
    }

    /// <summary>
    /// 아이템 추가하기 (특정한 슬롯에 넣기)
    /// </summary>
    /// <param name="data">추가할 아이템의 아이템 데이터</param>
    /// <param name="index">아이템을 추가할 슬롯의 인덱스</param>
    /// <returns>아이템을 추가하는데 성공하면 true. 아니면 false</returns>
    public bool AddItem(ItemData data, uint index)
    {
        bool result = false;

        Debug.Log($"인벤토리의 {index} 슬롯에  {data.itemName}을 추가합니다");
        ItemSlot slot = slots[index];   // index번째의 슬롯 가져오기
        if(slot.IsEmpty())              // 찾은 슬롯이 비었는지 확인
        {
            slot.AssignSlotItem(data);  // 비어있으면 아이템 추가
            result = true;
            Debug.Log($"추가에 성공했습니다.");
        }
        else
        {
            Debug.Log($"실패 : {index} 슬롯에는 다른 아이템이 들어있습니다.");
        }

        return result;
    }


    /// <summary>
    /// 특정 슬롯의 아이템을 버리는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 버릴 슬롯의 인덱스</param>
    /// <returns>버리는데 성공하면 true, 아니면 false</returns>
    public bool RemoveItem(uint slotIndex)
    {
        bool result = false;

        Debug.Log($"인벤토리에서 {slotIndex} 슬롯을 비웁니다.");
        if (IsValidSlotIndex(slotIndex))        // slotIndex가 적절한 범위인지 확인
        {
            ItemSlot slot = slots[slotIndex];   
            Debug.Log($"{slot.SlotItemData.itemName}을 삭제합니다.");
            slot.ClearSlotItem();               // 적절한 슬롯이면 삭제 처리
            Debug.Log($"삭제에 성공했습니다.");
            result = true;
        }
        else
        {
            Debug.Log($"실패 : 잘못된 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 모든 아이템 슬롯을 비우는 함수
    /// </summary>
    public void ClearInventory()
    {
        Debug.Log("인벤토리 클리어");
        foreach(var slot in slots)
        {
            slot.ClearSlotItem();   // 전체 슬롯들을 돌면서 하나씩 삭제
        }
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

    /// <summary>
    /// 빈 슬롯을 찾아주는 함수
    /// </summary>
    /// <returns>빈 슬롯</returns>
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

    /// <summary>
    /// index값이 적절한 범위인지 확인해주는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 범위. 아니면 false</returns>
    private bool IsValidSlotIndex(uint index) => index < SlotCount;
    //{
    //    return index < SlotCount;
    //}


    /// <summary>
    /// 인벤토리 내용을 콘솔창에 출력해주는 함수
    /// </summary>
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
        if (!slot.IsEmpty())
        {
            printText += $"{slot.SlotItemData.itemName}]";
        }
        else
        {
            printText += "(빈칸)]";
        }

        //string.Join(',', 문자열 배열);
        Debug.Log(printText);
    }
}


// ItemData[]   -> ItemDataManager로 처리
// 아이템 이미지 -> ItemData에 추가될 내용
// 아이템 종류   -> ItemData에 추가될 내용