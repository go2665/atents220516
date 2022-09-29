using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserPlayer : PlayerBase
{
    Ship selectedShip = null;

    Action<Vector2>[] onClick;
    Action<Vector2>[] onMouseMove;
    Action<float>[] onMouseWheel;

    private void Awake()
    {
        int length = Enum.GetValues(typeof(PlayerState)).Length;
        onClick = new Action<Vector2>[length];
        onClick[(int)PlayerState.Title] = OnClick_Title;
        onClick[(int)PlayerState.ShipDeployment] = OnClick_ShipDeployment;
        onClick[(int)PlayerState.Battle] = OnClick_Battle;
        onClick[(int)PlayerState.GameEnd] = OnClick_GameEnd;

        onMouseMove = new Action<Vector2>[length];
        onMouseMove[(int)PlayerState.Title] = OnMouseMove_Title;
        onMouseMove[(int)PlayerState.ShipDeployment] = OnMouseMove_ShipDeployment;
        onMouseMove[(int)PlayerState.Battle] = OnMouseMove_Battle;
        onMouseMove[(int)PlayerState.GameEnd] = OnMouseMove_GameEnd;

        onMouseWheel = new Action<float>[length];
        onMouseWheel[(int)PlayerState.Title] = OnMouseWheel_Title;
        onMouseWheel[(int)PlayerState.ShipDeployment] = OnMouseWheel_ShipDeployment;
        onMouseWheel[(int)PlayerState.Battle] = OnMouseWheel_Battle;
        onMouseWheel[(int)PlayerState.GameEnd] = OnMouseWheel_GameEnd;
    }    

    protected override void Start()
    {
        base.Start();
        GameManager.Inst.Input.onClick += OnClick;
        GameManager.Inst.Input.onMouseMove += OnMouseMove;
        GameManager.Inst.Input.onMouseWheel += OnMouseWheel;
    }
        
    public void SelectShipToDeploy(ShipType type)
    {
        selectedShip = ships[(int)(type - 1)];
        selectedShip.gameObject.SetActive(true);
    }

    private void OnClick(Vector2 screenPos)
    {
        onClick[(int)state]?.Invoke(screenPos);
    }

    private void OnMouseMove(Vector2 screenPos)
    {
        onMouseMove[(int)state]?.Invoke(screenPos);
    }

    private void OnMouseWheel(float wheel)
    {
        onMouseWheel[(int)state]?.Invoke(wheel);
    }


    protected virtual void OnClick_Title(Vector2 screenPos)
    {
    }

    protected virtual void OnClick_ShipDeployment(Vector2 screenPos)
    {
    }

    protected virtual void OnClick_Battle(Vector2 screenPos)
    {
    }

    protected virtual void OnClick_GameEnd(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_Title(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_ShipDeployment(Vector2 screenPos)
    {
        if (selectedShip != null)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            Vector2Int gridPos = board.WorldToGrid(worldPos);
            worldPos = board.GridToWorld(gridPos);
            //Debug.Log(gridPos);

            selectedShip.transform.position = worldPos;
        }
    }

    protected virtual void OnMouseMove_Battle(Vector2 screenPos)
    {
    }

    protected virtual void OnMouseMove_GameEnd(Vector2 screenPos)
    {
    }

    private void OnMouseWheel_Title(float wheel)
    {
    }

    private void OnMouseWheel_ShipDeployment(float wheel)
    {
        bool ccw = false;
        if (wheel > 0)
            ccw = true;

        selectedShip.Rotate(ccw);
    }

    private void OnMouseWheel_Battle(float wheel)
    {
    }

    private void OnMouseWheel_GameEnd(float wheel)
    {
    }
}
