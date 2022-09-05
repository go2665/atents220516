using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tank : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float turnSpeed = 3.0f;
    public float turretTurnSpeed = 0.05f;

    private Transform turret;
    private Quaternion turretTargetRotation = Quaternion.identity;

    TankInputActions inputActions;

    Vector2 inputDir = Vector2.zero;

    Rigidbody rigid;

    private void Awake()
    {
        inputActions = new TankInputActions();
        rigid = GetComponent<Rigidbody>();
        turret = transform.Find("TankRenderers").Find("TankTurret");
    }

    private void OnEnable()
    {
        inputActions.Tank.Enable();
        inputActions.Tank.Move.performed += OnMove;
        inputActions.Tank.Move.canceled += OnMove;
        inputActions.Tank.Look.performed += OnMouseMove;
    }

    private void OnDisable()
    {
        inputActions.Tank.Look.performed -= OnMouseMove;
        inputActions.Tank.Move.canceled -= OnMove;
        inputActions.Tank.Move.performed -= OnMove;
        inputActions.Tank.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
    }

    void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector2 screenPos = context.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if( Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")) )
        {
            //Debug.Log("피킹");
            //hit.point;    // 마우스가 가리키는 땅 위치
            Vector3 lookDir = hit.point - turret.transform.position;
            lookDir.y = 0.0f;
            lookDir = lookDir.normalized;
            turretTargetRotation = Quaternion.LookRotation(lookDir);
        }
    }

    void TurretTurn()
    {
        turret.rotation = Quaternion.Slerp(turret.rotation, turretTargetRotation, turretTurnSpeed * Time.deltaTime);
    }

    private void Update()
    {
        TurretTurn();
    }

    private void FixedUpdate()
    {
        rigid.AddForce(inputDir.y * moveSpeed * transform.forward);
        rigid.AddTorque(inputDir.x * turnSpeed * transform.up);        
    }
}
