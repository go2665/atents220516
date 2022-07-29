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

    InventoryUI invenUI;
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
        countText = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// ItemSlotUI의 초기화 작업
    /// </summary>
    /// <param name="newID">이 슬롯의 ID</param>
    /// <param name="targetSlot">이 슬롯이랑 연결된 ItemSlot</param>
    public void Initialize(uint newID, ItemSlot targetSlot)
    {
        invenUI = GameManager.Inst.InvenUI;
        detailUI = invenUI.Detail;

        id = newID;
        itemSlot = targetSlot;
        itemSlot.onSlotItemChage = Refresh; // ItemSlot에 아이템이 변경될 경우 실행될 델리게이트에 함수 등록        
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
        }
        else
        {
            // 이 슬롯에 아이템이 없을 때
            itemImage.sprite = null;        // 아이콘 이미지 제거하고
            itemImage.color = Color.clear;  // 투명하게 만들기
            countText.text = "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSlot.SlotItemData != null)
        {
            //Debug.Log($"마우스가 {gameObject.name}안으로 들어왔다.");
            detailUI.Open(itemSlot.SlotItemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"마우스가 {gameObject.name}에서 나갔다.");
        detailUI.Close();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //Debug.Log($"마우스가 {gameObject.name}안에서 움직인다.");
        Vector2 mousePos = eventData.position;

        RectTransform rect = (RectTransform)detailUI.transform;
        if( (mousePos.x + rect.sizeDelta.x) > Screen.width )
        {
            mousePos.x -= rect.sizeDelta.x;
        }

        detailUI.transform.position = mousePos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if( eventData.button == PointerEventData.InputButton.Left )
        {
            if(Keyboard.current.leftShiftKey.ReadValue() > 0)
            {
                Debug.Log("Shift+좌클릭 => 분할창 열기");
                invenUI.SpliterUI.Open(this);
            }
            else
            {
                // 분할된 아이템을 슬롯에 넣기
                //2.1.완전히 빈칸에 넣기
                //2.2. (같은 종류의 아이템이) 절반쯤 차있는 칸에 넣기(넣었을 때 넘치지 않음)
                //2.3. (같은 종류의 아이템이) 절반쯤 차있는 칸에 넣기(넣고도 남는 경우)

                //invenUI.TempSlotUI.ItemSlot.IsEmpty();

                TempItemSlotUI temp = invenUI.TempSlotUI;

                //temp.gameObject.activeSelf;
                if (temp.ItemSlot != null)  // temp에 ItemSlot이 들어있다 => 아이템을 덜어낸 상황이다.
                {
                    if (ItemSlot.IsEmpty())
                    {
                        // 이 슬롯이 빈칸이다.
                        itemSlot.AssignSlotItem(temp.ItemSlot.SlotItemData, temp.ItemSlot.ItemCount);
                        temp.Close();
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
                    }
                    
                }
            }
        }
    }
}
