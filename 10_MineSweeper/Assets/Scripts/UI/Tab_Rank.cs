using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tab_Rank : TabBase
{
    // 랭크의 종류
    public enum RankType
    {
        Time,   // 시간 순서
        Click   // 클릭 순서
    }

    /// <summary>
    /// 인스팩터 창에서 설정하기 위한 용도
    /// </summary>
    public RankType rankType;

    /// <summary>
    /// 표시할 랭킹의 기록들
    /// </summary>
    List<int> rankList;

    /// <summary>
    /// 랭킹 데이터를 출력할 텍스트
    /// </summary>
    TextMeshProUGUI[] rankDataText;

    private void Awake()
    {
        rankDataText = transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();    // 텍스트 찾아오기
    }

    private void Start()
    {
        switch (rankType)   // 설정된 랭킹 종류에 따라서 다른 델리게이트에 연결하기
        {
            case RankType.Time:
                GameManager.Inst.onTimeRankUpdated += OnRankUpdated;
                break;
            case RankType.Click:
                GameManager.Inst.onClickRankUpdated += OnRankUpdated;
                break;
        }
    }

    /// <summary>
    /// 랭킹 변화를 알리는 델리게이트에 연결될 함수
    /// </summary>
    /// <param name="rank"></param>
    private void OnRankUpdated(List<int> rank)
    {
        rankList = rank;    // 랭킹 정보 받아와서
        Refresh();          // 화면 갱신
    }

    /// <summary>
    /// 화면 갱신용 함수. 출력될 랭킹 데이터 변경
    /// </summary>
    public override void Refresh()
    {
        int i = 0;
        foreach(var data in rankList)       // rankList에 있는 데이터를 텍스트에 하나씩 출력
        {
            rankDataText[i].text = data.ToString();
            i++;
        }
    }
}
