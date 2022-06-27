using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearTime : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Awake()
    {
        // UI 찾기
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// 시간 받아서 글자 변경하고 보여주기
    /// </summary>
    /// <param name="time">표시될 시간</param>
    public void SetTime(float time)
    {
        text.text = $"{time:f2}초 클리어";
        gameObject.SetActive(true);
    }
}
