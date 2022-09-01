using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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

    /// <summary>
    /// 공격 범위(컬라이더 가지고 안에 들어온 슬라임 죽임)
    /// </summary>
    AttackArea attackArea;

    // 수명 처리용 변수들
    public Slider lifeTimeSlider;       // 수명 게이지 표시용 슬라이더
    public Text lifeTimeText;           // 남은 수명 시간 표시용.
    const float MaxLifeTime = 10.0f;    // 플레이어의 최대 수명
    float lifeTime;                     // 플레이어의 현재 수명
    float totalLifeTime;                // 플레이어가 이번판에 플레이한 전체 수명

    /// <summary>
    /// post process 볼륨에서 가져온 비네트(회면 외각 어둡게 처리하는 효과)
    /// </summary>
    Vignette vignette;

    /// <summary>
    /// 게임 오버 UI 스크립트
    /// </summary>
    GameOverUI gameOver;

    /// <summary>
    /// 사망 표시용(true면 사망, false면 살아있음)
    /// </summary>
    bool isDead = false;

    /// <summary>
    /// 수명 프로퍼티
    /// </summary>
    public float LifeTime
    {
        get => lifeTime;

        set
        {
            if(value < 0.0f && isDead == false) // 수명이 0이하이고 플레이어가 살아있을 때
            {
                Die();  // 사망처리
            }
            else
            {
                // 아직 살아있음
                lifeTime = Mathf.Clamp(value, 0.0f, MaxLifeTime);   // 수명을 0~최대치로 제한
                lifeTimeSlider.value = lifeTime;             // 슬라이더의 위치를 수명으로 조정
                lifeTimeText.text = $"{lifeTime:F2} 초";     // 수명을 글자로 출력

                // 비네트는 0이면 다 보이고 1이면 가운데만 둥글게 보인다.
                vignette.intensity.value = (MaxLifeTime - lifeTime) / MaxLifeTime;  // 수명이 최대치면 비네트로 가려지는 부분이 없다.
            }
        }
    }

    Vector2Int currentMap = Vector2Int.one;     // 플레이어가 존재하는 맵의 번호
    Vector2 mapSize = new Vector2(20, 20);      // 맵 하나의 크기
    Vector2Int mapCount = new Vector2Int(3, 3); // 맵의 갯수(가로,세로)
    Vector2 offset = Vector2.zero;              // 맵의 원점 조절용
    public Vector2Int CurrentMap
    {
        get => currentMap;
        set
        {
            if(currentMap != value)
            {
                currentMap = value;
                Debug.Log($"현재 맵의 위치 : {currentMap}");
                onMapChange?.Invoke(currentMap);    // 플레이어가 있는 맵 주변만 유지/로딩하고 나머지는 로딩 해제
            }
        }
    }
    public System.Action<Vector2Int> onMapChange;   // 플레이어가 존재하는 맵이 변경될 때 실행되는 델리게이트

    Light2D spotLight;  // 플레이어 주변을 비추는 빛(사용안함)
        
    private void Awake()
    {
        // 컴포넌트 가져오고
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spotLight = transform.GetChild(1).GetComponent<Light2D>();

        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        circle.radius = sightRange; // 시야 범위 만큼 서클 컬라이더 키우기(사용안함)

        attackArea = GetComponentInChildren<AttackArea>();
        // 공격 범위에 몬스터가 들어가 죽었을 때 실행되는 델리게이트. 수명 추가 람다함수 등록
        attackArea.onMonsterKill += (reward) => LifeTime += reward; 

        // 인풋 액션 만들기
        inputActions = new PlayerInputActions();

        // 월드 원점에서 맵이 얼마나 이동해 있는가? => offset으로 저장
        offset = new Vector2(mapSize.x * mapCount.x * -0.5f, mapSize.y * mapCount.y * -0.5f);
    }

    private void Start()
    {
        // 포스트 프로세스 볼륨에서 비네트 가져오기
        GameManager.Inst.PostProcessVolume.profile.TryGet<Vignette>(out vignette);

        // 수명 표시용 UI 가져오기
        if (lifeTimeSlider != null)
            lifeTimeSlider = FindObjectOfType<Slider>();
        if (lifeTimeText != null)
            lifeTimeText = FindObjectOfType<Text>();

        lifeTimeSlider.minValue = 0;            // 슬라이더의 min,max 수정해서 수명값을 그대로 사용하도록 처리
        lifeTimeSlider.maxValue = MaxLifeTime;
        lifeTimeSlider.value = MaxLifeTime;
        lifeTimeText.text = $"{MaxLifeTime:F2} 초";
        lifeTime = MaxLifeTime;                 // 현재 수명을 최대치와 맞추기

        gameOver = FindObjectOfType<GameOverUI>();  // 게임 오버용 UI 가져오기
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

    private void Update()
    {
        LifeTime -= Time.deltaTime;         // 현재 수명 감소시키기
        totalLifeTime += Time.deltaTime;    // 총 플레이 시간 증가시키기
    }

    private void FixedUpdate()
    {
        // 입력 받은 방향에 따라 리지드바드를 이용해 움직이기
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * speed * dir);

        Vector2 pos = (Vector2)transform.position - offset; // 맵의 왼쪽 아래가 원점이라고 가정했을 때 나의 위치
        CurrentMap = new Vector2Int((int)(pos.x / mapSize.x), (int)(pos.y / mapSize.y));    // 위치를 맵 하나의 크기로 나누어서 (,)맵인지 계산
    }

    // 시야범위 안에 적이 들어왔는지 확인하는 용도.(사용안함)
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.CompareTag("Enemy"))
        {
            if (IsInSight(collision.transform.position))
            {
                //Debug.Log("적이 보인다!");
                seenSlime = collision.gameObject.GetComponent<Slime>();
                seenSlime.OutlineOnOff(true);
            }
            else
            {
                seenSlime?.OutlineOnOff(false);
            }
        }
    }

    // 시야범위 안에 적이 있는지 확인하는 용도.(사용안함)
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (IsInSight(collision.transform.position))
            {
                //Debug.Log("적이 계속 보인다!");
                seenSlime = collision.gameObject.GetComponent<Slime>();
                seenSlime?.OutlineOnOff(true);
            }
            else
            {
                seenSlime?.OutlineOnOff(false);
            }
        }
    }

    // 시야범위 안에 적이 나갔는지 확인하는 용도.(사용안함)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            seenSlime?.OutlineOnOff(false);
            seenSlime = null;
        }
    }

    // 공격 입력 처리
    private void OnAttack(InputAction.CallbackContext _)
    {
        anim.SetTrigger("Attack");  // 애니메이션 재생하고 끝
    }

    // 이동 입력 처리
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

        attackArea.transform.localPosition = dir * 0.8f;    // 공격 범위 위치 변경하기

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

    // 시야 범위 안에 대상이 있는지 확인하는 함수
    bool IsInSight(Vector3 targetPos)
    {
        float angle = Vector2.Angle(oldDir, (targetPos - transform.position));
        return angle <= sightAngle * 0.5f;
    }

    // 사망 처리용
    void Die()
    {
        isDead = true;  // 죽었다고 표시하기
        gameOver.SetTotoalLifeTime(totalLifeTime);  // 이번판의 플레이 타임 넘겨주기
        gameOver.Show(true);    // 게임 오버 UI보여주기
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // 기즈모로 시야범위 표시
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
