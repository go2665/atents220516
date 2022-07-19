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
    TextMeshProUGUI goldText;   // 돈 표시할 text

    // --------------------------------------------------------------------------------------------

    private void Awake()
    {
        goldText = transform.Find("Gold").Find("GoldText").GetComponent<TextMeshProUGUI>(); // 그냥 찾기
    }

    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
        player.OnMoneyChange += RefreshMoney;   // 플레이어의 Money가 변경되는 실행되는 델리게이트에 함수 등록
        RefreshMoney(player.Money);
    }

    private void RefreshMoney(int money)
    {
        goldText.text = $"{money:N0}";  // Money가 변경될 때 실행될 함수
    }
}
