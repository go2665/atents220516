using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_Manager : Singleton<UI_Manager>
{
    public Button startHost;
    public Button startClient;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI playerCounter;

    public TMP_InputField NameInputField => nameInputField;

    private void Start()
    {
        startHost?.onClick.AddListener( () =>
        {
            if(NetworkManager.Singleton.StartHost())
            {
                Debug.Log("호스트가 시작되었습니다...");
            }
            else
            {
                Debug.Log("호스트 시작에 실패했습니다....");
            }
        });

        startClient?.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("클라이언트가 연결을 시작하였습니다...");
            }
            else
            {
                Debug.Log("클라이언트가 연결에 실패했습니다....");
            }
        });

        // OnClientConnectedCallback : 클라이언트가 연결되면 실행되는 델리게이트
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;

        PlayerManager.Inst.onPlayerCountChange += RefreshPlayerCounter;
    }

    private void OnClientConnect(ulong id)
    {
        Debug.Log($"{id} 클라이언트가 연결되었습니다.");
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();    // 로컬 플레이어 가져오기(자기 자신)
        PlayerDeco deco = netObj.GetComponent<PlayerDeco>();
        deco.SetPlayerNameServerRpc(nameInputField.text);       // 이름 변경 요청
    }

    void RefreshPlayerCounter(int playerInGame)
    {
        // 플레이어 숫자 찍어주기
        playerCounter.text = $"Player Count : {playerInGame}";
    }
}
