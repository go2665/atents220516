using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireData
{
    private ShellData shellData;
    private float currentCoolTime = 0.0f;

    public float CurrentCoolTime
    {
        get => currentCoolTime;
        set
        {
            currentCoolTime = value;
            onCoolTimeChange?.Invoke(currentCoolTime, shellData.coolTime);
        }
    }

    public bool IsFireReady { get => (currentCoolTime <= 0.0f); }

    public Action<float, float> onCoolTimeChange;

    public FireData(ShellData shellData, float startDelay = 0.0f)
    {
        this.shellData = shellData;
        this.currentCoolTime = startDelay;
    }

    public void ResetCoolTime()
    {
        CurrentCoolTime = shellData.coolTime;
    }

}
