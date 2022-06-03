using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : EnemySpanwer
{
    public Transform target = null;
    public float targetLength = 10.0f;
    //public Color gizmoColor = Color.white;  // public으로 만들어서 인스팩터창에서 컬러 설정 가능하게 변경

    private void Awake()
    {
        //GameObject obj = GameObject.Find("이름");           // 이름으로 찾기. 가장 비효율적.
        //GameObject obj = GameObject.FindGameObjectWithTag("태그이름");    // 태그로 찾기. 이것도 씬 전체 확인
        //타입 obj = GameObject.FindObjectOfType<타입>();       // 타입으로 찾기. 씬 전체 확인

        target = transform.Find("Target");  //비효율적이지만 자식의 수는 보통 얼마되지 않기 때문에 그냥 사용하는 것
    }

    protected override IEnumerator Spawn()
    {
        while (true)
        {
            yield return waitSecond;
            GameObject obj = Instantiate(enemy);    // 적 생성
            obj.transform.position = this.transform.position;   // 적 초기 위치 설정
            obj.transform.Translate(Vector3.up * Random.Range(0.0f, randomRange));  // 적을 랜덤한 높이만큼 올리기

            // 도착방향을 랜덤으로 정하기

            // 타겟의 위치 + 위쪽방향으로 최대 targetLength까지 증가
            Vector3 toPosition = target.transform.position + Vector3.up * Random.Range(0.0f, targetLength);
            // 생성한 obj에서 Asteroid 컴포넌트 가져오기
            Asteroid asteroid = obj.GetComponent<Asteroid>();   
            // obj의 위치에서 toPosition로 가는 방향벡터 구하고 단위벡터로 변경해서 asteroid의 targetDir에 저장
            asteroid.targetDir = (toPosition - obj.transform.position).normalized;

            // 도착지점을 향하도록 회전하기
            // 왼쪽으로 가는 방향벡터와 obj의 위치에서 toPosition로 가는 방향벡터 사이각 구하기(2d니까 회전축을 Vector3.forward로 설정)
            //float angle = Vector3.SignedAngle(Vector3.left, toPosition - obj.transform.position, Vector3.forward);
            // 구해진 각도로 회전 만들기
            //obj.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = myGizmoColor;  // 입력받은 gizmoColor를 현재 기즈모의 색상으로 변경

        // 목표지점의 위치 ~ 목표지점의 위치 위쪽으로 targetLength 만큼 직선을 그리기
        Gizmos.DrawLine(target.position, target.position + Vector3.up * targetLength);
        // 오브젝트의 위치 ~ 오브젝트의 위치 위쪽으로 randomRange 만큼 직선을 그리기
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * randomRange);
    }
}

// public : 모두가 사용 가능
// private : 나만 사용 가능
// protected : 나랑 나를 상속받은 대상만 사용 가능