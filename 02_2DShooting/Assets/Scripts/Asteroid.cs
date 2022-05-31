using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int splitCount = 3;          // 쪼개질 개수
    public float lifeTime = 0.2f;       // 자연사 시간
    public GameObject small = null;     // 쪼개질 때 생길 작은 운석
    public int hitPoint = 3;            // HP(총알 버티는 수)


    // 뭔가 계속하는 것은 Update 계열의 함수에서 실행
    private void Update()
    {
        // 오일러 앵글 : 3차원 회전을 x축 y축 z축의 합으로 나타낸 것
        // 예시 : ( 10, 20, 30 ) => x축으로 10도, y축으로 20도, z축으로 30도
        // 오일러 앵글의 문제점 : 짐벌락이라는 현상이 발생
        // 짐벌락 해결을 위해 쿼터니언(Quaternion, 사원수) 등장( 오일러 앵글에 비해 속도도 빠르고 메모리도 덜 차지함)
        // 대신 쿼터니언은 사람이 알아보거나 만들기 어려움

        //transform.rotation *= Quaternion.Euler(0, 0, 30.0f * Time.deltaTime);   // 1초에 30도씩 돌려라(반시계방향)
        transform.Rotate(0, 0, 30.0f * Time.deltaTime);
        //transform.Rotate(new Vector3(0, 0, 30.0f * Time.deltaTime));

        lifeTime -= Time.deltaTime; // 수명 감소
        if( lifeTime < 0.0f )
        {
            Crush();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 피직스 레이어 설정으로 적들끼리는 부딪치지 안게 만들어서 체크할 필요 없음.
        //if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Player"))
        //{
            hitPoint--;
            if (hitPoint < 1)
            {
                Crush();
            }
        //}
    }

    private void Crush()
    {
        // 터질 때가 되었다.
        float angle = 360.0f / (float)splitCount;       // 사이 각도 구하기
        for (int i = 0; i < splitCount; i++)               // 쪼개질 개수만큼 반복
        {
            GameObject obj = Instantiate(small);        // 작은 운석 만들고
            obj.transform.position = transform.position;    // 기준위치(큰운석)로 일단 이돌
            obj.transform.Rotate(0, 0, angle * i);          // 계산한 사이 각도만큼 회전
            //obj.transform.position += (obj.transform.up * 2);   // 서로 떨어진체로 시작하고 싶을 때
        }
        Destroy(this.gameObject);   // 다 만들고 나면 큰 운석 죽이기
    }

}
