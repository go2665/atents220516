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

    void Refresh()
    {
        lifeText.text = $"X {GameManager.Inst.MainPlayer.Life}";
    }
}
