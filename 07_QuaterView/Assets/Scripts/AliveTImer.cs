using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class AliveTImer : MonoBehaviour
{
    TextMeshProUGUI timer;

    float time = 0.0f;
    bool isTimerWork = false;

    private void Awake()
    {
        timer = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        PlayerTank player = FindObjectOfType<PlayerTank>();
        player.onDead += TimerStop;

        TimerReset();
    }

    private void Update()
    {
        if( isTimerWork )
        {
            time += Time.deltaTime;
            timer.text = $"{time:f2}";
        }
    }

    void TimerReset()
    {
        time = 0.0f;
        isTimerWork = true;
    }

    void TimerStop()
    {
        isTimerWork = false;
    }


}
