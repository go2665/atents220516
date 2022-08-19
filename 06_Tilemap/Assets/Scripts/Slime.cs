using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime : MonoBehaviour
{    
    public float moveSpeed = 2.0f;  // 이동 속도
    public bool showPath = true;    // 이동 경로 표시 여부
    List<Vector2Int> path;          // 이동 할 경로

    Material mainMat;
    private void Start()
    {
        Renderer renderer = GetComponent<SpriteRenderer>();
        mainMat = renderer.material;
        mainMat.SetColor("_Color", Color.red * 5);
        mainMat.SetFloat("_Tickness", 0);

        path = new List<Vector2Int>();
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
        path = AStar.PathFind(GameManager.Inst.Map, GameManager.Inst.WorldToGrid(transform.position), target);  // 경로 찾기
        if (showPath)
        {
            GameManager.Inst.DrawPath(path);    // 경로 그리기
        }
        else
        {
            GameManager.Inst.DrawPath(null);    // 경로를 지우기
        }
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
            Vector3 targetPos = GameManager.Inst.GridToWorld(path[0]);  // 남은 경로의 첫번째 위치 가져오기
            Vector3 dir = targetPos - transform.position;   // 방향 계산하기
            if( dir.sqrMagnitude < 0.001f ) // 목표지점에 도착했는지 확인
            {
                path.RemoveAt(0);   // 목표지점에 도착했으면 경로의 첫번째 노드 삭제
            }
            transform.Translate(Time.deltaTime * moveSpeed * dir.normalized);   // 실제 이동하기            
        }
    }
}
