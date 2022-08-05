using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    // 변수 ---------------------------------------------------------------------------------------    
    
    /// <summary>
    /// 인벤토리가 가지는 각 아이템 칸.
    /// </summary>
    ItemSlot[] slots = null;

    /// <summary>
    /// 아이템을 옮기거나 덜어낼때 사용할 임시 슬롯.
    /// </summary>
    ItemSlot tempSlot = null;

    // 상수 ---------------------------------------------------------------------------------------
    
    /// <summary>
    /// 인벤토리 기본 크기
    /// </summary>
    public const int Default_Inventory_Size = 6;

    /// <summary>
    /// TempSlot용 아이디
    /// </summary>
    public const uint TempSlotID = 99999;   //숫자는 의미가 없다. Slot Index로 적절하지 않은 값이면 된다.

    // 프로퍼티  -----------------------------------------------------------------------------------

    /// <summary>
    /// 인벤토리의 크기
    /// </summary>
    public int SlotCount => slots.Length;

    /// <summary>
    /// 임시 슬롯(읽기전용)
    /// </summary>
    public ItemSlot TempSlot => tempSlot;

    /// <summary>
    /// 인덱서. 인벤토리에서 슬롯 가져오기
    /// </summary>
    /// <param name="index">가져올 슬롯의 인덱스</param>
    /// <returns>index번째의 아이템 슬롯</returns>
    public ItemSlot this[int index] => slots[index];    


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
        tempSlot = new ItemSlot();      // 임시 용도로 사용하는 슬롯은 따로 생성
    }

    // AddItem은 함수 오버로딩(Overloading)을 통해 이름과 리턴값은 같지만 다양한 종류의 파라메터를 입력 받을 수 있게 했다.
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
        //Debug.Log($"인벤토리에 {data.itemName}을 추가합니다");

        ItemSlot target = FindSameItem(data);   // 같은 종류의 아이템이 인벤토리에 있는지 찾기
        if(target != null)
        {
            // 같은 종류의 아이템이 있으니 1만 증가시킨다.
            target.IncreaseSlotItem();
            result = true;
            //Debug.Log($"{data.itemName}을 하나 증가시킵니다.");
        }
        else
        {
            // 같은 종류의 아이템이 없다.
            ItemSlot empty = FindEmptySlot();    // 적절한 빈 슬롯 찾기
            if (empty != null)
            {
                empty.AssignSlotItem(data);      // 아이템 할당
                result = true;
                //Debug.Log($"아이템 슬롯에 {data.itemName}을 할당합니다.");
            }
            else
            {
                // 모든 슬롯에 아이템이 들어있다.(인벤토리가 가득찼다.)
                //Debug.Log($"실패 : 인벤토리가 가득찼습니다.");
            }
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

        //Debug.Log($"인벤토리의 {index} 슬롯에  {data.itemName}을 추가합니다");
        ItemSlot slot = slots[index];   // index번째의 슬롯 가져오기

        if(slot.IsEmpty())              // 찾은 슬롯이 비었는지 확인
        {
            slot.AssignSlotItem(data);  // 비어있으면 아이템 추가
            result = true;
            //Debug.Log($"추가에 성공했습니다.");
        }
        else
        {
            if (slot.SlotItemData == data)  // 같은 종류의 아이템인가?
            {
                if( slot.IncreaseSlotItem() == 0 )  // 들어갈 자리가 있는가?
                {
                    result = true;
                    //Debug.Log($"아이템 갯수 증가에 성공했습니다.");
                }
                else
                {
                    //Debug.Log($"실패 : 슬롯이 가득 찼습니다.");
                }
            }
            else
            {
                //Debug.Log($"실패 : {index} 슬롯에는 다른 아이템이 들어있습니다.");
            }
        }

        return result;
    }

    /// <summary>
    /// 특정 슬롯의 아이템을 버리는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 버릴 슬롯의 인덱스</param>
    /// <param name="decreaseCount">버리는 아이템 갯수</param>
    /// <returns>버리는데 성공하면 true, 아니면 false</returns>
    public bool RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        bool result = false;

        //Debug.Log($"인벤토리에서 {slotIndex} 슬롯의 아이템을 {decreaseCount}개 비웁니다.");
        if (IsValidSlotIndex(slotIndex))        // slotIndex가 적절한 범위인지 확인
        {
            ItemSlot slot = slots[slotIndex];
            slot.DecreaseSlotItem(decreaseCount);
            //Debug.Log($"삭제에 성공했습니다.");
            result = true;
        }
        else
        {
            //Debug.Log($"실패 : 잘못된 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 특정 슬롯의 아이템을 모두 버리는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 버릴 슬롯의 인덱스</param>
    /// <returns>버리는데 성공하면 true, 아니면 false</returns>
    public bool ClearItem(uint slotIndex)
    {
        bool result = false;

        //Debug.Log($"인벤토리에서 {slotIndex} 슬롯을 비웁니다.");
        if (IsValidSlotIndex(slotIndex))        // slotIndex가 적절한 범위인지 확인
        {
            ItemSlot slot = slots[slotIndex];
            //Debug.Log($"{slot.SlotItemData.itemName}을 삭제합니다.");
            slot.ClearSlotItem();               // 적절한 슬롯이면 삭제 처리
            //Debug.Log($"삭제에 성공했습니다.");
            result = true;
        }
        else
        {
            //Debug.Log($"실패 : 잘못된 인덱스입니다.");
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

    /// <summary>
    /// 아이템 이동시키기
    /// </summary>
    /// <param name="from">시작 슬롯의 ID</param>
    /// <param name="to">도착 슬롯의 ID</param>
    public void MoveItem(uint from, uint to)
    {
        // from, to 값에 따라 발생 가능한 4가지 경우의 수
            // from에 있고 to에 있고
            // from에 있고 to에 없고
            // from에 없고 to에 있고 -> 뭔가 실행되면 안된다.
            // from에 없고 to에 없고 -> 뭔가 실행되면 안된다.

        // from은 밸리드한 슬롯 인덱스고 슬롯이 비어있지 않다. 그리고 to는 밸리드한 슬롯 인덱스다.
        if(IsValidAndNotEmptySlot(from) && IsValidSlotIndex(to))
        {
            ItemSlot fromSlot = null;
            ItemSlot toSlot = null;

            // 인덱스로 슬롯 찾기
            if( from == TempSlotID )
            {
                fromSlot = TempSlot;    // temp슬롯은 별도로 인덱스 확인
            }
            else
            {
                fromSlot = slots[from]; // 다른 슬롯은 인덱스값 그대로 활용
            }
            if (to == TempSlotID)
            {
                toSlot = TempSlot;      // temp슬롯은 별도로 인덱스 확인
            }
            else
            {
                toSlot = slots[to];     // 다른 슬롯은 인덱스값 그대로 활용
            }

            // 두 슬롯에 들어있는 아이템 확인
            if ( fromSlot.SlotItemData == toSlot.SlotItemData )
            {
                // 같은 종류의 아이템이다. => to에 최대한 채우고 넘치면 temp에 그대로 남긴다.
                uint overCount = toSlot.IncreaseSlotItem(fromSlot.ItemCount);
                fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);
            }
            else
            {
                // 다른 종류의 아이템이다. => 아이템과 아이템 갯수를 서로 스왑한다.
                ItemData tempItemData = toSlot.SlotItemData;    // 임시 저장
                uint tempItemCount = toSlot.ItemCount;                
                toSlot.AssignSlotItem(fromSlot.SlotItemData, fromSlot.ItemCount);   // to에다 from의 정보 넣기
                fromSlot.AssignSlotItem(tempItemData, tempItemCount);               // from에다가 임시로 저장한 to의 정보 넣기                                
            }
            (toSlot.ItemEquiped, fromSlot.ItemEquiped) = (fromSlot.ItemEquiped, toSlot.ItemEquiped);
        }        
    }

    /// <summary>
    /// 아이템을 나눠서 임시 슬롯에 저장
    /// </summary>
    /// <param name="from">아이템을 나눌 슬롯</param>
    /// <param name="count">나는 아이템 갯수</param>
    public void TempRemoveItem(uint from, uint count = 1, bool equiped = false)
    {
        if( IsValidAndNotEmptySlot(from) )  // from이 절절한 슬롯이면
        {
            ItemSlot slot = slots[from];
            tempSlot.AssignSlotItem(slot.SlotItemData, count);  // temp 슬롯에 지정된 갯수의 아이템 할당
            slot.DecreaseSlotItem(count);   // from 슬롯에서 해당 갯수만큼 감소            
            tempSlot.ItemEquiped = equiped;
        }
    }

    // 아이템 사용하기
    // 아이템 장비하기
    // 아이템 정렬

    // 함수(백엔드) --------------------------------------------------------------------------------

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
    /// 같은 종류의 아이템이 들어있고 슬롯에 여유도 있는 슬롯을 찾아주는 함수
    /// </summary>
    /// <param name="itemData">찾을 아이템</param>
    /// <returns>찾을 아이템이 들어있는 슬롯</returns>
    private ItemSlot FindSameItem(ItemData itemData)
    {
        ItemSlot slot = null;
        for(int i=0;i<SlotCount;i++)
        {
            // 같은 종류의 아이템이 있고 슬롯에 아이템이 들어갈 여유가 있음
            if (slots[i].SlotItemData == itemData && slots[i].ItemCount < slots[i].SlotItemData.maxStackCount )
            {
                slot = slots[i];
                break;      // 찾으면 break로 종료
            }
        }
        return slot;
    }

    /// <summary>
    /// index값이 적절한 범위인지 확인해주는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 범위. 아니면 false</returns>
    private bool IsValidSlotIndex(uint index) => (index < SlotCount) || (index == TempSlotID);
    //{
    //    return index < SlotCount;
    //}

    /// <summary>
    /// index값이 적절한 범위이면서 아이템이 들어있는지도 확인해주는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 범위이면서 아이템도 들어있음.</returns>
    private bool IsValidAndNotEmptySlot(uint index)
    {
        ItemSlot testSlot = null;
        if(index != TempSlotID)
        {
            testSlot = slots[index];    // index가 tempSlot이 아니면 인덱스로 찾기
        }
        else
        {
            testSlot = TempSlot;    // index가 tempSlot인 경우 TempSlot 저장
        }

        return (IsValidSlotIndex(index) && !testSlot.IsEmpty());
    }

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
                printText += $"{slots[i].SlotItemData.itemName}({slots[i].ItemCount})";
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
            printText += $"{slot.SlotItemData.itemName}({slot.ItemCount})]";
        }
        else
        {
            printText += "(빈칸)]";
        }

        //string.Join(',', 문자열 배열);
        Debug.Log(printText);
    }
}