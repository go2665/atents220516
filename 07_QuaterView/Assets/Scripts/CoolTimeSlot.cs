using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoolTimeSlot : MonoBehaviour
{
    Image progressImage;
    TextMeshProUGUI coolTimeText;

    private void Awake()
    {
        progressImage = transform.GetChild(1).GetComponent<Image>();
        coolTimeText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void RefreshUI(float current, float max)
    {
        if(current < 0)
        {
            current = 0;
        }
        coolTimeText.text = $"{current:f1}";
        progressImage.fillAmount = current / max;
    }
}
