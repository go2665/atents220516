using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class ResultPanel : MonoBehaviour
{
    UserPlayer user;
    EnemyPlayer enemy;

    Button dropdown;
    bool isTableOpen = false;

    ResultTable table;

    private void Awake()
    {
        table = GetComponentInChildren<ResultTable>();
        dropdown = GetComponentInChildren<Button>();
        dropdown.onClick.AddListener(ToggleTable);
    }

    private void Start()
    {
        user = GameManager.Inst.UserPlayer;
        user.onDefeat += (_) => {
            Open();
            table.SetDefeat(); 
        };
        enemy = GameManager.Inst.EnemyPlayer;
        enemy.onDefeat += (_) => {
            Open();
            table.SetVictory(); 
        };

        Close();
    }

    private void ToggleTable()
    {
        if( isTableOpen )
        {
            table.Close();
        }
        else
        {
            table.Open();
        }
        isTableOpen = !isTableOpen;
    }

    private void Open()
    {
        gameObject.SetActive(true);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
