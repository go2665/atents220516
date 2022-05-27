using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RequireComponent를 사용하면 이 스크립트를 가진 게임 오브젝트가 해당 컴포넌트가 없을 경우 자동으로 추가해준다.
[RequireComponent(typeof(Rigidbody2D))] 
public class Shoot : MonoBehaviour
{
    public float lifeTime = 3.0f;   // 총알의 수명
    public float speed = 10.0f;
    Rigidbody2D rigid = null;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 백터 : 힘의 방향과 크기
        rigid.velocity = transform.right * speed;

        Destroy(this.gameObject, lifeTime);     // lifeTime초 후에 게임 오브젝트를 삭제한다.
    }

    // Update 함수를 이용해 일정 시간 후에 삭제하기
    //private void Update()
    //{
    //    // Time.deltaTime;  // 이전 업데이트 함수가 호출되고 현제 업데이트 함수가 호출될 때까지의 시간                
    //    lifeTime -= Time.deltaTime;
    //    if( lifeTime < 0.0f )
    //    {
    //        //죽으면 된다.
    //        Destroy(this.gameObject);
    //    }
    //}
}
