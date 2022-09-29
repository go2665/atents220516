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
        userPlayer.Test_SetState(PlayerState.ShipDeployment);
        userPlayer.SelectShipToDeploy(ShipType.PatrolBoat);

        //bool result = board.ShipDeployment(ship, new Vector2Int(0, 0));
        //Debug.Log(result);
        //result = board.ShipDeployment(ship, new Vector2Int(4, 1));
        //Debug.Log(result);
        
    }
}
