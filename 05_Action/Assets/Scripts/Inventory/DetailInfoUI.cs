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

    public bool IsPause;    // 상세정보창 열고 닫기 일시 정지(true면 열리지 않는다.)

    private void Awake()
    {
        itemName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        itemPrice = transform.Find("Value").GetComponent<TextMeshProUGUI>();
        itemIcon = transform.Find("Icon").GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Open(ItemData data)
    {
        if (!IsPause)
        {
            itemData = data;
            Refresh();
            canvasGroup.alpha = 1;
        }
    }

    public void Close()
    {
        if (!IsPause)
        {
            itemData = null;
            canvasGroup.alpha = 0;
        }
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
