using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    // 슬롯 번호
    // 아이템슬롯(아이템 데이터(아이템 이미지))

    uint id;
    ItemSlot itemSlot;  // inventory클래스가 가지고 있는 ItemSlot중 하나

    Image itemImage;


    public uint ID { get => id; }
    public ItemSlot ItemSlot { get => itemSlot; }

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void Initialize(uint newID, ItemSlot targetSlot)
    {
        id = newID;
        itemSlot = targetSlot;
        itemSlot.onSlotItemChage = Refresh;
    }

    public void Refresh()
    {
        if( itemSlot.SlotItemData != null )
        {
            itemImage.sprite = itemSlot.SlotItemData.itemIcon;
            itemImage.color = Color.white;
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
        }
    }
}
