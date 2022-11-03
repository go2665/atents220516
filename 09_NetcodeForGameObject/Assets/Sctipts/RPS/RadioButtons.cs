using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RPS_State
{
    Scissors = 0,
    Rock,
    Paper,
    None
}

public class RadioButtons : MonoBehaviour
{
    public Color selectColor;

    Button[] buttons;
    Image[] images;
    RPS_State select;

    public RPS_State Select
    {
        get => select;
        private set
        {
            select = value;
            onSelectChange?.Invoke(select);
        }
    }

    public Action<RPS_State> onSelectChange;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        images = GetComponentsInChildren<Image>();

        buttons[(int)RPS_State.Rock].onClick.AddListener(SelectRock);
        buttons[(int)RPS_State.Paper].onClick.AddListener(SelectPaper);
        buttons[(int)RPS_State.Scissors].onClick.AddListener(SelectScissors);
    }

    private void Start()
    {
        SelectScissors();
    }

    void SelectRock()
    {
        images[(int)RPS_State.Rock].color = selectColor;
        images[(int)RPS_State.Paper].color = Color.white;
        images[(int)RPS_State.Scissors].color = Color.white;
        Select = RPS_State.Rock;
    }

    void SelectPaper()
    {
        images[(int)RPS_State.Rock].color = Color.white;
        images[(int)RPS_State.Paper].color = selectColor;
        images[(int)RPS_State.Scissors].color = Color.white;
        Select = RPS_State.Paper;
    }

    void SelectScissors()
    {
        images[(int)RPS_State.Rock].color = Color.white; 
        images[(int)RPS_State.Paper].color = Color.white;
        images[(int)RPS_State.Scissors].color = selectColor;
        Select = RPS_State.Scissors;
    }
}
