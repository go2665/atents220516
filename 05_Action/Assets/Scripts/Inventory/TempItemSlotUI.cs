using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 임시로 한번씩만 보이는 슬롯
/// </summary>
public class TempItemSlotUI : ItemSlotUI
{
    /// <summary>
    /// Awake을 override해서 부모의 Awake 실행안되게 만들기(base.Awake 제거)
    /// </summary>
    protected override void Awake()
    {
        itemImage = GetComponent<Image>();  // 이미지 찾아오기
        countText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();    // 마우스 위치에 맞춰서 임시 슬롯 이동
    }

    /// <summary>
    /// 임시 슬롯을 보이도록 열기
    /// </summary>
    /// <param name="itemSlot">임시 슬롯에 할당할 아이템이 들어있는 슬롯</param>
    public void Open(ItemSlot itemSlot)
    {
        SetTempSlot(itemSlot);  // 슬롯 설정
        transform.position = Mouse.current.position.ReadValue();    // 보이기 전에 위치 조정
        gameObject.SetActive(true); // 실제로 보이게 만들기(활성화시키기)
    }

    /// <summary>
    /// 임시 슬롯이 보이지 않게 닫기
    /// </summary>
    public void Close()
    {        
        itemSlot = null;
        gameObject.SetActive(false);    // 실제로 보이지 않게 만들기(비활성화시키기)
    }

    /// <summary>
    /// 임시 슬롯에서 보일 슬롯 지정
    /// </summary>
    /// <param name="slot">보여질 슬롯</param>
    private void SetTempSlot(ItemSlot slot)
    {        
        itemSlot = slot;    // 슬롯 설정하고
        Refresh();          // 화면 갱신
    }
}
