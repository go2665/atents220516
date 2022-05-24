using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // public 변수는 인스펙터 창에서 확인 할 수 있다.
    public float moveSpeed = 2.0f;

    // Start is called before the first frame update => 게임이 시작되었을 때 Start가 호출됩니다.
    //void Start()
    //{        
    //}

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

        // transform.position = transform.position + new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
        // transform.position = transform.position + Vector3.right * moveSpeed * Time.deltaTime;
        if( Keyboard.current.aKey.ReadValue() > 0.0f )
        {
            Debug.Log($"A 키 눌렀습니다.");
        }
        if (Keyboard.current.sKey.ReadValue() > 0.0f)
        {
            Debug.Log($"S 키 눌렀습니다.");
        }
    }
}
