using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 접속자 수 표시용 클래스
/// </summary>
public class PlayerManager : NetworkSingleton<PlayerManager>
{
    /// <summary>
    /// 현재 게임에 접속해 있는 플레이어의 숫자
    /// </summary>
    NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    /// <summary>
    /// playersInGame에 변화가 있을 때 실행될 델리게이트
    /// </summary>
    public Action<int> onPlayerCountChange;

    /// <summary>
    /// 현재 접속한 인원을 알려주는 프로퍼티
    /// </summary>
    public int PlayerInGame => playersInGame.Value;

    private void Start()
    {
        // 클라이언트가 접속했을 때 OnIncreasePlayerInGame이 실행되도록 연결
        NetworkManager.Singleton.OnClientConnectedCallback += OnIncreasePlayerInGame;

        // 클라이언트가 접속 해제했을 때 OnDecreasePlayerInGame이 실행되도록 연결
        NetworkManager.Singleton.OnClientDisconnectCallback += OnDecreasePlayerInGame;

        //NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        //{
        //    if (NetworkManager.Singleton.IsServer)
        //    {
        //        playersInGame.Value++;
        //        onPlayerCountChange?.Invoke(PlayerInGame);
        //        Debug.Log($"{id}가 연결 되었습니다.");
        //    }
        //};

        //NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        //{
        //    if (NetworkManager.Singleton.IsServer)
        //    {
        //        playersInGame.Value--;
        //        onPlayerCountChange?.Invoke(PlayerInGame);
        //        Debug.Log($"{id}가 연결 해제 되었습니다.");
        //    }
        //};
        // playersInGame이 변경되었을 때 실행될 함수 연결
        playersInGame.OnValueChanged += OnPlayersInGameChange;
    }

    /// <summary>
    /// 클라이언트가 접속했을 때 playersInGame을 증가 시키는 함수
    /// </summary>
    /// <param name="id">접속자의 OwnerClientID</param>
    private void OnIncreasePlayerInGame(ulong id)
    {
        if (NetworkManager.Singleton.IsServer)  
        {
            playersInGame.Value++;  // 서버일 때만 playersInGame 수정
            Debug.Log($"{id}가 연결 되었습니다.");
        }
    }

    /// <summary>
    /// 클라이언트가 접속 해제했을 때 playersInGame을 감소 시키는 함수
    /// </summary>
    /// <param name="id">접속자의 OwnerClientID</param>
    private void OnDecreasePlayerInGame(ulong id)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            playersInGame.Value--;  // 서버일 때만 playersInGame 수정
            Debug.Log($"{id}가 연결 해제 되었습니다.");
        }
    }

    /// <summary>
    /// playersInGame이 변경되었을 때 실행될 함수
    /// </summary>
    /// <param name="previousValue">이전값</param>
    /// <param name="newValue">현재값</param>
    private void OnPlayersInGameChange(int previousValue, int newValue)
    {
        onPlayerCountChange?.Invoke(newValue);  // 델리게이트 실행(접속자 수 갱신 함수만 연결되어있음)
    }
}
