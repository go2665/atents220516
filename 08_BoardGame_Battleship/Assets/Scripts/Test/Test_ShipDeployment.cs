using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ShipDeployment : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        GameManager.Inst.SaveShipDeployData(GameManager.Inst.UserPlayer);
        //int i = 0;
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        GameManager.Inst.LoadShipDeplyData(GameManager.Inst.UserPlayer);
    }

    protected override void OnTest3(InputAction.CallbackContext obj)
    {
        UserPlayer player = GameManager.Inst.UserPlayer;
        Board board = player.Board;

        player.UndoAllShipDeployment();


        player.Ships[0].Direction = ShipDirection.NORTH;
        player.Ships[1].Direction = ShipDirection.NORTH;
        player.Ships[2].Direction = ShipDirection.NORTH;
        player.Ships[3].Direction = ShipDirection.NORTH;
        player.Ships[4].Direction = ShipDirection.NORTH;

        board.ShipDeployment(player.Ships[0], new Vector2Int(0, 0));
        board.ShipDeployment(player.Ships[1], new Vector2Int(1, 0));
        board.ShipDeployment(player.Ships[2], new Vector2Int(2, 0));
        board.ShipDeployment(player.Ships[3], new Vector2Int(3, 0));
        board.ShipDeployment(player.Ships[4], new Vector2Int(4, 0));

        //int i = 0;
    }
}
