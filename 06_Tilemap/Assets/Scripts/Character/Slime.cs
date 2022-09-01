using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime : MonoBehaviour
{
    public float RewardLife => 2.0f;    // 이 몬스터를 잡았을 때 얻을 수 있는 수명(2초)

    public float moveSpeed = 2.0f;  // 이동 속도
    public bool showPath = true;    // 이동 경로 표시 여부
    List<Vector2Int> path;          // 이동 할 경로
    
    private float pathWaitTime = 0.0f;      // 길막으로 현재 기다린 시간
    private const float MaxWaitTime = 1.0f; // 길막으로 최대 기다리는 시간

    private float dissolveTime = 2.0f;  // 죽을 때 dissolve 되는 시간
    private bool isDead = false;        // 죽었는지 살았는지 표시(죽으면 true)

    SubMapManager subMapManager;        // 단순 유틸리티 용도

    public System.Action<Slime> onDead; // 사망시 실행될 델리게이트
    
    public Vector2Int Position => subMapManager.WorldToGrid(transform.position);  // 슬라임의 그리드 좌표

    Material mainMat;               // 쉐이더 처리 때문에 가지고 있는 머티리얼

    public LineRenderer linePrefab; // 경로 표시용 프리팹(없을 때 만들기 위한 용도)
    LineRenderer line;              // 실제 경로 표시용 라인 랜더러

    private void Awake()
    {
        path = new List<Vector2Int>();  // 움직일 경로

        // 셰이더 프로퍼티 설정용
        Renderer renderer = GetComponent<SpriteRenderer>();
        mainMat = renderer.material;
        mainMat.SetColor("_Color", Color.red * 5);  
        mainMat.SetFloat("_Tickness", 0);
    }

    /// <summary>
    /// subMapManager 설정을 위한 초기화
    /// </summary>
    /// <param name="parent">이 슬라임이 생성된 서브맵(매니저)</param>
    public void Initialize(SubMapManager submap)
    {
        subMapManager = submap;
    }

    /// <summary>
    /// 아웃라인 표시 on/off
    /// </summary>
    /// <param name="on">true면 아웃라인 표시, false면 끄기</param>
    public void OutlineOnOff(bool on)
    {
        if( on )
            mainMat.SetFloat("_Tickness", 0.005f);
        else
            mainMat.SetFloat("_Tickness", 0.0f);
    }

    /// <summary>
    /// 이동할 위치 지정
    /// </summary>
    /// <param name="target">이동할 위치</param>
    public void SetMoveDestination(Vector2Int target)
    {
        path.Clear();   // 이전 경로 지우기

        path = AStar.PathFind(subMapManager.GridMap, Position, target);  // 경로 찾기
        //path = AStar.PathFind(spawner.GridMap, Position, new(-10,-10));
        DrawPath();     // 경로 그리기
    }

    /// <summary>
    /// 슬라임의 실제 이동처리
    /// </summary>
    public void MoveUpdate()
    {
        if (!isDead)
        {
            // 경로에 따른 이동 처리
            if (path.Count > 0 && pathWaitTime < MaxWaitTime)    // 경로에 남은 노드가 있고 오래 기다리지 않았으면 이동처리
            {
                if (!subMapManager.IsMonsterThere(path[0])
                    || (Position == path[0] && subMapManager.IsMonsterThere(path[0])))  // 내가 아닌 다른 몬스터가 길을 막고 있으면 스킵
                {
                    Vector3 targetPos = subMapManager.GridToWorld(path[0]);  // 남은 경로의 첫번째 위치 가져오기
                    Vector3 dir = targetPos - transform.position;   // 방향 계산하기
                    if (dir.sqrMagnitude < 0.001f) // 목표지점에 도착했는지 확인
                    {
                        path.RemoveAt(0);   // 목표지점에 도착했으면 경로의 첫번째 노드 삭제
                    }
                    transform.Translate(Time.deltaTime * moveSpeed * dir.normalized);   // 실제 이동하기            
                    pathWaitTime = 0.0f;            // 이동했으니까 대기시간 초기화
                }
                else
                {
                    pathWaitTime += Time.deltaTime; // 대기시간 누적하기
                }
            }
            else
            {
                // 최종 위치 도착
                if (line != null)
                {
                    line.gameObject.SetActive(false);       // 라인렌더러 비활성화
                }
                int failCount = 0;  // 다음 위치 찾기 실패 회수
                do // 갈 수 없는 지역을 선택했을 때의 대비 용
                {
                    SetMoveDestination(subMapManager.RandomMovablePotion());    // 다음 위치 구하기
                    failCount++;    // 찾을 때마다 실패 회수 기록
                }
                while (path.Count <= 0 && failCount < 100);     // 경로가 안나오고 실패가 100번 미만이면 다시 시도
                pathWaitTime = 0.0f;                            // 새 경로를 만들어졌으면 대기시간 초기화
            }
        }
    }

    /// <summary>
    /// 사망용 함수
    /// </summary>
    public void Die()
    {
        if (!isDead)
        {
            isDead = true;                  // 죽었다고 표시
            GetComponent<BoxCollider2D>().enabled = false;
            if (line != null)
            {
                Destroy(line.gameObject);   // 라인랜더러 삭제
            }
            path.Clear();                   // 기존에 존재하던 경로 삭제

            StartCoroutine(DieProcess());
        }
    }       

    /// <summary>
    /// 사망용 이팩트 처리
    /// </summary>
    /// <returns></returns>
    IEnumerator DieProcess()
    {
        float timeElipsed = 0.0f;       // dissolve 진행 시간
        float dissolveNomalize = 1 / dissolveTime;  // dissolve의 최대값은 1이 되어야 하므로 변형해주는 작업

        while (timeElipsed < dissolveTime)
        {
            timeElipsed += Time.deltaTime;  // 시간 누적하고
            mainMat.SetFloat("_DissolveFade", 1 - (timeElipsed * dissolveNomalize));    // 1-누적시간으로 dissolve처리
            yield return null;              // 다음 프레임까지 대기(매 프레임 진행)
        }

        onDead?.Invoke(this);           // 사망때 실행되어야 할 것들 실행
        Destroy(this.gameObject, 0.1f); // 실제 자신의 오브젝트 삭제
    }


    /// <summary>
    /// 길찾기 경로 표시용 함수
    /// </summary>
    /// <param name="path">길찾기로 찾은 경로</param>
    void DrawPath()
    {
        if(showPath && path != null && path.Count > 1)  // 최소 2개의 위치는 필요함
        {
            if (line == null)
            {
                line = Instantiate(linePrefab, subMapManager.transform);    // 라인 랜더러가 없으면 생성
            }
            line.gameObject.SetActive(true);    // 라인랜더러 겨코
            line.positionCount = path.Count;    // 점 갯수 설정
            int index = 0;
            foreach (var pos in path)
            {
                Vector2 worldPos = subMapManager.GridToWorld(pos);          // 그리드 좌표를 월드좌표로 변경
                line.SetPosition(index, new(worldPos.x - subMapManager.transform.position.x, 
                    worldPos.y - subMapManager.transform.position.y, 1));   // 경로를 이용해서 라인랜더러가 그려질 위치 결정
                index++;
            }
        }
        else
        {
            // 그려질 선이 없으면 비활성화
            if(line != null)
            {
                line.gameObject.SetActive(false);
            }
        }
    }
}
