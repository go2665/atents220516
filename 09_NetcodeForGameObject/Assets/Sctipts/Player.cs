using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    public float walkSpeed = 3.5f;
    public float rotateSpeed = 3.5f;

    PlayerInputActions inputActions;
    CharacterController controller;

    NetworkVariable<Vector3> networkMoveDelta = new NetworkVariable<Vector3>();
    NetworkVariable<float> networkRotateDelta = new NetworkVariable<float>();

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        ClientMoveAndRotate();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (IsClient && IsOwner)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector3 moveDelta = moveInput.y * walkSpeed * transform.forward;
            float rotateDelta = moveInput.x * rotateSpeed;

            UpdateClientMoveAndRotateServerRpc(moveDelta, rotateDelta);
        }
    }

    void ClientMoveAndRotate()
    {
        if (networkMoveDelta.Value != Vector3.zero)
        {
            controller.SimpleMove(networkMoveDelta.Value);
        }
        if (networkRotateDelta.Value != 0.0f)
        {
            transform.Rotate(0, networkRotateDelta.Value * Time.deltaTime, 0, Space.World);
        }
    }

    [ServerRpc]
    public void UpdateClientMoveAndRotateServerRpc(Vector3 moveDelta, float rotateDelta)
    {
        networkMoveDelta.Value = moveDelta;
        networkRotateDelta.Value = rotateDelta;
    }
    
}
