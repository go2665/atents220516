using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

// 슬라임은 생성되면 자기 씬 내의 랜덤한 지역으로 이동한다.
// 슬라임 옆칸에 플레이어가 있으면 이동을 멈추고 공격한다.
// 슬라임 옆칸에 플레이어가 없으면 다시 랜덤한 지역으로 이동한다.

public class Slime : MonoBehaviour
{
    public float moveSpeed = 2.0f;  // 이동 속도
    public bool showPath = true;    // 이동 경로 표시 여부
    List<Vector2Int> path;          // 이동 할 경로
    
    private float pathWaitTime = 0.0f;      // 길막으로 현재 기다린 시간
    private const float MaxWaitTime = 5.0f; // 길막으로 최대 기다리는 시간

    //Spawner spawner;                // 다른 클래스에 접근 할 용도로 가지고 있음
    SubMapManager subMapManager;    // 단순 유틸리티 용도

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

    public void MoveUpdate()
    {
        // 경로에 따른 이동 처리
        if (path.Count > 0 && pathWaitTime < MaxWaitTime)    // 경로에 남은 노드가 있으면 이동처리
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
                pathWaitTime = 0.0f;
            }
            else
            {
                pathWaitTime += Time.deltaTime;
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
            do
            {
                SetMoveDestination(subMapManager.RandomMovablePotion());    // 다음 위치 구하기
                failCount++;    
            }
            while (path.Count <= 0 && failCount < 100);    // 갈 수 없는 지역을 선택했을 때의 대비용
        }
    }

    //private void Update()
    //{
    //    //if (Keyboard.current.digit1Key.wasPressedThisFrame)
    //    //{
    //    //    OutlineOnOff(true);
    //    //}
    //    //if (Keyboard.current.digit2Key.wasPressedThisFrame)
    //    //{
    //    //    OutlineOnOff(false);
    //    //}
    //    // 경로에 따른 이동 처리
    //    if( path.Count > 0 )    // 경로에 남은 노드가 있으면
    //    {
    //        Vector3 targetPos = subMapManager.GridToWorld(path[0]);  // 남은 경로의 첫번째 위치 가져오기
    //        Vector3 dir = targetPos - transform.position;   // 방향 계산하기
    //        if( dir.sqrMagnitude < 0.001f ) // 목표지점에 도착했는지 확인
    //        {
    //            path.RemoveAt(0);   // 목표지점에 도착했으면 경로의 첫번째 노드 삭제
    //        }
    //        transform.Translate(Time.deltaTime * moveSpeed * dir.normalized);   // 실제 이동하기            
    //    }
    //    else
    //    {
    //        // 최종 위치 도착
    //        if (line != null)
    //        {
    //            line.gameObject.SetActive(false);       // 라인렌더러 비활성화
    //        }

    //        do
    //        {                
    //            Move(subMapManager.RandomMovablePotion());    // 다음 위치 구하기
    //        }
    //        while (path.Count <= 0);    // 갈 수 없는 지역을 선택했을 때의 대비용
    //    }
    //}

    /// <summary>
    /// 사망처리 함수
    /// </summary>
    void Dead()
    {
        onDead?.Invoke(this);
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
                line = Instantiate(linePrefab); // 라인 랜더러가 없으면 생성
            }
            line.gameObject.SetActive(true);
            line.positionCount = path.Count;
            int index = 0;
            foreach (var pos in path)
            {
                Vector2 worldPos = subMapManager.GridToWorld(pos);
                line.SetPosition(index, new(worldPos.x, worldPos.y, 1));    // 경로를 이용해서 라인랜더러가 그려질 위치 결정
                index++;
            }
        }
        else
        {
            if(line != null)
            {
                line.gameObject.SetActive(false);
            }
        }
    }
}
