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

    public Action<uint,uint> OnOkClick;

    uint ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            itemSplitCount = value;
            itemSplitCount = (uint)Mathf.Max(1, itemSplitCount);    // 1이 최소값
            //(targetSlotUI.ItemSlot.ItemCount - 1)이 최대 값.
            if (targetSlotUI != null)
            {
                itemSplitCount = (uint)Mathf.Min(itemSplitCount, targetSlotUI.ItemSlot.ItemCount - 1);
            }
            inputField.text = itemSplitCount.ToString();
        }
    }

    public void Initialize()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnInputChange);
        inputField.text = itemSplitCount.ToString();

        Button increase = transform.Find("IncreaseButton").GetComponent<Button>();
        increase.onClick.AddListener(OnIncrease);
        Button decrease = transform.Find("DecreaseButton").GetComponent<Button>();
        decrease.onClick.AddListener(OnDecrease);
        Button ok = transform.Find("OK_Button").GetComponent<Button>();
        ok.onClick.AddListener(OnOK);
        Button cancel = transform.Find("Cancel_Button").GetComponent<Button>();
        cancel.onClick.AddListener(OnCancelClick);

        Close();
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
        //targetSlotUI.ItemSlot.DecreaseSlotItem(ItemSplitCount);
        //ItemSlot tempSlot = new(targetSlotUI.ItemSlot.SlotItemData, ItemSplitCount);
        //tempSlot.onSlotItemChage = GameManager.Inst.InvenUI.TempSlotUI.Refresh;
        //GameManager.Inst.InvenUI.TempSlotUI.Open(tempSlot);

        OnOkClick?.Invoke(targetSlotUI.ID, ItemSplitCount);

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
        if( input.Length == 0)
        {
            ItemSplitCount = 0;
        }
        else
        {
            ItemSplitCount = uint.Parse(input);
        }
    }    
}
