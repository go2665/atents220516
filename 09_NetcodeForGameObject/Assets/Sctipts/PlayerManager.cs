using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkSingleton<PlayerManager>
{
    /// <summary>
    /// 현재 게임에 접속해 있는 플레이어의 숫자
    /// </summary>
    NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public Action<int> onPlayerCountChange;

    public int PlayerInGame => playersInGame.Value;

    private void Start()
    {
        // 접속자 수의 변화가 있으면 playersInGame를 증감 시키는 코드 + 델리게이트 실행

        NetworkManager.Singleton.OnClientConnectedCallback += OnIncreasePlayerInGame;
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

        playersInGame.OnValueChanged += OnPlayersInGameChange;
    }

    private void OnPlayersInGameChange(int previousValue, int newValue)
    {
        onPlayerCountChange?.Invoke(newValue);
    }

    private void OnIncreasePlayerInGame(ulong id)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            playersInGame.Value++;
            
            Debug.Log($"{id}가 연결 되었습니다.");
        }
    }

    private void OnDecreasePlayerInGame(ulong id)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            playersInGame.Value--;
            //onPlayerCountChange?.Invoke(PlayerInGame);
            Debug.Log($"{id}가 연결 해제 되었습니다.");
        }
    }
}
