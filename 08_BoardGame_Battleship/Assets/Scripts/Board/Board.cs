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

    private void Awake()
    {
        shipInfo = new ShipType[BoardSize * BoardSize];
        bombInfo = new bool[BoardSize * BoardSize];
    }

    private void Start()
    {
        GameManager.Inst.Input.onClick += OnClick;
    }

    /// <summary>
    /// 공격 당할 때 호출되는 함수
    /// </summary>
    /// <param name="pos">공격 당한 위치</param>
    /// <returns>true면 실제로 공격을 받았다. false면 여러 이유로 공격이 안됬다.</returns>
    public bool Attacked(Vector2Int pos)
    {
        bool result = false;
        if(IsValidPosition(pos))
        {
            if (IsAttackable(pos))
            {
                bombInfo[pos.y * BoardSize + pos.x] = true;
                result = true;
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


    // 배를 배치하기
    //  배를 배치하는 함수
    //  배의 방향을 지정하는 함수
    //  배를 배치할 수 있는 위치인지 확인하는 함수

    public bool ShipDeployment(Ship ship, Vector2Int pos)
    {
        Vector2Int[] gridPositions = new Vector2Int[ship.Size];

        Vector2Int offset = Vector2Int.zero;
        switch (ship.Direction)
        {
            case ShipDirection.NORTH:
                offset = Vector2Int.down;
                break;
            case ShipDirection.EAST:
                offset = Vector2Int.left;
                break;
            case ShipDirection.SOUTH:
                offset = Vector2Int.up;
                break;
            case ShipDirection.WEST:
                offset = Vector2Int.right;
                break;
        }

        for( int i=0;i<ship.Size;i++)
        {
            gridPositions[i] = pos + offset * i;
        }

        bool result = true;
        foreach(var tempPos in gridPositions)
        {
            if( !IsValidPosition(tempPos) || IsShipDeployed(tempPos) )
            {
                result = false;
                break;
            }
        }

        if (result)
        {
            foreach (var tempPos in gridPositions)
            {
                shipInfo[tempPos.y * BoardSize + tempPos.x] = ship.Type;
            }
        }

        // 주석 설명. 테스트 편하게 할 기능들 만들기

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
    /// 마우스 클릭 입력 처리용 함수
    /// </summary>
    /// <param name="screenPos">마우스 커서의 스크린 좌표</param>
    void OnClick(Vector2 screenPos)
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


    void Test()
    {
        shipInfo = new ShipType[BoardSize * BoardSize];
        for(int i=0;i<BoardSize;i++)
        {
            for(int j=0;j<BoardSize;j++)
            {
                ShipType ship = shipInfo[i*BoardSize+j];
            }
        }
    }
}
