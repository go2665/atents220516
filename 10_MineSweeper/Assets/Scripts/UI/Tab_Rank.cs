using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tab_Rank : TabBase
{
    public enum RankType
    {
        Time,
        Click
    }

    public RankType rankType;

    List<int> rankList;
    TextMeshProUGUI[] rankDataText;

    private void Awake()
    {
        rankDataText = transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        switch (rankType)
        {
            case RankType.Time:
                GameManager.Inst.onTimeRankUpdated += OnRankUpdated;
                break;
            case RankType.Click:
                GameManager.Inst.onClickRankUpdated += OnRankUpdated;
                break;
        }
    }

    private void OnRankUpdated(List<int> rank)
    {
        rankList = rank;
        Refresh();
    }

    public override void Refresh()
    {
        int i = 0;
        foreach(var data in rankList)
        {
            rankDataText[i].text = data.ToString();
            i++;
        }
    }
}
