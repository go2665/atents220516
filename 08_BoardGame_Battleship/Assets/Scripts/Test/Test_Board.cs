using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Board : MonoBehaviour
{
    public Board board;
    public GameObject shipPrefab;
    public UserPlayer userPlayer;

    void Start()
    {
        userPlayer.Test_SetState(PlayerState.ShipDeployment);   // 테스트 유저의 상태를 함선 배치모드로 설정
        userPlayer.SelectShipToDeploy(ShipType.Submarine);      // 현재 배치할 배를 잠수함으로 설정

        //bool result = board.ShipDeployment(ship, new Vector2Int(0, 0));
        //Debug.Log(result);
        //result = board.ShipDeployment(ship, new Vector2Int(4, 1));
        //Debug.Log(result);
        
    }
}
