using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int splitCount = 3;          // 쪼개질 개수
    public float lifeTime = 0.2f;       // 자연사 시간
    public GameObject small = null;     // 쪼개질 때 생길 작은 운석(prefab)
    public int hitPoint = 3;            // HP(총알 버티는 수)
    public float moveSpeed = 1.0f;      // 이동속도
    public int score = 10;

    public Vector3 targetDir = Vector3.zero;

    private void Awake()        // 게임 오브젝트가 완성된 직후에 호출
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, 4);
        renderer.flipX = ((rand & 0b_01) != 0);
        renderer.flipY = ((rand & 0b_10) != 0);
    }

    //private void OnEnable()     // 게임 오브젝트가 활성화 될 때 호출
    //{
        
    //}

    //private void Start()        // 첫번째 업데이트 직전
    //{
        
    //}

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
        transform.Translate(targetDir * moveSpeed * Time.deltaTime, Space.World);    //43, 44 똑같다.
        //transform.position += (Vector3)(Vector2.left * moveSpeed * Time.deltaTime);


        //움직임
        // transform을 이용해서 움직이는 것 : 물리같은 것 고려없음. 그냥 무조건 지정된 위치로 위치 변경(텔레포트)
        //   - position으로 움직이는 것
        //   - Translate로 움직이는 것
        // Rigidbody를 이용해서 움직이는 것 : 물리 고려(운동량, 속도, 질량). MovePosition(텔레포트는 텔레포트인데 물체의 충돌영역은 고려함)

        // 월드 좌표계(World Coordinate System)
        //  - 월드(맵, 씬)의 원점을 기준으로 만들어진 좌표계
        // 로컬 좌표계(Local Coordinate System)
        //  - 각 오브젝트의 중심점(Pivot)을 기준으로 만들어진 좌표계

        // 백터 : 힘의 방향과 크기(스칼라)를 나타내는 개념
        // 3D 백터 (x, y, z)
        // 2D 백터 (x, y)
        // 백터와 위치는 완전히 다른 개념
        // 백터는 위치와 전혀 상관없다.
        // 백터의 더하기 (1, 2, 3) + ( 2, -1, 3) = (3, 1, 6)
        // 백터의 빼기 (1, 2, 3) - ( 2, -1, 3) = (-1, 3, 0)
        // 목표 위치 - 출발점 위치 = 출발점에서 목표지점으로 향하는 방향 백터     (무조건 외울 것)

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
            if (collision.gameObject.CompareTag("Bullet"))
            {
                GameManager.Inst.Score += score;
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        // 현재 운석의 위치에서 목표 방향으로 1.5만큼 진행한 지점까지 선 그리기
        Gizmos.DrawLine(transform.position, transform.position + targetDir * 1.5f);  
    }
}
