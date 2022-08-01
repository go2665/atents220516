using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // public -------------------------------------------------------------------------------------
    /// <summary>
    /// ItemSlotUI가 있는 프리팹. 
    /// </summary>
    public GameObject slotPrefab;   // 초기화시 새로 생성해야할 경우 사용


    // 기본 데이터 ---------------------------------------------------------------------------------
    /// <summary>
    /// 이 인벤토리를 사용하는 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 이 클래스로 표현하려는 인벤토리
    /// </summary>
    Inventory inven;

    /// <summary>
    /// 슬롯 생성시 부모가 될 게임 오브젝트의 트랜스폼
    /// </summary>
    Transform slotParent;

    /// <summary>
    /// 이 인벤토리가 가지고 있는 슬롯UI들
    /// </summary>
    ItemSlotUI[] slotUIs;

    /// <summary>
    /// 열고 닫기용 캔버스 그룹
    /// </summary>
    CanvasGroup canvasGroup;


    // Item관련  ----------------------------------------------------------------------------------    
    /// <summary>
    /// 드래그 시작 표시용( 시작 id가 InvalideID면 드래그 시작을 안한 것)
    /// </summary>
    const uint InvalideID = uint.MaxValue;

    /// <summary>
    /// 드래그가 시작된 슬롯의 ID
    /// </summary>
    uint dragStartID = InvalideID;

    /// <summary>
    /// 임시 슬롯(아이템 드래그나 아이템 분리할 때 사용)
    /// </summary>
    TempItemSlotUI tempItemSlotUI;
    public TempItemSlotUI TempSlotUI => tempItemSlotUI;

    // 상세 정보 UI --------------------------------------------------------------------------------
    /// <summary>
    /// 아이템 상세정보 창
    /// </summary>
    DetailInfoUI detail;
    public DetailInfoUI Detail => detail;

    // 아이템 분할 UI --------------------------------------------------------------------------------
    /// <summary>
    /// 아이템 분할 창
    /// </summary>
    ItemSpliterUI itemSpliterUI;
    public ItemSpliterUI SpliterUI => itemSpliterUI;

    // 돈 UI --------------------------------------------------------------------------------------
    /// <summary>
    /// 돈 표시할 text
    /// </summary>
    TextMeshProUGUI goldText;

    // 델리게이트 ----------------------------------------------------------------------------------
    public Action OnInventoryOpen;
    public Action OnInventoryClose;


    // 유니티 이벤트 함수들 -------------------------------------------------------------------------
    private void Awake()
    {
        // 미리 찾아놓기
        canvasGroup = GetComponent<CanvasGroup>();
        goldText = transform.Find("Gold").Find("GoldText").GetComponent<TextMeshProUGUI>(); 
        slotParent = transform.Find("ItemSlots");
        tempItemSlotUI = GetComponentInChildren<TempItemSlotUI>();
        detail = GetComponentInChildren<DetailInfoUI>();
        itemSpliterUI = GetComponentInChildren<ItemSpliterUI>();
        itemSpliterUI.Initialize();

        Button closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(Close);
    }


    private void Start()
    {
        player = GameManager.Inst.MainPlayer;   // 게임 메니저에서 플레이어 가져오기
        player.OnMoneyChange += RefreshMoney;   // 플레이어의 Money가 변경되는 실행되는 델리게이트에 함수 등록
        RefreshMoney(player.Money);             // 첫 갱신

        Close();    // 시작할 때 무조건 닫기
    }

    // 일반 함수들 ---------------------------------------------------------------------------------

    /// <summary>
    /// 인벤토리를 입력받아 UI를 초기화하는 함수
    /// </summary>
    /// <param name="newInven">이 UI로 표시할 인벤토리</param>
    public void InitializeInventory(Inventory newInven)
    {
        inven = newInven;   //즉시 할당
        if( Inventory.Default_Inventory_Size != newInven.SlotCount )    // 기본 사이즈와 다르면 기본 슬롯UI 삭제
        {
            // 기존 슬롯UI 전부 삭제
            ItemSlotUI[] slots = GetComponentsInChildren<ItemSlotUI>();
            foreach(var slot in slots)
            {
                Destroy(slot.gameObject);
            }

            // 새로 만들기
            slotUIs = new ItemSlotUI[inven.SlotCount];
            for (int i=0;i<inven.SlotCount;i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);
                obj.name = $"{slotPrefab.name}_{i}";            // 이름 지어주고
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();
                slotUIs[i].Initialize((uint)i, inven[i]);       // 각 슬롯UI들도 초기화
            }
        }
        else
        {
            // 크기가 같을 경우 슬롯UI들의 초기화만 진행
            slotUIs = slotParent.GetComponentsInChildren<ItemSlotUI>();
            for (int i = 0; i < inven.SlotCount; i++)
            {
                slotUIs[i].Initialize((uint)i, inven[i]);   
            }
        }

        tempItemSlotUI.Initialize(TempItemSlotUI.TempSlotID, inven.TempSlot);
        tempItemSlotUI.Close(); // 닫은체로 시작하기

        RefreshAllSlots();  // 전체 슬롯UI 갱신
    }

    /// <summary>
    /// 모든 슬롯의 Icon이미지를 갱신
    /// </summary>
    private void RefreshAllSlots()
    {
        foreach(var slotUI in slotUIs)
        {
            slotUI.Refresh();
        }
    }

    /// <summary>
    /// 플레이어가 가진 돈을 갱신
    /// </summary>
    /// <param name="money">표시될 금액</param>
    private void RefreshMoney(int money)
    {
        goldText.text = $"{money:N0}";  // Money가 변경될 때 실행될 함수
    }

    /// <summary>
    /// 인벤토리 열고 닫기
    /// </summary>
    public void InventoryOnOffSwitch()
    {
        if(canvasGroup.blocksRaycasts)  // 캔버스 그룹의 blocksRaycasts를 기준으로 처리
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    /// <summary>
    /// 인벤토리 열기
    /// </summary>
    void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        OnInventoryOpen?.Invoke();
    }

    /// <summary>
    /// 인벤토리 닫기
    /// </summary>
    void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        OnInventoryClose?.Invoke();
    }

    // 이벤트 시스템의 인터페이스 함수들 -------------------------------------------------------------

    /// <summary>
    /// 드래그 중에 실행(OnBeginDrag, OnEndDrag를 사용하려면 반드시 필요해서 넣은 것)
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        // OnBeginDrag, OnEndDrag를 사용하려면 반드시 필요해서 넣은 것

        //if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    tempItemSlotUI.transform.position = eventData.position;
        //}
    }

    /// <summary>
    /// 드래그 시작시 실행
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if( eventData.button == PointerEventData.InputButton.Left ) // 좌클릭일 때만 처리
        {
            if (TempSlotUI.IsEmpty())    // 임시 슬롯에 아이템이 없는 경우에만 실행(아이템은 나누어서 들고 있는 상황)
            {
                GameObject startObj = eventData.pointerCurrentRaycast.gameObject;   // 드래그 시작한 위치에 있는 게임 오브젝트 가져오기
                if (startObj != null)
                {
                    // 드래그 시작한 위치에 게임 오브젝트가 있으면
                    //Debug.Log(startObj.name);
                    ItemSlotUI slotUI = startObj.GetComponent<ItemSlotUI>();    // ItemSlotUI 컴포넌트 가져오기
                    if (slotUI != null)
                    {
                        // ItemSlotUI 컴포넌트가 있으면 ID 기록해 놓기
                        //Debug.Log($"Start SlotID : {slotUI.ID}");
                        dragStartID = slotUI.ID;
                        inven.TempRemoveItem(dragStartID, slotUI.ItemSlot.ItemCount);
                        tempItemSlotUI.Open();   // 드래그 시작할 때 열기
                        detail.Close();         // 상세정보창 닫기
                        detail.IsPause = true;  // 상세정보창 안열리게 하기
                    }
                }
            }
        }
    }

    /// <summary>
    /// 드래그가 끝났을 때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // 좌클릭일 때만 처리
        {
            if (dragStartID != InvalideID)  // 드래그가 정상적으로 시작되었을 때만 처리
            {
                GameObject endObj = eventData.pointerCurrentRaycast.gameObject; // 드래그 끝난 위치에 있는 게임 오브젝트 가져오기
                if (endObj != null)
                {
                    // 드래그 끝난 위치에 게임 오브젝트가 있으면
                    //Debug.Log(endObj.name);
                    ItemSlotUI slotUI = endObj.GetComponent<ItemSlotUI>();  // ItemSlotUI 컴포넌트 가져오기
                    if (slotUI != null)
                    {
                        // ItemSlotUI 컴포넌트가 있으면 Inventory.MoveItem() 실행시키기
                        //Debug.Log($"End SlotID : {slotUI.ID}");
                        //inven.MoveItem(dragStartID, slotUI.ID);         // 아이템 실제로 옮기기

                        inven.TempToSlot(slotUI.ID);
                        inven.TempToSlot(dragStartID);

                        detail.IsPause = false;                         // 상세정보창 다시 열릴 수 있게 하기
                        detail.Open(slotUI.ItemSlot.SlotItemData);      // 상세정보창 열기
                        dragStartID = InvalideID;                       // 드래그 시작 id를 될 수 없는 값으로 설정(드래그가 끝났음을 표시)
                    }
                }
                if (tempItemSlotUI.IsEmpty())
                {
                    tempItemSlotUI.Close(); // 정상적으로 드래그가 끝나면 닫기
                }
            }
        }
    }
}
