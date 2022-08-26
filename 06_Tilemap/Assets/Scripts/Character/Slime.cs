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

    Spawner spawner;

    public System.Action<Slime> onDead;
    
    public Vector2Int Position => spawner.WorldToGrid(transform.position);

    Material mainMat;

    public LineRenderer linePrefab;
    LineRenderer line;

    private void Awake()
    {
        path = new List<Vector2Int>();

        Renderer renderer = GetComponent<SpriteRenderer>();
        mainMat = renderer.material;
        mainMat.SetColor("_Color", Color.red * 5);
        mainMat.SetFloat("_Tickness", 0);
    }

    public void Initialize(Spawner parent)
    {
        spawner = parent;
    }

    public void OutlineOnOff(bool on)
    {
        if( on )
            mainMat.SetFloat("_Tickness", 0.005f);
        else
            mainMat.SetFloat("_Tickness", 0.0f);
    }

    public void Move(Vector2Int target)
    {
        path.Clear();   // 이전 경로 지우기

        path = AStar.PathFind(spawner.GridMap, Position, target);  // 경로 찾기
        //path = AStar.PathFind(spawner.GridMap, Position, new(-10,-10));
        DrawPath();
    }

    private void Update()
    {
        //if (Keyboard.current.digit1Key.wasPressedThisFrame)
        //{
        //    OutlineOnOff(true);
        //}
        //if (Keyboard.current.digit2Key.wasPressedThisFrame)
        //{
        //    OutlineOnOff(false);
        //}
        if( path.Count > 0 )    // 경로에 남은 노드가 있으면
        {
            Vector3 targetPos = spawner.GridToWorld(path[0]);  // 남은 경로의 첫번째 위치 가져오기
            Vector3 dir = targetPos - transform.position;   // 방향 계산하기
            if( dir.sqrMagnitude < 0.001f ) // 목표지점에 도착했는지 확인
            {
                path.RemoveAt(0);   // 목표지점에 도착했으면 경로의 첫번째 노드 삭제
            }
            transform.Translate(Time.deltaTime * moveSpeed * dir.normalized);   // 실제 이동하기            
        }
        else
        {
            // 최종 위치 도착
            if (line != null)
            {
                line.gameObject.SetActive(false);
            }

            do
            {                
                Move(spawner.RandomMovablePotion());                
            }
            while (path.Count <= 0);
        }
    }

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
                line = Instantiate(linePrefab);
            }
            line.gameObject.SetActive(true);
            line.positionCount = path.Count;
            int index = 0;
            foreach (var pos in path)
            {
                Vector2 worldPos = spawner.GridToWorld(pos);
                line.SetPosition(index, new(worldPos.x, worldPos.y, 1));
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
