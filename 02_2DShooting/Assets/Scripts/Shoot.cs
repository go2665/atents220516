using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RequireComponent를 사용하면 이 스크립트를 가진 게임 오브젝트가 해당 컴포넌트가 없을 경우 자동으로 추가해준다.
[RequireComponent(typeof(Rigidbody2D))] 
public class Shoot : MonoBehaviour
{
    public GameObject spark = null;
    public float lifeTime = 3.0f;   // 총알의 수명
    public float speed = 10.0f;

    bool isOnDestroy = false;       // OnCollisionEnter2D 중복실행 방지용
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
    //    // Time.deltaTime;  // 이전 업데이트 함수가 호출되고 현재 업데이트 함수가 호출될 때까지의 시간                
    //    lifeTime -= Time.deltaTime;
    //    if( lifeTime < 0.0f )
    //    {
    //        //죽으면 된다.
    //        Destroy(this.gameObject);
    //    }
    //}

    
    /// <summary>
    /// 서로 충돌했을 때 실행되는 함수(이 스크립트를 가지고 있는 게임 오브젝트의 컬라이더에 다른 컬라이더가 충돌했을 때 실행)
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isOnDestroy)   // 중복해서 OnCollisionEnter2D가 실행되는 것을 방지
        {
            isOnDestroy = true;
            //Debug.Log($"OnCollisionEnter2D : {collision.gameObject.name}");

            if (collision.gameObject.CompareTag("Enemy"))
            {
                spark.transform.parent = null;
                spark.transform.position = collision.contacts[0].point;     // 부딪친 위치
                // spark.transform.position = collision.contacts[0].normal; // 부딪친 면의 노멀 벡터  
                // 노멀벡터 : 특정 평면에 수직인 벡터. 외적을 통해 구할 수 있다.
                // 노멀벡터를 이용해 반사를 계산할 수 있다. => 빛과 그림자. 물리 반사 등을 계산하는데 필수.
                // 반사된 벡터 = Vector3.Reflect(진입벡터, 노멀벡터);
                spark.SetActive(true);
            }
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 서로 겹쳤을 때 실행되는 함수(이 스크립트를 가지고 있는 게임 오브젝트의 컬라이더가 다른 트리거에 들어갔을 때 실행)
    /// </summary>
    /// <param name="collision"></param>
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //Debug.Log($"OnTriggerEnter2D : {collision.gameObject.name}");
    //    //if (collision.tag == "KillZone") ;    // 매우 좋지 않음
    //    //if( collision.CompareTag("KillZone") )  // 해시 (Hash) -> 유니크한 요약본을 만들어준다.
    //    //{
    //    //    // 킬존에 들어갔다.
    //    //    Destroy(this.gameObject);
    //    //}
    //}
}
