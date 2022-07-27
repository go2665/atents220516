using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ItemSpliterUI : MonoBehaviour
{
    uint itemSplitCount = 1;
    ItemSlotUI targetSlotUI;

    TMP_InputField inputField;

    uint ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            itemSplitCount = value;
            itemSplitCount = (uint)Mathf.Max(1, itemSplitCount);    // 1이 최소값
            //(targetSlotUI.ItemSlot.ItemCount - 1)이 최대 값.            
            itemSplitCount = (uint)Mathf.Min(itemSplitCount, targetSlotUI.ItemSlot.ItemCount - 1);            
            inputField.text = itemSplitCount.ToString();
        }
    }

    public void Open(ItemSlotUI target)
    {
        if (target.ItemSlot.ItemCount > 1)
        {
            targetSlotUI = target;
            ItemSplitCount = 1;
            gameObject.SetActive(true);
        }
    }

    public void Close() => gameObject.SetActive(false);

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnInputChange);

        Button increase = transform.Find("IncreaseButton").GetComponent<Button>();
        increase.onClick.AddListener(OnIncrease);
        Button decrease = transform.Find("DecreaseButton").GetComponent<Button>();
        decrease.onClick.AddListener(OnDecrease);
        Button ok = transform.Find("OK_Button").GetComponent<Button>();
        ok.onClick.AddListener(OnOK);
        Button cancel = transform.Find("Cancel_Button").GetComponent<Button>();
        cancel.onClick.AddListener(OnCancelClick);
    }

    private void Start()
    {
        Debug.Log("Start");        
        inputField.text = itemSplitCount.ToString();
    }

    private void OnIncrease()
    {
        Debug.Log("OnIncrease");
        ItemSplitCount++;
    }

    private void OnDecrease()
    {
        Debug.Log("OnDecrease");
        ItemSplitCount--;
    }

    private void OnOK()
    {
        Debug.Log("OnOK");
        targetSlotUI.ItemSlot.DecreaseSlotItem(ItemSplitCount);
        ItemSlot tempSlot = new(targetSlotUI.ItemSlot.SlotItemData, ItemSplitCount);
        GameManager.Inst.InvenUI.TempSlotUI.Open(tempSlot);
        Close();
    }

    private void OnCancelClick()
    {
        Debug.Log("OnCancelClick");
        targetSlotUI = null;
        Close();
    }

    private void OnInputChange(string input)
    {
        Debug.Log($"OnInputChange : {input}");
        ItemSplitCount = uint.Parse(input);
    }    
}
