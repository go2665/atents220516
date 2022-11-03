using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// UI 관리 클래스
/// </summary>
public class UI_Manager_RPS : Singleton<UI_Manager_RPS>
{
    public CanvasGroup loginPanel;
    public Button startHost;                // 호스트로 접속을 시도하는 버튼
    public Button startClient;              // 클라이언트로 접속을 시도하는 버튼
    public TMP_InputField nameInputField;   // 접속할 때 사용할 이름을 입력받을 인풋 필드
    public Button confirm;
    public RadioButtons SelectHandPanel;

    public TextMeshProUGUI mySelect;
    public TextMeshProUGUI enemySelect;
    public TextMeshProUGUI result;

    public Dictionary<ulong, RPS_Player> ConfirmedSelect = new Dictionary<ulong, RPS_Player>(2);

    private void Start()
    {
        loginPanel.interactable = true;
        loginPanel.alpha = 1.0f;

        // 호스트로 시작하는 람다함수를 버튼에 연결
        startHost?.onClick.AddListener( () =>
        {
            if(NetworkManager.Singleton.StartHost())    
            {
                Debug.Log("호스트가 시작되었습니다...");
                loginPanel.interactable = false;
                loginPanel.alpha = 0.0f;
            }
            else
            {
                Debug.Log("호스트 시작에 실패했습니다....");
            }
        });

        // 클라이언트로 시작하는 람다함수를 버튼에 연결
        startClient?.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("클라이언트가 연결을 시작하였습니다...");
                loginPanel.interactable = false;
                loginPanel.alpha = 0.0f;
            }
            else
            {
                Debug.Log("클라이언트가 연결에 실패했습니다....");
            }
        });

        // OnClientConnectedCallback : 클라이언트가 연결되면 실행되는 델리게이트
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;  // 이름을 설정하는 함수 연결

        ConfirmedSelect.Clear();
    }

    /// <summary>
    /// 클라이언트가 연결되었을 때 이름표에 이름 변경
    /// </summary>
    /// <param name="id">접속자의 OwnerClientID</param>
    private void OnClientConnect(ulong id)
    {
        Debug.Log($"{id} 클라이언트가 연결되었습니다.");
        //NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();    // 로컬 플레이어 가져오기(자기 자신)
        //PlayerDeco deco = netObj.GetComponent<PlayerDeco>();    // 이름표 가져오기
        //deco.SetPlayerNameServerRpc(nameInputField.text);       // 이름 변경 요청
    }

    /// <summary>
    /// 승부 결과 표시
    /// </summary>
    public void SetResultText()
    {
        if (ConfirmedSelect.Count > 1)  // 완료한 사람이 2명 이상일 때 처리
        {
            // 선택 사항 텍스트에 기입
            string resultText = $"{ConfirmedSelect[0].Name}은 {ConfirmedSelect[0].Hand}를 선택했습니다."
                + $"\n{ConfirmedSelect[1].Name}은 {ConfirmedSelect[1].Hand}를 선택했습니다.";

            RPS_Player winner = null;

            // 승자 구분
            switch (ConfirmedSelect[0].Hand)
            {
                case RPS_State.Scissors:
                    switch (ConfirmedSelect[1].Hand)
                    {
                        case RPS_State.Rock:
                            winner = ConfirmedSelect[1];
                            break;
                        case RPS_State.Paper:
                            winner = ConfirmedSelect[0];
                            break;
                        case RPS_State.Scissors:
                        default:
                            winner = null;
                            break;
                    }
                    break;
                case RPS_State.Rock:
                    switch (ConfirmedSelect[1].Hand)
                    {
                        case RPS_State.Scissors:
                            winner = ConfirmedSelect[0];
                            break;
                        case RPS_State.Paper:
                            winner = ConfirmedSelect[1];
                            break;
                        case RPS_State.Rock:
                        default:
                            winner = null;
                            break;
                    }
                    break;
                case RPS_State.Paper:
                    switch (ConfirmedSelect[1].Hand)
                    {
                        case RPS_State.Scissors:
                            winner = ConfirmedSelect[1];
                            break;
                        case RPS_State.Rock:
                            winner = ConfirmedSelect[0];
                            break;
                        case RPS_State.Paper:
                        default:
                            winner = null;
                            break;
                    }
                    break;
                default:
                    winner = null;
                    break;
            }

            // 최종 승자 기입
            resultText += $"\n{winner.Name}이 승리했습니다.";

            // 기입한 내용을 출력
            this.result.text = resultText;
        }
    }
}
