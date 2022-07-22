using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempItemSlotUI : ItemSlotUI
{
    protected override void Awake()
    {
        itemImage = GetComponent<Image>();
    }

    public void Open()
    {
        gameObject.SetActive(true);        
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SetTempSlot(ItemSlot slot)
    {
        itemSlot = slot;
    }
}
