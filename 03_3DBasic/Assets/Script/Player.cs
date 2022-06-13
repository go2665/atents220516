using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float turnSpeed = 180.0f;
    PlayerInputActions actions = null;
    Vector3 inputDir = Vector3.zero;
    float inputSide = 0.0f;

    Rigidbody rigid = null;

    private void Awake()
    {
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
        actions.Player.SideMove.performed += OnSideMoveInput;
        actions.Player.SideMove.canceled += OnSideMoveInput;
    }    

    private void OnDisable()
    {
        actions.Player.SideMove.canceled -= OnSideMoveInput;
        actions.Player.SideMove.performed -= OnSideMoveInput;
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Move.performed -= OnMoveInput;   
        actions.Player.Disable();
    }

    private void FixedUpdate()
    {
        // inputDir의 y값을 이용하여 이 오브젝트의 앞쪽 방향(transform.forward)으로 이동
        // inputSide를 이용해서 이 오브젝트의 오른쪽 방향(transform.right)으로 이동
        rigid.MovePosition(rigid.position 
            + moveSpeed * Time.fixedDeltaTime * (inputDir.y * transform.forward + inputSide * transform.right));

        rigid.MoveRotation(rigid.rotation * Quaternion.AngleAxis(inputDir.x * turnSpeed * Time.fixedDeltaTime, transform.up));
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        inputDir = context.ReadValue<Vector2>();    // Vector2.x = a키(-1) d키(+1),  Vector2.y = w키(+1) s키(-1)
    }

    private void OnSideMoveInput(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<float>());
        inputSide = context.ReadValue<float>();
    }
}
