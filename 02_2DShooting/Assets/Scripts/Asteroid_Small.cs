using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_Small : MonoBehaviour
{
    public float speed = 3.0f;
    public int score = 1;

    private void Awake()        // 게임 오브젝트가 완성된 직후에 호출
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, 4);
        renderer.flipX = ((rand & 0b_01) != 0);
        renderer.flipY = ((rand & 0b_10) != 0);

        // & 연산자 : 결과는 int
        // != 연산자 : 결과는 bool
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);   // 그냥 위로 올라가기
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameManager.Inst.Score += score;
        }

        Destroy(this.gameObject);
    }
}