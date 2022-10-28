using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    UserPlayer user;        // 유저 플레이어
    EnemyPlayer enemy;      // 적 플레이어

    Button restart;         // 재시작 버튼
    Button dropdown;        // 테이블 열기/닫기 버튼
    bool isTableOpen = false;   // 테이블 열린 상태

    ResultTable table;              // 각종 결과가 출력되는 테이블
    ResultAnalysis userAnalysis;    // 유저 플레이 상세 정보 패널
    ResultAnalysis enemyAnalysis;   // 적 플레이 상세 정보 패널

    private void Awake()
    {
        // 컴포넌트 찾고 연결하기
        table = GetComponentInChildren<ResultTable>();
        ResultAnalysis[] analysises = GetComponentsInChildren<ResultAnalysis>();
        userAnalysis = analysises[0];
        enemyAnalysis = analysises[1];

        dropdown = transform.GetChild(0).GetComponent<Button>();        
        dropdown.onClick.AddListener(ToggleTable);

        restart = transform.GetChild(2).GetComponent<Button>();
        restart.onClick.AddListener(Restart);
    }

    private void Start()
    {
        user = GameManager.Inst.UserPlayer;     // 유저 캐싱해 놓기
        user.onDefeat += (_) => {
            Open();                             // 유저가 지면 Result 패널 열고
            table.SetDefeat();                  // 패배 상태로 테이블 열기
        };
        enemy = GameManager.Inst.EnemyPlayer;   // 적 캐싱해 놓기
        enemy.onDefeat += (_) => {
            Open();                             // 적이 지면 Result 패널 열고
            table.SetVictory();                 // 승리 상태로 테이블 열기
        };

        Close();    // 시작할 때는 닫아놓기
    }

    /// <summary>
    /// 테이블 영역 열고 닫는 함수
    /// </summary>
    private void ToggleTable()
    {
        if( isTableOpen )
        {
            table.Close();  // 열려있으면 닫고
        }
        else
        {
            table.Open();   // 닫혀있으면 열고
        }
        isTableOpen = !isTableOpen; // 표시 변경
    }

    /// <summary>
    /// 다시 함선 배치 씬으로 이동시키는 함수
    /// </summary>
    private void Restart()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Result 패널 열고 유저와 적의 상세 정보 설정
    /// </summary>
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

    /// <summary>
    /// Result 패널 닫기
    /// </summary>
    private void Close()
    {
        gameObject.SetActive(false);
    }
}
