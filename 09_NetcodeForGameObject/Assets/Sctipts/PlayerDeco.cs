using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerDeco : NetworkBehaviour
{
    NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>();

    /// <summary>
    /// 네트워크 상에서 이 네트워크 오브젝트가 스폰되었을 때 실행
    /// </summary>
    public override void OnNetworkSpawn()
    {
        if( IsServer )
        {
            //OwnerClientId;
            playerName.Value = "aaaaaa";
        }
    }

        
}
