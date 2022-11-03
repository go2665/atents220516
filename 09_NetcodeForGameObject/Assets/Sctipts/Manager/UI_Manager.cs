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
public class UI_Manager : Singleton<UI_Manager>
{
    public Button startHost;                // 호스트로 접속을 시도하는 버튼
    public Button startClient;              // 클라이언트로 접속을 시도하는 버튼
    public TMP_InputField nameInputField;   // 접속할 때 사용할 이름을 입력받을 인풋 필드

    private void Start()
    {
        // 호스트로 시작하는 람다함수를 버튼에 연결
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

        // 클라이언트로 시작하는 람다함수를 버튼에 연결
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
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;  // 이름을 설정하는 함수 연결
    }

    /// <summary>
    /// 클라이언트가 연결되었을 때 이름표에 이름 변경
    /// </summary>
    /// <param name="id">접속자의 OwnerClientID</param>
    private void OnClientConnect(ulong id)
    {
        Debug.Log($"{id} 클라이언트가 연결되었습니다.");
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();    // 로컬 플레이어 가져오기(자기 자신)
        PlayerDeco deco = netObj.GetComponent<PlayerDeco>();    // 이름표 가져오기
        deco.SetPlayerNameServerRpc(nameInputField.text);       // 이름 변경 요청
    }
}
