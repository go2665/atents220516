using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    public GameObject enemy = null;     // 생성할 적
    public float spawnInterval = 1.0f;  // 생성 간격
    public float randomRange = 8.0f;    // 높이 랜덤 범위
    public Color myGizmoColor = Color.white;

    protected WaitForSeconds waitSecond = null;   // 코루틴에서 사용할 일정 시간 대기

    private void Start()
    {
        waitSecond = new WaitForSeconds(spawnInterval);     //new WaitForSeconds를 재활용하기 위해 변수에 저장
        StartCoroutine(Spawn());        // 코루틴 시작
    }

    protected virtual IEnumerator Spawn()
    {
        while(true) // 무한 반복
        {
            yield return waitSecond;    // 지정된 시간만큼 대기
            GameObject obj = Instantiate(enemy);    // 적 생성
            obj.transform.position = this.transform.position;   // 적 초기 위치 설정
            obj.transform.Translate(Vector3.up * Random.Range(0.0f, randomRange));  // 적을 랜덤한 높이만큼 올리기
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = myGizmoColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * randomRange);
    }
}
