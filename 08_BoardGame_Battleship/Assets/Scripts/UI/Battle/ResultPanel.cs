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
    ResultAnalysis userAnalysis;
    ResultAnalysis enemyAnalysis;

    private void Awake()
    {
        table = GetComponentInChildren<ResultTable>();
        ResultAnalysis[] analysises = GetComponentsInChildren<ResultAnalysis>();
        userAnalysis = analysises[0];
        enemyAnalysis = analysises[1];

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
        userAnalysis.AllAttackCount = user.SuccessAttackCount + user.FailAttackCount;
        userAnalysis.SuccessAttackCount = user.SuccessAttackCount;
        userAnalysis.FailAttackCount = user.FailAttackCount;
        userAnalysis.SuccessAttackRatio = (float)user.SuccessAttackCount / (user.SuccessAttackCount + user.FailAttackCount);

        enemyAnalysis.AllAttackCount = enemy.SuccessAttackCount + enemy.FailAttackCount;
        enemyAnalysis.SuccessAttackCount = enemy.SuccessAttackCount;
        enemyAnalysis.FailAttackCount = enemy.FailAttackCount;
        enemyAnalysis.SuccessAttackRatio = (float)enemy.SuccessAttackCount / (enemy.SuccessAttackCount + enemy.FailAttackCount);

        //enemyAnalysis;
        gameObject.SetActive(true);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
