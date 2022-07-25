using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailInfoUI : MonoBehaviour
{    
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;
    Image itemIcon;
    CanvasGroup canvasGroup;

    ItemData itemData;

    private void Awake()
    {
        itemName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        itemPrice = transform.Find("Value").GetComponent<TextMeshProUGUI>();
        itemIcon = transform.Find("Icon").GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Open(ItemData data)
    {
        itemData = data;
        Refresh();
        canvasGroup.alpha = 1;
    }

    public void Close()
    {
        itemData = null;
        canvasGroup.alpha = 0;
    }

    void Refresh()
    {
        if (itemData != null)
        {
            itemName.text = itemData.itemName;
            itemPrice.text = itemData.value.ToString();
            itemIcon.sprite = itemData.itemIcon;
        }
    }
}
