using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerDeco : NetworkBehaviour
{
    /// <summary>
    /// 플레이어의 이름을 저장할 네트워크 변수
    /// </summary>
    NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>();

    /// <summary>
    /// 플레이어의 색상을 저장할 네트워크 변수
    /// </summary>
    NetworkVariable<Color> playerColor = new NetworkVariable<Color>();

    /// <summary>
    /// 플레이어에게 달려있는 이름판
    /// </summary>
    TextMeshPro playerNamePlate;

    private void Awake()
    {
        // 이름판 미리 찾기
        playerNamePlate = GetComponentInChildren<TextMeshPro>();

        // 플레이어의 이름이 변경되었을 때 실행될 함수 연결
        playerName.OnValueChanged += OnNameChange;
    }

    /// <summary>
    /// 플레이어의 이름이 변경되었을때 실행될 함수
    /// </summary>
    /// <param name="previousValue"></param>
    /// <param name="newValue"></param>
    private void OnNameChange(FixedString64Bytes previousValue, FixedString64Bytes newValue)
    {
        // 플레이어의 이름이 변경되면 이름판에 표시되는 이름 갱신
        playerNamePlate.text = newValue.Value.ToString();
    }

    /// <summary>
    /// 네트워크 상에서 이 네트워크 오브젝트가 스폰되었을 때 실행
    /// </summary>
    public override void OnNetworkSpawn()
    {
        if( IsServer )
        {            
            playerColor.Value = Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);    // 시작할 때 서버면 랜덤하게 색상 설정
        }

        Renderer renderer = GetComponentInChildren<Renderer>();     // 색상 설정하기 위해 랜더러 찾기
        renderer.material.color = playerColor.Value;                // 렌덤으로 정해진 색상으로 설정  
    }

    /// <summary>
    /// 플레이어의 이름 변경을 서버에 요청
    /// </summary>
    /// <param name="text">새 이름</param>
    [ServerRpc]
    public void SetPlayerNameServerRpc(string text)
    {
        playerName.Value = text;
    }        
}
