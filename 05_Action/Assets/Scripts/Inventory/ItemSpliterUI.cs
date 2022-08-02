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
    /// <summary>
    /// 아이템 분할 갯수용 변수
    /// </summary>
    uint itemSplitCount = 1;

    /// <summary>
    /// 아이템을 분할할 슬롯UI
    /// </summary>
    ItemSlotUI targetSlotUI;

    /// <summary>
    /// 갯수 입력 및 표시를 위한 UI(Input Field)
    /// </summary>
    TMP_InputField inputField;

    /// <summary>
    /// OK 버튼을 눌렀을 때 실행될 델리게이트
    /// </summary>
    public Action<uint,uint> OnOkClick;

    /// <summary>
    /// 아이템 분할 갯수용 프로퍼티(private)
    /// </summary>
    uint ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            // 값이 입력될 때 최소값은 1, 최대값은 (대상슬롯이 가지고 있는 아이템 갯수 - 1)로 설정하는 코드
            if (itemSplitCount != value)
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
    }

    /// <summary>
    /// 초기화 함수(컴포넌트 찾고 UI에 함수 연결)
    /// </summary>
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

    /// <summary>
    /// 아이템 분할창 열기.
    /// </summary>
    /// <param name="target">아이템을 분할할 대상 슬롯</param>
    public void Open(ItemSlotUI target)
    {
        if (target.ItemSlot.ItemCount > 1)  // 대상 슬롯에 아이템이 1개를 초과할 때만 실행
        {
            targetSlotUI = target;  // 대상 슬롯으로 설정
            ItemSplitCount = 1;     // 기본 값으로 1 설정
            transform.position = target.transform.position; // 아이템을 나눌 슬롯의 위치로 스플리터UI 옮기기
            gameObject.SetActive(true); // 보여주기
        }
    }

    /// <summary>
    /// 아이템 분할창 닫기
    /// </summary>
    public void Close() => gameObject.SetActive(false);

    /// <summary>
    /// 갯수 증가시키는 버튼 눌렀을 때 실행될 함수
    /// </summary>
    private void OnIncrease()
    {
        Debug.Log("OnIncrease");
        ItemSplitCount++;
    }

    /// <summary>
    /// 갯수 감소시키는 버튼 눌렀을 때 실행될 함수
    /// </summary>
    private void OnDecrease()
    {
        Debug.Log("OnDecrease");
        ItemSplitCount--;
    }

    /// <summary>
    /// OK 버튼 눌렀을 때 실행될 함수
    /// </summary>
    private void OnOK()
    {
        //Debug.Log("OnOK");

        OnOkClick?.Invoke(targetSlotUI.ID, ItemSplitCount); // 델리게이트에 연결된 함수들 실행.(InventoryUI에서 SpliterOK 함수 실행)

        Close();    // 닫기
    }

    /// <summary>
    /// Cancel 버튼 눌렀을 때 실행될 함수
    /// </summary>
    private void OnCancelClick()
    {
        //Debug.Log("OnCancelClick");
        targetSlotUI = null;    // 변수들 초기화하고
        Close();                // 닫기
    }

    /// <summary>
    /// InputField에서 값이 변경될 때 실행될 함수
    /// </summary>
    /// <param name="input">변경된 값</param>
    private void OnInputChange(string input)
    {
        //Debug.Log($"OnInputChange : {input}");
        if( input.Length == 0)
        {
            ItemSplitCount = 0; // ""인 경우 0으로 처리
        }
        else
        {
            ItemSplitCount = uint.Parse(input); // uint 파싱해서 ItemSplitCount에 대입
        }
    }    
}
