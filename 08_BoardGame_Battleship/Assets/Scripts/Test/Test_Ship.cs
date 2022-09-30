using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Ship : TestBase
{
    public UserPlayer userPlayer;

    private void Start()
    {
        userPlayer.Test_SetState(PlayerState.ShipDeployment);
    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Carrier);
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Battleship);
    }

    protected override void OnTest3(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Destroyer);
    }

    protected override void OnTest4(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.Submarine);
    }

    protected override void OnTest5(InputAction.CallbackContext obj)
    {
        userPlayer.SelectShipToDeploy(ShipType.PatrolBoat);
    }

}
