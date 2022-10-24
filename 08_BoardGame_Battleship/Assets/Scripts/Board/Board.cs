using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // 상수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 게임 판의 크기용 상수. 한변의 길이가 10인 정사각형(10*10) .
    /// </summary>
    public const int BoardSize = 10;

    // 기본정보 ------------------------------------------------------------------------------------
    
    /// <summary>
    /// 배의 배치 정보. 2차원을 1차원으로 저장
    /// </summary>
    ShipType[] shipInfo;

    /// <summary>
    /// 공격 당한 위치 정보.  2차원을 1차원으로 저장
    /// </summary>
    bool[] bombInfo;

    /// <summary>
    /// 공격당한 위치를 시작적으로 표시해주는 클래스(O,X,검정색 구(테스트 전용))
    /// </summary>
    BombMark bombMark;

    /// <summary>
    /// 배 종류별 위치 정보저장용 딕셔너리.
    /// 배 종류별로 배가 배치되는 모든 칸 저장.
    /// </summary>
    Dictionary<ShipType, List<Vector2Int>> shipPositions;

    // 테스트 편의용 데이터 -------------------------------------------------------------------------
#if UNITY_EDITOR
    /// <summary>
    /// 배 배치된 위치를 시각적으로 표시(색깔있는 구, 배종류별로 색상 다름)
    /// </summary>
    ShipDeploymentInfo testShipDeploymentInfo;    
