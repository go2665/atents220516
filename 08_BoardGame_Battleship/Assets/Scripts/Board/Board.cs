using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // 게임 판의 크기(10*10)
    const int BoardSize = 10;

    // 배의 배치 정보를 저장한 배열
    ShipType[] shipInfo;    // 2차원 배열이지만 1차원 배열로 표현

    // 공격 당한 위치들을 표시하는 배열
    bool[] bombInfo;

#if UNITY_EDITOR
    ShipDeploymentInfo testShipDeploymentInfo;
    BombInfo testBombInfo;
#endif

    private void Awake()
    {
        shipInfo = new ShipType[BoardSize * BoardSize]; // 배 배치 정보 배열 만들기
        bombInfo = new bool[BoardSize * BoardSize];     // 포탄 공격 당한 위치 배열 만들기

#if UNITY_EDITOR
        testShipDeploymentInfo = GetComponentInChildren<ShipDeploymentInfo>();
        testBombInfo = GetComponentInChildren<BombInfo>();
#endif
    }

    private void Start()
    {
        GameManager.Inst.Input.onClick += Test_OnClick;
    }

    /// <summary>
    /// 공격 당할 때 호출되는 함수
    /// </summary>
    /// <param name="pos">공격 당한 위치</param>
    /// <returns>true면 실제로 공격을 받았다. false면 여러 이유로 공격이 안됬다.</returns>
    public bool Attacked(Vector2Int pos)
    {
        bool result = false;
        if(IsValidPosition(pos))    // 적절한 좌표인지 확인
        {
            if (IsAttackable(pos))  // 공격 가능한 위치인지 확인(공격했던 곳은 다시 공격 못함)
            {
                bombInfo[pos.y * BoardSize + pos.x] = true; // 공격 받았다고 표시
                result = true;

#if UNITY_EDITOR
                if( testBombInfo != null )
                {

                }
#endif
            }
        }

        return result;
    }

    /// <summary>
    /// 공격 당할 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>true면 공격 가능한 지역. false면 공격 불가능한 지역</returns>
    private bool IsAttackable(Vector2Int pos)
    {
        return !bombInfo[pos.y * BoardSize + pos.x];
    }

    /// <summary>
    /// 적절한 위치인지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>board 안이면 true, 아니면 false</returns>
    private bool IsValidPosition(Vector2Int pos)
    {
        return ((-1 < pos.x && pos.x < BoardSize) && (-1 < pos.y && pos.y < BoardSize));
    }

    /// <summary>
    /// 해당 위치에 배가 있는지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>true면 해당 위치에 배가 있다. false면 없다.</returns>
    private bool IsShipDeployed(Vector2Int pos)
    {
        return (shipInfo[pos.y * BoardSize + pos.x] != ShipType.None);
    }

    //bool IsValidAndAttackable(Vector2Int pos)
    //{
    //    return IsAttackable(pos) && IsValidPosition(pos);
    //}


    
    /// <summary>
    /// 함선 배치하는 함수
    /// </summary>
    /// <param name="ship">배치할 함선</param>
    /// <param name="pos">배치할 월드좌표</param>
    /// <returns>성공여부(true면 성공)</returns>
    public bool ShipDeployment(Ship ship, Vector3 pos)
    {
        Vector2Int gridPos = WorldToGrid(pos);

        return ShipDeployment(ship, gridPos);
    }

    /// <summary>
    /// 함선 배치하는 함수
    /// </summary>
    /// <param name="ship">배치할 함선</param>
    /// <param name="pos">배치할 그리드 좌표</param>
    /// <returns>성공여부(true면 성공)</returns>
    public bool ShipDeployment(Ship ship, Vector2Int pos)
    {
        Vector2Int[] gridPositions = new Vector2Int[ship.Size];

        Vector2Int offset = Vector2Int.zero;    // 배 머리부터 꼬리까지 한칸씩 확인하기 위한 값
        switch (ship.Direction)
        {
            case ShipDirection.NORTH:
                offset = Vector2Int.up;         // 북쪽을 바라보고 있기 때문에 꼬리로 갈수록 그리드 좌표로는 y가 점점 증가한다.
                break;
            case ShipDirection.EAST:
                offset = Vector2Int.left;       // 동쪽을 바라보고 있기 때문에 꼬리로 갈수록 그리드 좌표로는 x가 점점 감소한다.
                break;
            case ShipDirection.SOUTH:
                offset = Vector2Int.down;       // 남쪽을 바라보고 있기 때문에 꼬리로 갈수록 그리드 좌표로는 y가 점점 감소한다.
                break;
            case ShipDirection.WEST:
                offset = Vector2Int.right;      // 서쪽을 바라보고 있기 때문에 꼬리로 갈수록 그리드 좌표로는 x가 점점 증가한다.
                break;
        }

        // 배 머리부터 시작해서 배꼬리까지 좌표를 하나씩 구하기
        for( int i=0;i<ship.Size;i++)
        {
            gridPositions[i] = pos + offset * i;
        }

        // 배의 각 부분 좌표를 확인해서 해당칸이 맵 바깥이거나 다른 배가 하나라도 있으면 실패
        bool result = true;
        foreach(var tempPos in gridPositions)
        {
            if( !IsValidPosition(tempPos) || IsShipDeployed(tempPos) )
            {
                result = false;
                break;
            }
        }

        // 모든 칸이 배치가 가능한 지역이면
        if (result)
        {
            foreach (var tempPos in gridPositions)
            {
                shipInfo[tempPos.y * BoardSize + tempPos.x] = ship.Type;    // 모든 칸에 이 배의 타입을 저장
            }

#if UNITY_EDITOR
            if (testShipDeploymentInfo != null)
            {
                Vector3[] worldPositions = new Vector3[gridPositions.Length];
                for(int i=0;i<worldPositions.Length;i++)
                {
                    worldPositions[i] = GridToWorld(gridPositions[i]);
                }
                testShipDeploymentInfo.MarkShipDeplymentInfo(ship.Type, worldPositions);    // 배치된 배를 보여주기
            }
#endif
        }

        return result;
    }


    // 그리드 변환( 월드좌표 <-> 그리드 좌표). 그리드의 좌상이 (0,0), 우하가 (9,9), 왼->오른(+x), 위->아래(+y)
    public Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(x + 0.5f, 0, -(y + 0.5f));
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return GridToWorld(gridPos.x, gridPos.y);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 diff = worldPos - transform.position;

        return new Vector2Int( (int)diff.x, (int)-diff.z);
    }

    /// <summary>
    /// 마우스 클릭 입력 테스트 함수
    /// </summary>
    /// <param name="screenPos">마우스 커서의 스크린 좌표</param>
    void Test_OnClick(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if( Physics.Raycast(ray, out RaycastHit hit, 100.0f, LayerMask.GetMask("Sea")) )
        {
            //Vector2Int gridPos = WorldToGrid(hit.point);
            //gridPos.x = 120;
            //Attacked(gridPos);

            //Debug.Log($"Grid : {gridPos}");
            //Vector3 worldPos = GridToWorld(gridPos);
            //Debug.Log($"World : {worldPos}");
        }
    }
}
