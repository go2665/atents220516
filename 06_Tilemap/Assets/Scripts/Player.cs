using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float speed = 3.0f;

    // 시야범위
    public float sightRange = 3.0f;
    public float sightAngle = 90.0f;
    Slime seenSlime;

    /// <summary>
    /// 미리 캐싱해놓을 컴포넌트들
    /// </summary>
    Rigidbody2D rigid;
    Animator anim;
    
    /// <summary>
    /// 인풋 액션 클래스
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 마지막으로 입력 된 방향
    /// </summary>
    Vector2 dir;
    Vector2 oldDir = Vector2.down;


    Vector2Int currentMap = Vector2Int.one;     // 플레이어가 존재하는 맵의 번호
    Vector2 mapSize = new Vector2(20, 20);      // 맵 하나의 크기
    Vector2Int mapCount = new Vector2Int(3, 3); // 맵의 갯수(가로,세로)
    Vector2 offset = Vector2.zero;
    public Vector2Int CurrentMap
    {
        set
        {
            if(currentMap != value)
            {
                currentMap = value;
                Debug.Log($"현재 맵의 위치 : {currentMap}");
            }
        }
    }


    Light2D spotLight;

    private void Awake()
    {
        // 컴포넌트 가져오고
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spotLight = transform.GetChild(1).GetComponent<Light2D>();

        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        circle.radius = sightRange;

        // 인풋 액션 만들기
        inputActions = new PlayerInputActions();

        // 월드 원점에서 맵이 얼마나 이동해 있는가? => offset으로 저장
        offset = new Vector2(mapSize.x * mapCount.x * -0.5f, mapSize.y * mapCount.y * -0.5f);
    }

    private void OnEnable()
    {
        // 인풋 액션과 함수 연결
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        inputActions.Player.Attack.performed += OnAttack;
        
        // 인풋 액션의 Player 액션맵 활성화
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        // 연결했던 것 해제
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Move.canceled -= OnStop;
        inputActions.Player.Move.performed -= OnMove;

        // 액션맵 비활성화
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        // 입력 받은 방향에 따라 리지드바드를 이용해 움직이기
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * speed * dir);

        Vector2 pos = (Vector2)transform.position - offset; // 맵의 왼쪽 아래가 원점이라고 가정했을 때 나의 위치
        CurrentMap = new Vector2Int((int)(pos.x / mapSize.x), (int)(pos.y / mapSize.y));    // 위치를 맵 하나의 크기로 나누어서 (,)맵인지 계산
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        anim.SetTrigger("Attack");
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // dir 갱신
        dir = context.ReadValue<Vector2>();
        List<ContactPoint2D> temp = new List<ContactPoint2D>();
        if(rigid.GetContacts(temp) == 0 )   // 충돌한 다른 물체가 없으면 노멀라이즈
        {
            dir = dir.normalized;
        }

        oldDir = dir;

        // 바라보는 방향으로 스포트라이트 회전시키기
        //float angle = Vector3.SignedAngle(Vector3.up, (Vector3)dir, Vector3.forward);
        //spotLight.transform.rotation = Quaternion.Euler(0, 0, angle);
        spotLight.transform.up = dir;

        // 애니메이터의 파라메터도 갱신
        anim.SetBool("IsMove", true);   // Move로 트랜지션이 일어나게 만들기
        anim.SetFloat("InputX", dir.x); // 방향도 기록
        anim.SetFloat("InputY", dir.y);
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        //dir = context.ReadValue<Vector2>();
        oldDir = dir;
        dir = Vector2.zero;             // 방향을 제거
        anim.SetBool("IsMove", false);  // Idle로 트랜지션이 일어나게 만들기
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {            
            if(IsInSight(collision.transform.position))
            {
                Debug.Log("적이 보인다!");
                seenSlime = collision.gameObject.GetComponent<Slime>();
                seenSlime.OutlineOnOff(true);
            }  
            else
            {
                seenSlime?.OutlineOnOff(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (IsInSight(collision.transform.position))
            {
                Debug.Log("적이 계속 보인다!");
                seenSlime = collision.gameObject.GetComponent<Slime>();
                seenSlime?.OutlineOnOff(true);
            }
            else
            {
                seenSlime?.OutlineOnOff(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            seenSlime?.OutlineOnOff(false);
            seenSlime = null;
        }
    }

    bool IsInSight(Vector3 targetPos)
    {
        float angle = Vector2.Angle(oldDir, (targetPos - transform.position));
        return angle <= sightAngle * 0.5f;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.forward, sightRange);
        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, transform.forward, 
            Quaternion.Euler(0, 0, -sightAngle*0.5f) * oldDir, sightAngle, sightRange, 3f);
        Handles.DrawLine(transform.position, 
            transform.position + Quaternion.Euler(0, 0, -sightAngle * 0.5f) * oldDir * sightRange, 3.0f);
        Handles.DrawLine(transform.position, 
            transform.position + Quaternion.Euler(0, 0, sightAngle * 0.5f) * oldDir * sightRange, 3.0f);
    }
#endif
}
