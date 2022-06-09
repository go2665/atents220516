using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeUI : MonoBehaviour
{
    Text lifeText = null;

    private void Awake()
    {
        lifeText = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        GameManager.Inst.MainPlayer.onHit += Refresh;   // onHit에 Refresh를 추가
    }

    /// <summary>
    /// 플레이어가 맞아서 생명이 감소할 때마다 실행될 함수
    /// </summary>
    void Refresh()
    {       
        lifeText.text = $"X {GameManager.Inst.MainPlayer.Life}";
    }
}
