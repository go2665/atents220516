using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Test_InputField : MonoBehaviour
{
    public TMP_InputField field;

    private void Start()
    {
        field.onValueChanged.AddListener(OnValueChange);    // 값이 변경될 때 실행
        field.onSelect.AddListener(OnSelect);               // 선택되었을 때 실행
        field.onDeselect.AddListener(OnDeselect);           // 선택이 해제되었을 때
        field.onEndEdit.AddListener(OnEndEdit);             // 입력이 끝났을 때
        field.onSubmit.AddListener(OnSubmit);               // 완료했을 때
    }

    private void OnSubmit(string arg0)
    {
        Debug.Log($"OnSubmit : {arg0}");
    }

    private void OnDeselect(string arg0)
    {
        Debug.Log($"OnDeselect : {arg0}");
    }

    private void OnEndEdit(string arg0)
    {
        Debug.Log($"OnEndEdit : {arg0}");
    }

    private void OnSelect(string arg0)
    {
        Debug.Log($"OnSelect : {arg0}");
    }

    private void OnValueChange(string arg0)
    {
        Debug.Log($"OnValueChange : {arg0}");
    }
}
