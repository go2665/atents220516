using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 몬스터 자동 생성용 클래스
/// </summary>
public class Spawner : MonoBehaviour
{
    public GameObject monsterPrefab;    // 생성할 몬스터의 프리팹
    public int maxSpawn = 1;            // 이 스포너에서 동시한 유지 가능한 최대 몬스터 수
    public float spawnDelay = 1.0f;     // 몬스터 생성 간격

    public Vector2 spawnArea;           // 스폰 영역의 크기(원점은 tranform의 position)

    int currentSpawn = 0;               // 현재 생성된 몬스터 수
    float delayCount = 0.0f;            // 생성용 시간 카운터(spawnDelay보다 커지면 몬스터 생성)

    public System.Action onRequestSpawn;   // 서브맵 메니저에게 스폰 요청을 보내는 델리게이트
    
    /// <summary>
    /// 몬스터를 생성하는 함수. 내부에서 직접 호출 할 일은 없음.
    /// </summary>
    /// <returns>생성한 몬스터</returns>
    public Slime Spawn()
    {
        currentSpawn++; // 생성 카운팅용 숫자 증가
        GameObject obj = Instantiate(monsterPrefab, transform);
        Slime slime = obj.GetComponent<Slime>();
        slime.onDead += MonsterDead;

        return slime;
    }

    /// <summary>
    /// 몬스터 사망시 호출될 함수
    /// </summary>
    /// <param name="_">사용안함</param>
    private void MonsterDead(Slime _)
    {
        currentSpawn--; // 생성 카운팅용 숫자 감소
    }

    /// <summary>
    /// SubmapManager에게 생성 요청을 위한 함수
    /// </summary>
    private void RequestSpawn()
    {
        onRequestSpawn?.Invoke();
    }

    private void Update()
    {
        if (currentSpawn < maxSpawn)        // 최대 스폰 수보다 생성되어 있는 슬라임의 수가 적으면
        {
            delayCount += Time.deltaTime;   // 카운트다운
            if (delayCount > spawnDelay)    // 원래 딜레이시간보다 커지면
            {
                RequestSpawn();             // SubmapManager에게 생성 요청
                delayCount = 0.0f;          // 딜레이용 카운트 다운 초기화
            }
        }
    }

    private void OnDrawGizmos()
    {
        // 스폰 위치 씬창에 그리기
        Vector3 pos = transform.position;
        pos.x = Mathf.Floor(pos.x);     // 그리드 칸 맞추기 위해 소수점 내림
        pos.y = Mathf.Floor(pos.y);

        Vector3 p0 = pos;
        Vector3 p1 = pos + new Vector3(spawnArea.x, 0);
        Vector3 p2 = pos + new Vector3(spawnArea.x, spawnArea.y);
        Vector3 p3 = pos + new Vector3(0, spawnArea.y);

        Handles.color = Color.red;
        Handles.DrawLine(p0, p1, 5.0f);
        Handles.DrawLine(p1, p2, 5.0f);
        Handles.DrawLine(p2, p3, 5.0f);
        Handles.DrawLine(p3, p0, 5.0f);        
    }
}
