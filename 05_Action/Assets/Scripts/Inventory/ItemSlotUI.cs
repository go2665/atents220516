using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    // 기본 데이터 ---------------------------------------------------------------------------------
    /// <summary>
    /// 아이템 슬롯 아이디
    /// </summary>
    uint id;

    /// <summary>
    /// 이 슬롯UI에서 가지고 있을 ItemSlot(inventory클래스가 가지고 있는 ItemSlot중 하나)
    /// </summary>
    protected ItemSlot itemSlot;

    // 주요 인벤토리 UI 가지고 있기 -----------------------------------------------------------------

    /// <summary>
    /// 인벤토리 UI
    /// </summary>
    InventoryUI invenUI;

    /// <summary>
    /// 상세 정보창
    /// </summary>
    DetailInfoUI detailUI;

    // UI처리용 데이터 -----------------------------------------------------------------------------
    
    /// <summary>
    /// 아이템의 Icon을 표시할 이미지 컴포넌트
    /// </summary>
    protected Image itemImage;

    /// <summary>
    /// 아이템의 갯수를 표시할 Text 컴포넌트
    /// </summary>
    protected TextMeshProUGUI countText;

    /// <summary>
    /// 아이템의 장비 여부를 표시할 Text 컴포넌트
    /// </summary>
    protected TextMeshProUGUI equipMark;



    // 프로퍼티들 ----------------------------------------------------------------------------------

    /// <summary>
    /// 아이템 슬롯 아이디(읽기 전용)
    /// </summary>
    public uint ID { get => id; }

    /// <summary>
    /// 이 슬롯UI에서 가지고 있을 ItemSlot(읽기 전용)
    /// </summary>
    public ItemSlot ItemSlot { get => itemSlot; }

    // 함수들 --------------------------------------------------------------------------------------
    protected virtual void Awake()  // 오버라이드 가능하도록 virtual 추가
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();    // 아이템 표시용 이미지 컴포넌트 찾아놓기
        countText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        equipMark = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        equipMark.gameObject.SetActive(false);
    }

    /// <summary>
    /// ItemSlotUI의 초기화 작업
    /// </summary>
    /// <param name="newID">이 슬롯의 ID</param>
    /// <param name="targetSlot">이 슬롯이랑 연결된 ItemSlot</param>
    public void Initialize(uint newID, ItemSlot targetSlot)
    {
        invenUI = GameManager.Inst.InvenUI; // 미리 찾아놓기
        detailUI = invenUI.Detail;

        id = newID;
        itemSlot = targetSlot;
        itemSlot.onSlotItemChange = Refresh; // ItemSlot에 아이템이 변경될 경우 실행될 델리게이트에 함수 등록        
    }

    /// <summary>
    /// 슬롯에서 표시되는 아이콘 이미지 갱신용 함수
    /// </summary>
    public void Refresh()
    {
        if( itemSlot.SlotItemData != null )
        {
            // 이 슬롯에 아이템이 들어있을 때
            itemImage.sprite = itemSlot.SlotItemData.itemIcon;  // 아이콘 이미지 설정하고
            itemImage.color = Color.white;  // 불투명하게 만들기
            countText.text = itemSlot.ItemCount.ToString();

            // equipMark는 장비아이템이 장비중인 상황일때만 보여지기
            equipMark.gameObject.SetActive((itemSlot.SlotItemData is ItemData_Weapon) && itemSlot.ItemEquiped);
            
        }
        else
        {
            // 이 슬롯에 아이템이 없을 때
            itemImage.sprite = null;        // 아이콘 이미지 제거하고
            itemImage.color = Color.clear;  // 투명하게 만들기
            countText.text = "";
            equipMark.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 슬롯위에 마우스 포인터가 들어왔을 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSlot.SlotItemData != null)
        {
            //Debug.Log($"마우스가 {gameObject.name}안으로 들어왔다.");
            detailUI.Open(itemSlot.SlotItemData);
        }
    }

    /// <summary>
    /// 슬롯위에서 마우스 포인터가 나갔을 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"마우스가 {gameObject.name}에서 나갔다.");
        detailUI.Close();
    }

    /// <summary>
    /// 슬롯위에서 마우스 포인터가 움직일 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerMove(PointerEventData eventData)
    {
        //Debug.Log($"마우스가 {gameObject.name}안에서 움직인다.");
        Vector2 mousePos = eventData.position;

        // 상세정보창이 화면을 벗어났는지 체크
        RectTransform rect = (RectTransform)detailUI.transform;
        if( (mousePos.x + rect.sizeDelta.x) > Screen.width )
        {
            mousePos.x -= rect.sizeDelta.x; // 벗어났으면 상세정보창을 마우스 왼쪽으로 이동시킴)
        }

        detailUI.transform.position = mousePos; // 상세정보창을 마우스 커서 위치로 변경
    }

    /// <summary>
    /// 슬롯을 마우스로 클릭했을 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 왼쪽 버튼 클릭일 때
        if( eventData.button == PointerEventData.InputButton.Left )
        {
            TempItemSlotUI temp = invenUI.TempSlotUI;

            if (Keyboard.current.leftShiftKey.ReadValue() > 0 && temp.IsEmpty())
            {
                // 쉬프트 좌클릭하고 temp가 비었을 때 아이템 분할창 연다.

                //Debug.Log("Shift+좌클릭 => 분할창 열기");
                invenUI.SpliterUI.Open(this);   // 아이템 분할창 열기
                detailUI.Close();               // 상세정보창 닫기
                detailUI.IsPause = true;        // 상세정보창 일시 정지
            }
            else
            {
                if (!temp.IsEmpty())  // temp에 ItemSlot이 들어있다 => 아이템을 덜어낸 상황이다.                
                {
                    bool isEquipItem = temp.ItemSlot.ItemEquiped;
                    // 들고 있던 임시 아이템을 슬롯에 넣기                
                    if (ItemSlot.IsEmpty())
                    {
                        // 클릭한 슬롯이 빈칸이다.

                        // temp에 있는 내용을 이 슬롯에 다 넣기
                        itemSlot.AssignSlotItem(temp.ItemSlot.SlotItemData, temp.ItemSlot.ItemCount);
                        (temp.ItemSlot.ItemEquiped, itemSlot.ItemEquiped) = (itemSlot.ItemEquiped, temp.ItemSlot.ItemEquiped);
                        temp.Close();   // temp칸 비우기
                    }
                    else if (temp.ItemSlot.SlotItemData == ItemSlot.SlotItemData)
                    {
                        // 이 슬롯에는 같은 종류의 아이템이 들어있다.

                        // 담길 대상의 남은 공간
                        uint remains = ItemSlot.SlotItemData.maxStackCount - ItemSlot.ItemCount;
                        // 임시슬롯이 가지고 있는 것과 남은 공간 중 더 작은 것을 선택
                        //uint small = System.Math.Min(remains, temp.ItemSlot.ItemCount);
                        uint small = (uint)Mathf.Min((int)remains, (int)temp.ItemSlot.ItemCount);

                        ItemSlot.IncreaseSlotItem(small);
                        temp.ItemSlot.DecreaseSlotItem(small);
                        (temp.ItemSlot.ItemEquiped, itemSlot.ItemEquiped) = (itemSlot.ItemEquiped, temp.ItemSlot.ItemEquiped);

                        if (temp.ItemSlot.ItemCount < 1)    // 임시 슬롯에 있던 것을 전부 넣었을 때만 닫아라
                        {
                            temp.Close();
                        }                        
                    }
                    else
                    {
                        // 다른 종류의 아이템이다. => 서로 스왑
                        ItemData tempData = temp.ItemSlot.SlotItemData;
                        uint tempCount = temp.ItemSlot.ItemCount;
                        temp.ItemSlot.AssignSlotItem(itemSlot.SlotItemData, itemSlot.ItemCount);
                        itemSlot.AssignSlotItem(tempData, tempCount);
                        (temp.ItemSlot.ItemEquiped, itemSlot.ItemEquiped) = (itemSlot.ItemEquiped, temp.ItemSlot.ItemEquiped);
                    }  

                    if(isEquipItem) // 장비중인 아이템을 옮기는 상황이면 일단 해제하고 다시 장비
                    {
                        GameManager.Inst.MainPlayer.UnEquipWeapon();    
                        GameManager.Inst.MainPlayer.EquipWeapon(ItemSlot);
                    }

                    detailUI.IsPause = false;   // 상세정보창 일시정지 풀기
                }
                else
                {
                    // 그냥 클릭한 상황
                    if( !ItemSlot.IsEmpty() )
                    {
                        // 아이템 사용 시도
                        ItemSlot.UseSlotItem(GameManager.Inst.MainPlayer.gameObject);
                        if (ItemSlot.IsEmpty())
                        {
                            invenUI.Detail.Close();
                        }

                        // 아이템 장비 시도
                        bool isEquiped = ItemSlot.EquipSlotItem(GameManager.Inst.MainPlayer.gameObject);
                        if (isEquiped)
                        {
                            invenUI.ClearAllEquipMark();
                        }
                        ItemSlot.ItemEquiped = isEquiped;
                    }
                }
            }
        }
    }

    public void ClearEquipMark()
    {
        equipMark.gameObject.SetActive(false);
    }
}
