using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    // 기본 데이터 ---------------------------------------------------------------------------------
    Inventory inven;
    Player player;

    // 돈 UI --------------------------------------------------------------------------------------
    TextMeshProUGUI goldText;

    // --------------------------------------------------------------------------------------------

    private void Awake()
    {
        goldText = transform.Find("Gold").Find("GoldText").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
        player.OnMoneyChange += RefreshMoney;
    }

    private void RefreshMoney(int money)
    {
        goldText.text = $"{money:N0}";
    }
}
