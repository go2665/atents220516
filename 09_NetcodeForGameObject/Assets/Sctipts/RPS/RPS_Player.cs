using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class RPS_Player : NetworkBehaviour
{
    NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>(); // 로그인 할 때 플레이어의 이름
    NetworkVariable<RPS_State> finalSelectHand = new NetworkVariable<RPS_State>();              // 컴펌 버튼을 눌렀을 때의 값

    RPS_State playerSelect = RPS_State.Scissors;    // 플레이어의 현재 선택

    UI_Manager_RPS uiManager;   //UI 매니저
    
    // 읽기용 프로퍼티
    public string Name => playerName.Value.ToString();
    public RPS_State Hand => finalSelectHand.Value;

    private void Awake()
    {
        //Debug.Log("Awake");

        finalSelectHand.OnValueChanged += SelectConfirm;    // finalSelectHand가 변경될 때 실행될 함수 연결
        playerName.OnValueChanged += NameChange;            // 이름이 변경될 때 실행될 함수 연결
    }

    /// <summary>
    /// 스폰되었을 때 실행되는 함수
    /// </summary>
    public override void OnNetworkSpawn()
    {
        //Debug.Log("OnNetworkSpawn");

        uiManager = UI_Manager_RPS.Inst;    // UI매니저 찾아놓기

        uiManager.SelectHandPanel.onSelectChange += SelectChange;   // UI에서 선택이 변경될 때 실행되는 델리게이트에 함수 연결

        if (IsOwner)
        {
            SetPlayerNameServerRpc(uiManager.nameInputField.text);  // 연결할 때 이름 설정용(요청해서 자신의 이름 설정)
            uiManager.confirm.onClick.AddListener(OnClickConfirm);  // Owner는 컴펌 버튼을 누르면 자신의 선택을 네트워크 변수에 기록
        }        
        else
        {
            this.gameObject.name = $"Player_{playerName.Value}";    // 다른 사람의 이름 설정
        }
    }

    private void OnClickConfirm()
    {
        if (IsOwner)
        {
            SendPlayerSelectServerRpc(playerSelect);    // 결정 버튼 눌러서 네트워크 변수 변경 시도
        }
    }

    [ServerRpc]
    void SetPlayerNameServerRpc(string text)
    {
        playerName.Value = text;
    }

    [ServerRpc]
    void SendPlayerSelectServerRpc(RPS_State state)
    {
        //Debug.Log("asdasdasd");
        finalSelectHand.Value = state;
    }

    /// <summary>
    /// 선택이 변경될때 실행되는 함수
    /// </summary>
    /// <param name="select"></param>
    private void SelectChange(RPS_State select)
    {
        playerSelect = select;
    }

    /// <summary>
    /// finalSelectHand 변수가 변경되었을 때 실행되는 함수
    /// </summary>
    /// <param name="previousValue"></param>
    /// <param name="newValue"></param>
    private void SelectConfirm(RPS_State previousValue, RPS_State newValue)
    {
        if (IsOwner)
        {
            uiManager.mySelect.text = $"당신은 {newValue}를 선택했습니다.";        // 내것이면 내것에 표시
        }
        else
        {
            uiManager.enemySelect.text = $"상대방이 결정을 내렸습니다.";            // 상대방의 것이면 적에 표시
        }

        uiManager.ConfirmedSelect[OwnerClientId] = this;    // 완료한 사람 딕셔너리에 추가
        uiManager.SetResultText();                          // 결과 표시 시도(완료한 사람이 2명이면 실행됨)
    }

    /// <summary>
    /// playerName이 변경되었을 때 실행되는 함수
    /// </summary>
    /// <param name="previousValue"></param>
    /// <param name="newValue"></param>
    private void NameChange(FixedString64Bytes previousValue, FixedString64Bytes newValue)
    {
        this.gameObject.name = $"Player_{newValue}";
    }
}