#endif

    // 델리게이트 ----------------------------------------------------------------------------------

    /// <summary>
    /// 배 종류별로 피격당했을 때 실행되는 델리게이트를 딕셔너리에 저장
    /// </summary>
    public Dictionary<ShipType, Action> onShipAttacked;

    // static 함수들 -------------------------------------------------------------------------------

    /// <summary>
    /// 인덱스를 그리드 좌표로 변환
    /// </summary>
    /// <param name="index">변환할 인덱스</param>
    /// <returns>변환된 그리드 좌표</returns>
    public static Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % BoardSize, index / BoardSize);
    }

    /// <summary>
    /// 그리드 좌표를 인덱스로 변환
    /// </summary>
    /// <param name="grid">변환할 그리드 좌표</param>
    /// <returns>변환된 인덱스</returns>
    public static int GridToIndex(Vector2Int grid)
    {
        return grid.x + grid.y * BoardSize;
    }

    /// <summary>
    /// 그리드상의 랜덤한 위치를 찾아주는 함수
    /// </summary>
    /// <returns>찾은 랜덤한 위치</returns>
    public static Vector2Int RandomPosition()
    {
        return new Vector2Int(UnityEngine.Random.Range(0, BoardSize), UnityEngine.Random.Range(0, BoardSize));
    }


    // 함수들 --------------------------------------------------------------------------------------

    // 유니티 이벤트 함수 --------------------------------------------------------------------------
    private void Awake()
    {
        shipInfo = new ShipType[BoardSize * BoardSize]; // 배 배치 정보 배열 만들기
        bombInfo = new bool[BoardSize * BoardSize];     // 포탄 공격 당한 위치 배열 만들기

        // 배 위치 저장용 딕셔너리 만들기
        shipPositions = new Dictionary<ShipType, List<Vector2Int>>(ShipManager.Inst.ShipTypeCount);
        shipPositions[ShipType.Carrier] = new List<Vector2Int>();       // 배 종류별로 위치 리스트 만들기
        shipPositions[ShipType.Battleship] = new List<Vector2Int>();
        shipPositions[ShipType.Destroyer] = new List<Vector2Int>();
        shipPositions[ShipType.Submarine] = new List<Vector2Int>();
        shipPositions[ShipType.PatrolBoat] = new List<Vector2Int>();

        // 배가 공격 당했을 때 실행될 델리게이트를 가지는 딕셔너리 만들기
        onShipAttacked = new Dictionary<ShipType, Action>(ShipManager.Inst.ShipTypeCount);

        // 캐싱용으로 찾아놓기
        bombMark = GetComponentInChildren<BombMark>();
#if UNITY_EDITOR
        testShipDeploymentInfo = GetComponentInChildren<ShipDeploymentInfo>();
#endif
    }    

    // 좌표 변환용 함수들 --------------------------------------------------------------------------
        
    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변환
    /// </summary>
    /// <param name="x">그리드 x좌표 위치</param>
    /// <param name="y">그리드 y좌표 위치</param>
    /// <returns>변환된 월드 좌표</returns>
    public Vector3 GridToWorld(int x, int y)
    {
        // 그리드 변환( 월드좌표 <-> 그리드 좌표). 그리드의 좌상이 (0,0), 우하가 (9,9), 왼->오른(+x), 위->아래(+y)
        return transform.position + new Vector3(x + 0.5f, 0, -(y + 0.5f));
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변환
    /// </summary>
    /// <param name="gridPos">그리드 좌표 위치</param>
    /// <returns>변환된 월드 좌표</returns>
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return GridToWorld(gridPos.x, gridPos.y);
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변환
    /// </summary>
    /// <param name="worldPos">변환할 월드 좌표</param>
    /// <returns>변환된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 diff = worldPos - transform.position;

        return new Vector2Int((int)diff.x, (int)-diff.z);
    }

    /// <summary>
    /// 인덱스를 월드 좌표로 변환
    /// </summary>
    /// <param name="index">변환할 인덱스</param>
    /// <returns>변환된 월드 좌표</returns>
    public Vector3 IndexToWorld(int index)
    {
        return GridToWorld(Board.IndexToGrid(index));
    }


    // 확인용 함수들 -------------------------------------------------------------------------------

    /// <summary>
    /// 공격 당할 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치 그리드 좌표</param>
    /// <returns>true면 공격 가능한 지역. false면 공격 불가능한 지역</returns>
    public bool IsAttackable(Vector2Int pos)
    {
        return !bombInfo[GridToIndex(pos)];
    }

    /// <summary>
    /// 공격 당할 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 위치의 인덱스</param>
    /// <returns>true면 공격 가능한 지역. false면 공격 불가능한 지역</returns>
    public bool IsAttackable(int index)
    {
        return !bombInfo[index];
    }

    /// <summary>
    /// 공격 실패한 지점인지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치(그리드 좌표)</param>
    /// <returns>true면 공격을 실패한 지점</returns>
    public bool IsAttackFailPosition(Vector2Int pos)
    {
        int index = GridToIndex(pos);

        // 공격을 한 지점 and 배가 없는 지점
        return bombInfo[index] && (shipInfo[index] == ShipType.None);
    }

    /// <summary>
    /// 공격 성공한 지점인지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치(그리드 좌표)</param>
    /// <returns>true면 공격이 성공한 지점</returns>
    public bool IsAttackSuccessPosition(Vector2Int pos)
    {
        int index = GridToIndex(pos);

        // 공격을 한 지점 and 배가 있는 지점
        return bombInfo[index] && (shipInfo[index] != ShipType.None);
    }

    /// <summary>
    /// 적절한 위치인지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>board 안이면 true, 아니면 false</returns>
    public static bool IsValidPosition(Vector2Int pos)
    {
        return ((-1 < pos.x && pos.x < BoardSize) && (-1 < pos.y && pos.y < BoardSize));
    }

    /// <summary>
    /// 적절한 인덱스인지 확인하는 함수(가로 10을 벗어나는 것을 체크하기가 힘들다.)
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 인덱스</returns>
    public static bool IsValidIndex(int index)
    {
        return (-1 < index && index < BoardSize * BoardSize);
    }

    /// <summary>
    /// 해당 위치에 배가 있는지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>true면 해당 위치에 배가 있다. false면 없다.</returns>
    private bool IsShipDeployed(Vector2Int pos)
    {
        return (shipInfo[GridToIndex(pos)] != ShipType.None);
    }

    /// <summary>
    /// 특정 위치에 함선을 배치할 수 있는지 여부를 리턴
    /// </summary>
    /// <param name="ship">확인할 배(크기, 방향 사용)</param>
    /// <param name="pos">확인할 위치(그리드 포지션)</param>
    /// <param name="gridPositions">확인할 배의 그리드 좌표들</param>
    /// <returns>true면 배치가능, false 불가능</returns>
    public bool IsShipDeployment(Ship ship, Vector2Int pos, out Vector2Int[] gridPositions)
    {
        gridPositions = new Vector2Int[ship.Size];
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
        for (int i = 0; i < ship.Size; i++)
        {
            gridPositions[i] = pos + offset * i;
        }

        // 배의 각 부분 좌표를 확인해서 해당칸이 맵 바깥이거나 다른 배가 하나라도 있으면 실패
        bool result = true;
        foreach (var tempPos in gridPositions)
        {
            if (!IsValidPosition(tempPos) || IsShipDeployed(tempPos))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 특정 위치에 함선을 배치할 수 있는지 여부를 리턴
    /// </summary>
    /// <param name="ship">확인할 배(크기, 방향 사용)</param>
    /// <param name="pos">확인할 위치(월드 포지션)</param>
    /// <returns>true면 배치가능, false 불가능</returns>
    public bool IsShipDeployment(Ship ship, Vector3 pos)
    {
        Vector2Int gridPos = WorldToGrid(pos);
        return IsShipDeployment(ship, gridPos, out _);
    }

    /// <summary>
    /// 특정 위치에 함선을 배치할 수 있는지 여부를 리턴
    /// </summary>
    /// <param name="ship">확인할 배(크기, 방향 사용)</param>
    /// <param name="pos">확인할 위치(그리드 포지션)</param>
    /// <returns>true면 배치가능, false 불가능</returns>
    public bool IsShipDeployment(Ship ship, Vector2Int pos)
    {
        return IsShipDeployment(ship, pos, out _);
    }

    // 함선 배치용 함수들 --------------------------------------------------------------------------

    /// <summary>
    /// 함선 배치하는 함수
    /// </summary>
    /// <param name="ship">배치할 함선</param>
    /// <param name="pos">배치할 그리드 좌표</param>
    /// <returns>성공여부(true면 성공)</returns>
    public bool ShipDeployment(Ship ship, Vector2Int pos)
    {
        Vector2Int[] gridPositions;
        bool result = IsShipDeployment(ship, pos, out gridPositions);   // 배가 배치 가능한지 확인

        // 모든 칸이 배치가 가능한 지역이면
        if (result)
        {
            foreach (var tempPos in gridPositions)
            {
                shipInfo[GridToIndex(tempPos)] = ship.Type;     // 모든 칸에 이 배의 타입을 저장
                shipPositions[ship.Type].Add(tempPos);          // 배 종류별로 해당 칸들을 기록
            }

            Vector3 worldPos = GridToWorld(pos);
            ship.transform.position = worldPos;                 // 함선을 보드위에 배치시키기

            ship.Deploy(gridPositions);                         // 배를 배치했다고 표시하고 그리드 좌표 받아서 기록

#if UNITY_EDITOR
            // 함선이 배치된 곳에 표시
            if (testShipDeploymentInfo != null) 
            {
                Vector3[] worldPositions = new Vector3[gridPositions.Length];
                for (int i = 0; i < worldPositions.Length; i++)
                {
                    worldPositions[i] = GridToWorld(gridPositions[i]);  // 배치된 그리드 좌표들을 월드 좌표로 변환
                }
                testShipDeploymentInfo.MarkShipDeplymentInfo(ship.Type, worldPositions);    // 배치된 배를 보여주기
            }
#endif
        }

        return result;
    }

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
    /// 함선 배치 취소하는 함수
    /// </summary>
    /// <param name="ship">취소할 배</param>
    public void UndoShipDeployment(Ship ship)
    {
#if UNITY_EDITOR
        if (testShipDeploymentInfo != null)
        {
            testShipDeploymentInfo.UnMarkShipDeplymentInfo(ship.Type);    // 보여주던 것 삭제하기
        }
#endif

        // shipInfo에 배가 삭제되었다는 것을 표시
        foreach (var pos in shipPositions[ship.Type])
        {
            shipInfo[GridToIndex(pos)] = ShipType.None;
        }
        shipPositions[ship.Type].Clear();   // 전부 비우고            
        ship.UnDeploy();                    // 배가 배치되지 않은 것으로 표시
    }

    // 피격용 함수들--------------------------------------------------------------------------------

    /// <summary>
    /// 공격 당할 때 호출되는 함수
    /// </summary>
    /// <param name="gridPos">공격 당한 위치(그리드 좌표)</param>
    /// <returns>true면 실제로 배가 공격을 받았다. false면 여러 이유(그리드 밖, 이미 공격 한곳, 바다)로 공격이 안됬다.</returns>
    public bool Attacked(Vector2Int gridPos)
    {
        bool result = false;
        if(IsValidPosition(gridPos))    // 적절한 좌표인지 확인
        {
            if (IsAttackable(gridPos))  // 공격 가능한 위치인지 확인(공격했던 곳은 다시 공격 못함)
            {
                int index = GridToIndex(gridPos);
                bombInfo[index] = true; // 공격 받은 위치 표시

                if(shipInfo[index] != ShipType.None)    // 배가 명중되어야 true 리턴
                {
                    result = true;
                }

                onShipAttacked[shipInfo[index]]?.Invoke();  // 해당 위치에 배가 있으면 배가 공격당한 함수 실행                
                    
                bombMark.SetBombMark(GridToWorld(gridPos), result);    // 포탄 명중 여부를 표시
            }
        }

        return result;
    }

    /// <summary>
    /// 공격 당할 때 호출되는 함수
    /// </summary>
    /// <param name="worldPos">공격 당한 위치(월드 좌표)</param>
    /// <returns>true면 실제로 공격을 받았다. false면 여러 이유로 공격이 안됬다.</returns>
    public bool Attacked(Vector3 worldPos)
    {
        return Attacked(WorldToGrid(worldPos));
    }


    // 테스트용 함수 -------------------------------------------------------------------------------

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
