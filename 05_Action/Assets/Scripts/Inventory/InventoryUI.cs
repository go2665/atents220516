using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // public -------------------------------------------------------------------------------------
    public GameObject slotPrefab;

    // 기본 데이터 ---------------------------------------------------------------------------------
    Player player;
    Inventory inven;
    Transform slotParent;
    ItemSlotUI[] slotUIs;

    // Item관련  ----------------------------------------------------------------------------------
    uint dragStartID;

    // 돈 UI --------------------------------------------------------------------------------------
    TextMeshProUGUI goldText;   // 돈 표시할 text


    // --------------------------------------------------------------------------------------------

    private void Awake()
    {
        slotParent = transform.Find("ItemSlots");
        goldText = transform.Find("Gold").Find("GoldText").GetComponent<TextMeshProUGUI>(); // 그냥 찾기
    }

    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
        player.OnMoneyChange += RefreshMoney;   // 플레이어의 Money가 변경되는 실행되는 델리게이트에 함수 등록
        RefreshMoney(player.Money);
    }

    public void InitializeInventory(Inventory newInven)
    {
        inven = newInven;
        if( Inventory.Default_Inventory_Size != newInven.SlotCount )    // 기본 사이즈와 다르면 기본 슬롯 삭제
        {
            // 기존 슬롯 전부 삭제
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
                obj.name = $"{slotPrefab.name}_{i}";
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();
                slotUIs[i].Initialize((uint)i, inven[i]);
            }
        }
        else
        {
            slotUIs = GetComponentsInChildren<ItemSlotUI>();
            for (int i = 0; i < inven.SlotCount; i++)
            {
                slotUIs[i].Initialize((uint)i, inven[i]);
            }
        }
        RefreshAllSlots();
    }

    private void RefreshAllSlots()
    {
        foreach(var slotUI in slotUIs)
        {
            slotUI.Refresh();
        }
    }

    private void RefreshMoney(int money)
    {
        goldText.text = $"{money:N0}";  // Money가 변경될 때 실행될 함수
    }

    // 이벤트 시스템의 인터페이스 함수들 -------------------------------------------------------------
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if( eventData.button == PointerEventData.InputButton.Left )
        {
            GameObject startObj = eventData.pointerCurrentRaycast.gameObject;
            if (startObj != null)
            {
                //Debug.Log(startObj.name);
                ItemSlotUI slotUI = startObj.GetComponent<ItemSlotUI>();
                if(slotUI != null)
                {
                    Debug.Log($"Start SlotID : {slotUI.ID}");
                    dragStartID = slotUI.ID;
                }
            }

            //try
            //{
            //    if(eventData.pointerCurrentRaycast.gameObject.transform.parent == null)
            //    {
            //        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
            //    }
            //}
            //catch( Exception e )
            //{
            //    Debug.Log(e);
            //}
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject endObj = eventData.pointerCurrentRaycast.gameObject;
            if (endObj != null)
            {
                //Debug.Log(endObj.name);
                ItemSlotUI slotUI = endObj.GetComponent<ItemSlotUI>();
                if (slotUI != null)
                {
                    Debug.Log($"End SlotID : {slotUI.ID}");
                    inven.MoveItem(dragStartID, slotUI.ID);
                }
            }
        }
    }
}
