using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 현재 턴 표시
public class TurnPrinter : MonoBehaviour
{
    TextMeshProUGUI turnText;

    private void Awake()
    {
        turnText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        TurnManager turnManager = TurnManager.Inst;
        turnManager.onTurnStart += OnTurnChange;    // 턴매니저가 가지고 있는 턴시작 델리게이트에 연결
        turnText.text = $"1 턴";
    }

    /// <summary>
    /// 턴이 새로 시작되면 호출될 함수
    /// </summary>
    /// <param name="turnNumber">시작된 턴의 숫자</param>
    private void OnTurnChange(int turnNumber)
    {
        turnText.text = $"{turnNumber} 턴";  // 표시될 턴 값 변경
    }
}
