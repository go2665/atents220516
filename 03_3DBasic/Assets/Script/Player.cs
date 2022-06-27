using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDead
{
    public float moveSpeed = 5.0f;
    public float turnSpeed = 180.0f;
    public float jumpPower = 3.0f;

    bool isJumping = false;
    PlayerInputActions actions = null;
    Vector3 inputDir = Vector3.zero;
    float inputSide = 0.0f;
    bool tryUse = false;

    Rigidbody rigid = null;
    Animator anim = null;

    private void Awake()
    {
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
        actions.Player.SideMove.performed += OnSideMoveInput;
        actions.Player.SideMove.canceled += OnSideMoveInput;
        actions.Player.Jump.performed += OnJumpInput;
        actions.Player.Use.performed += OnUseInput;
    }    

    private void OnDisable()
    {
        actions.Player.Use.performed -= OnUseInput;
        actions.Player.Jump.performed -= OnJumpInput;
        actions.Player.SideMove.canceled -= OnSideMoveInput;
        actions.Player.SideMove.performed -= OnSideMoveInput;
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Move.performed -= OnMoveInput;   
        actions.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();               
    }

    void Move()
    {
        // inputDir의 y값을 이용하여 이 오브젝트의 앞쪽 방향(transform.forward)으로 이동
        // inputSide를 이용해서 이 오브젝트의 오른쪽 방향(transform.right)으로 이동
        rigid.MovePosition(rigid.position
            + moveSpeed * Time.fixedDeltaTime * (inputDir.y * transform.forward + inputSide * transform.right));
    }

    void Rotate()
    {
        // inputDir.x를 이용하여 우회전(d,+1)인지 좌회전(a,-1)인지 결정. 회전의 중심축
        rigid.MoveRotation(rigid.rotation * Quaternion.AngleAxis(inputDir.x * turnSpeed * Time.fixedDeltaTime, transform.up));
    }

    void Jump()
    {
        //if (isJumping == false)
        if (!isJumping)
        {
            rigid.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        }
    }
        
    void Use()
    {
        // 단순 애니메이션 재생만 한다.
        anim.SetTrigger("Use"); // 애니메이션 재생 결과로 Sphere 트리거가 활성화되면서 OnTriggerEnter가 실행될 수 있도록 한다.
        tryUse = true;
    }

    public void UseEnd()
    {
        tryUse = false;     // 헛손질 한 후 뒤에 사용되는 것을 방지(애니메이션 끝날 때 자동으로 처리)
    }

    // OnTriggerEnter : 트리거에 컬라이더가 들어왔을 때
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{other.name} : 무엇인가가 들어았다.");
        // 사용이 가능한지 아닌지 물어보기(useable이 null이 아니라면 IUseable인터페이스를 상속 받았기에 사용할 수 있는 오브젝트다)

        // other : 플레이어가 가진 트리거 안에 들어온 컬라이더들
        //DoorManual door = other.GetComponentInParent<DoorManual>();       // 98~102라인과 기능이 같다. 대신 한정된 상황에서만 사용가능
        //if( door != null)
        //{
        //    door.Use();
        //}    
                
        if (tryUse)     // 내가 사용을 시도했을 때만 대상을 사용하도록 if
        {
            // other는 DoorManual인데 DoorManual은 Door 클래스와 IUseable인터페이스를 상속 받았다.
            // 그래서 DoorManual 컴포넌트를 가져 올 때 IUseable 변수나 Door 변수로 저장할 수 있다.
            IUseable useable = other.GetComponentInParent<IUseable>();
            if (useable != null)
            {
                useable.Use();  // 사용할 수 있는 오브젝트라면 사용한다.
                tryUse = false; // 1회용으로 변수 사용하기 위해 
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log($"{other.name} : 무엇인가가 나갔다.");
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
        }
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector3>());
        inputDir = context.ReadValue<Vector2>();    // Vector2.x = a키(-1) d키(+1),  Vector2.y = w키(+1) s키(-1)
        
        anim.SetBool("IsMove", !context.canceled);
    }

    private void OnSideMoveInput(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<float>());
        inputSide = context.ReadValue<float>();

        anim.SetBool("IsMove", !context.canceled);
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        Jump();
    }

    private void OnUseInput(InputAction.CallbackContext context)
    {
        Use();
    }

    public void Die()
    {
        actions.Player.Disable();
        rigid.constraints = RigidbodyConstraints.None;
        rigid.AddForce(Random.insideUnitSphere * 10.0f);
        anim.SetTrigger("Die");
        StartCoroutine(Restart());  // 죽을 때 씬 재시작 코루틴 실행
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(5.0f);  // 5초 기다리기
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   // 현재 씬 다시 불러오기
    }
}
