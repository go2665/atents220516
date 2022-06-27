using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearTime : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTime(float time)
    {
        text.text = $"{time:f2}초 클리어";
        gameObject.SetActive(true);
    }
}
