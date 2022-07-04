using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public float runSpeed = 6.0f;
    public float turnSpeed = 5.0f;

    PlayerInputActions actions;
    CharacterController controller;
    
    Vector3 inputDir = Vector3.zero;
    Quaternion targetRotation = Quaternion.identity;

    private void Awake()
    {
        actions = new();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMove;
        actions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        actions.Player.Move.canceled -= OnMove;
        actions.Player.Move.performed -= OnMove;
        actions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Debug.Log(input);

        inputDir.x = input.x;
        inputDir.y = 0.0f;
        inputDir.z = input.y;
        inputDir.Normalize();

        if(inputDir.sqrMagnitude > 0.0f)
        {
            inputDir = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * inputDir;
            targetRotation = Quaternion.LookRotation(inputDir);
        }
    }

    private void Update()
    {
        controller.Move(runSpeed * Time.deltaTime * inputDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
