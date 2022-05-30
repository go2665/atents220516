using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 값타입(Value type : 실제 값을 가지는 타입int, float, bool) 
    // 참조타입(Reference type : 각종 클래스들을 new한 것들을 담을 수 있는 타입, 메모리 주소 같은 것들을 저장하는 타입 )
    // null은 참조타입의 변수가 비어있다고 표시하는 키워드
    // var는 컴파일타임에 변수의 타입을 결정해주는 키워드이다.

    // public 변수는 인스펙터 창에서 확인 할 수 있다.
    public float moveSpeed = 2.0f;
    public GameObject shootPrefab = null;   // 프리팹은 GameObject 타입에 담을 수 있다.
    public Transform[] firePosition = null;

    private Vector3 direction = Vector3.zero;
    private Rigidbody2D rigid = null;           // 계속 사용할 컴포넌트는 한번만 찾는게 좋다.
    private float boostSpeed = 1.0f;

    private IEnumerator fireContinue = null;

    private void Awake()        // 게임 오브젝트가 만들어진 직후에 호출
    {
        rigid = GetComponent<Rigidbody2D>();

        // Unity는 안 움직이는 Collider는 하나로 합친 후 움직이는 Collider와 충돌처리를 계산한다.
        // Unity는 Rigidbody가 있은 오브젝트만 움직인 오브젝트로 판단한다.
        // Rigidbody가 없는 Collider가 움직이게 되면 다음 프레임에 다시 Collider를 합치기 때문에
        // 이런 동작이 반복되면 최적화에 심각한 악영향을 미친다.
    }

    // Start is called before the first frame update => 게임이 시작되었을 때 Start가 호출됩니다.(첫번째 Update가 실행되기 전에)
    void Start()
    {
        fireContinue = FireCoroutine();
    }

    // Update is called once per frame => 게임이 실행되는 도중에 주기적(매 프레임마다)으로 계속 호출된다.
    private void Update()
    {
        // Vector3 : float 타입의 x, y, z를 가지는 구조체

        // 벡터3의 더하기, 빼기
        // Vector3 a = new Vector3(1, 2, 3);        // a.x = 1, a.y = 2, a.z = 3
        // Vector3 b = new Vector3(10, 20,30);      // b.x = 10, b.y = 20, b.z = 30
        // a + b = ( 1+10, 2+20, 3+30 )
        // a - b = ( 1-10, 2-20, 3-30 )
        // a * 2 = ( 1*2, 2*2, 3*2 )

        // transform.position에 (1,0,0)을 매 프레임마다 더한다.
        // Time.deltaTime; //이전 프레임에서 지금 프레임까지 걸린 시간
        // Updata 함수안에서 (moveSpeed * Time.deltaTime)는 1초에 moveSpeed 만큼 이라는 의미가 된다.

        //----------------------------------------------------------------------------------------------------

        // transform.position = transform.position + new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
        // transform.position = transform.position + Vector3.right * moveSpeed * Time.deltaTime;

        //----------------------------------------------------------------------------------------------------

        //if ( Keyboard.current.wKey.ReadValue() > 0.0f )
        //{
        //    direction.y = 1.0f;
        //}
        //else if (Keyboard.current.sKey.ReadValue() > 0.0f)
        //{
        //    direction.y = -1.0f;
        //}
        //else
        //{
        //    direction.y = 0.0f;
        //}
        //if (Keyboard.current.aKey.ReadValue() > 0.0f)
        //{
        //    direction.x = -1.0f;
        //}
        //else if (Keyboard.current.dKey.ReadValue() > 0.0f)
        //{
        //    direction.x = 1.0f;
        //}
        //else
        //{
        //    direction.x = 0.0f;
        //}

        //// 백터의 정규화 : 백터의 크기를 1로 만드는 작업. 백터에서 순수하게 방향만 남기는 작업. 백터의 x,y,z를 백터의 길이로 나누면 된다.
        //// 단위 백터 : 백터를 정규화한 결과. 길이가 1인 백터
        //transform.position = transform.position + direction.normalized * moveSpeed * Time.deltaTime;

        //----------------------------------------------------------------------------------------------------
        
    }

    private void FixedUpdate()
    {
        // 물리 연산은 정확한 시간 간격으로 실행해야 정확한 결과가 나온다.
        // 그런데 Update함수는 항상 똑같은 시간 간격으로 실행되지 않는다. 
        //  => 일반 Update에서는 물리 연산에 오류가 있을 수 있다.
        // FixedUpdate는 설정에 지정되어 있는 일정한 시간간격으로 항상 호출된다.
        // Rigidbody를 가지는 오브젝트를 움직일(이동, 회전 등) 때는 FixedUpdate 안에서 해야 한다.
        Move();
    }

    private void Move()
    {
        // transform의 position 변경
        //transform.position += (direction * moveSpeed * Time.deltaTime);
        //transform.Translate(direction * moveSpeed * Time.deltaTime);
        //transform.Translate(1*Time.deltaTime, 0, 0);  // 계속 오른쪽으로 이동하는 코드

        // Rigidbody를 이용해서 이동        
        rigid.MovePosition(transform.position + (direction * moveSpeed * boostSpeed * Time.fixedDeltaTime));
    }

    public void OnMoveInput(InputAction.CallbackContext context) // context는 이 함수와 연결된 액션에서 전달된 입력관련 정보가 들어있다.
    {
        //if (context.started)
        //{
        //    //Debug.Log("Move!");
        //}

        direction = context.ReadValue<Vector2>();   // input action asset에 vector2로 지정되어 있다.
        //transform.position += (Vector3)direction; // (Vector3)direction; 타입 캐스팅. direction의 타입을 임시로 Vector3로 취급하는 것

        //Debug.Log(direction);
    }

    public void OnFireInput(InputAction.CallbackContext context)
    {
        //if (context.started)         // 키를 누르기 시작했을 때(키보드에서는 started와 performed의 차이가 없다)
        //{
        //    Debug.Log("Fire!!!!! - 시작");
        //}
        //else if (context.performed)  // 키를 완전히 눌렀을 때
        //{
        //    Debug.Log("Fire!!!!!! - 완전히 누름");
        //}
        //else if (context.canceled)  // 키를 땠을 때
        //{
        //    Debug.Log("Fire!!!!!!! - 키보드 땠음");
        //}

        if( context.started)
        {
            //GameObject obj = Instantiate(shootPrefab);      // 총알 생성
            //obj.transform.position = transform.position + transform.right * 1.2f;   // 플레이어 오른쪽으로 1.2만큼 떨어진 위치에 배치
            //obj.transform.rotation = transform.rotation;    //플레이어의 회전을 그대로 적용
            //Debug.Log(obj.name);

            //// 3줄로 발사하기
            //for (int i = 0; i < 3; i++)
            //{
            //    GameObject obj = Instantiate(shootPrefab);
            //    obj.transform.position = transform.position + transform.right * 1.2f + (transform.up * (1-i));  
            //    obj.transform.rotation = transform.rotation;
            //}

            //// 위치 표시용 빈 게임 오브젝트를 이용해 총알 발사(여러개 가능)
            //for (int i = 0; i < firePosition.Length; i++)
            //{
            //    GameObject obj = Instantiate(shootPrefab);
            //    obj.transform.position = firePosition[i].position;
            //    obj.transform.rotation = firePosition[i].rotation;
            //    //obj.transform.parent = null;    // obj의 부모를 제거하기
            //}
            StartCoroutine(fireContinue);
        }
        if( context.canceled )
        {
            StopCoroutine(fireContinue);
            //StopAllCoroutines();
        }
    }

    public void OnBoostInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            boostSpeed = 2.0f;
        }
        if(context.canceled)
        {
            boostSpeed = 1.0f;
        }
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < firePosition.Length; i++)
            {
                GameObject obj = Instantiate(shootPrefab);
                obj.transform.position = firePosition[i].position;
                obj.transform.rotation = firePosition[i].rotation;
                //obj.transform.parent = null;    // obj의 부모를 제거하기
            }

            yield return new WaitForSeconds(0.2f);  // 0.2초 대기
        }
    }
}