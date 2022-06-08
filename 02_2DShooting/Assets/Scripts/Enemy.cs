using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f;
    public GameObject explosion = null;
    public int score = 5;

    private void Update()
    {
        transform.Translate(-transform.right * speed * Time.deltaTime); // 계속 자신의 왼쪽 방향으로 날아간다.
    }

    // 이 스크립트를 가지고 있는 게임 오브젝트의 컬라이더가 다른 트리거 안에 들어가야 실행
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    ////if (collision.tag == "KillZone") ;    // 매우 좋지 않음
    //    //if (collision.CompareTag("KillZone"))  // 해시 (Hash) -> 유니크한 요약본을 만들어준다.
    //    //{
    //    //    // 킬존에 들어갔다.
    //    //    Destroy(this.gameObject);
    //    //}
    //}

    // 이 스크립트를 가지고 있는 게임 오브젝트의 컬라이더가 다른 컬라이더와 부딪쳐야 실행
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameManager.Inst.Score += score;
        }

        explosion.transform.parent = null;  // explotion이 부모가 없도록 만든다.
        explosion.SetActive(true);
        Destroy(this.gameObject);
    }
}
