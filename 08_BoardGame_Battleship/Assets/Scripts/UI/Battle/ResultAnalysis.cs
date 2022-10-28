using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultAnalysis : MonoBehaviour
{
    int allAttackCount;
    int successAttackCount;
    int failAttackCount;
    float successRatio;

    public int AllAttackCount
    {
        set
        {
            allAttackCount = value;
            texts[0].text = allAttackCount.ToString();
        }
    }

    public int SuccessAttackCount
    {
        set
        {
            successAttackCount = value;
            texts[1].text = successAttackCount.ToString();
        }
    }

    public int FailAttackCount
    {
        set
        {
            failAttackCount = value;
            texts[2].text = failAttackCount.ToString();
        }
    }

    public float SuccessAttackRatio
    {
        set
        {
            successRatio = value;
            texts[3].text = $"{successRatio*100.0f:f1}%";
        }
    }

    TextMeshProUGUI[] texts;

    private void Awake()
    {
        Transform values = transform.GetChild(1);
        texts = values.GetComponentsInChildren<TextMeshProUGUI>();
    }
}
