using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TempItemSlotUI : ItemSlotUI
{
    protected override void Awake()
    {
        itemImage = GetComponent<Image>();
    }

    public void Open()
    {
        transform.position = Mouse.current.position.ReadValue();
        itemImage.color = Color.white;
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

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();
    }
}
