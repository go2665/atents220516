using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 플레이어 기본 클래스
/// </summary>
public class PlayerBase : MonoBehaviour
{
    public GameObject highCandidatePrefab;  // 우선순위가 높은지역을 표시할 프리팹

    /// <summary>
    /// 이 플레이어의 게임판( 플레이어의 자식으로 넣을지 고민중 )
    /// </summary>
    protected Board board;

    /// <summary>
    /// 플레이어의 상태
    /// </summary>
    protected PlayerState state = PlayerState.Title;

    /// <summary>
    /// 플레이어가 가지고 있는 배(Start할 때 자동 생성)
    /// </summary>
    protected Ship[] ships;

    protected int hp;

    protected PlayerBase oppenent;

    List<int> attackCandidateIndice;        // 섞여있는 전체 좌표 목록
    List<int> attackComboIndice;            // 연속 성공 판정을 위한 맞은 위치
    List<int> attackHighCandidateIndice;    // 우선순위가 높은 공격 목표
#if UNITY_EDITOR
    Dictionary<int, GameObject> highCandidateMark = new Dictionary<int, GameObject>();
#endif

    public Board Board => board;

    protected virtual void Awake()
    {
        board = GetComponentInChildren<Board>();
        attackComboIndice = new List<int>();
        attackHighCandidateIndice = new List<int>();
    }

    protected virtual void Start()
    {
        // 배 종류별로 하나씩 생성
        int shipTypeCount = ShipManager.Inst.ShipTypeCount;
        ships = new Ship[shipTypeCount];
        for ( int i=0;i< shipTypeCount; i++)
        {
            ships[i] = ShipManager.Inst.MakeShip((ShipType)(i + 1), this);
            ships[i].onDead += OnShipDestroy;
            board.onShipAttacked[(ShipType)(i + 1)] = ships[i].OnAttacked;  // 배 종류별로 공격 당할 때 실행될 함수 연결
        }
        board.onShipAttacked[ShipType.None] = null; // 키값 추가용.

        // 전체 공격 목표 만들기
        int fullSize = Board.BoardSize * Board.BoardSize;
        int[] tempCandidate = new int[fullSize];
        for(int i=0;i< fullSize;i++)        // 순서대로 들어간 것 만들기
        {
            tempCandidate[i] = i;
        }
        Utils.Shuffle<int>(tempCandidate);  //섞고
        attackCandidateIndice = new List<int>(tempCandidate);   // 섞은 것을 기반으로 리스트 만들기


        hp = shipTypeCount;
    }

    private void OnShipDestroy(ShipType type)
    {
        hp--;
        if( hp <= 0 )
        {
            OnDefeat();
        }
    }

    private void OnDefeat()
    {

    }

    /// <summary>
    /// 자동 함선 배치 함수
    /// 완전 랜덤 처리
    /// </summary>
    public void AutoShipDeployment_v1()
    {
        foreach (var ship in ships)
        {
            if (ship.IsDeployed)
                continue;

            int rotateCount = UnityEngine.Random.Range(0, ShipManager.Inst.ShipDirectionCount);
            bool isCCW = (UnityEngine.Random.Range(0, 10) % 2) == 0;
            for (int i=0;i< rotateCount;i++)
            {
                ship.Rotate(isCCW);
            }

            bool result;
            Vector2Int randPos;
            do
            {
                randPos = Board.RandomPosition();
                result = board.IsShipDeployment(ship, randPos);
            } while (!result);

            board.ShipDeployment(ship, randPos);
        }        
    }
            
    /// <summary>
    /// 자동 함선 배치 함수
    /// 가능한 배끼리 붙지 않고 벽부분에 배가 배치되지 않도록 설정
    /// </summary>
    public void AutoShipDeployment()
    {
        int maxCapacity = Board.BoardSize * Board.BoardSize;    // 보드의 모든 칸수만큼 크기 확보
        List<int> highPriority = new(maxCapacity);              // 우선 순위가 높은 배치 후보지
        List<int> lowPriority = new(maxCapacity);               // 우선 순위가 낮은 배치 후보지

        // 가장자리 부분을 낮은 우선 순위에 배치. 벽에 배치되면 상대방의 성공률이 올라가기 때문에
        for (int i = 0; i < maxCapacity; i++)
        {
            // Board.BoardSize가 10일때
            if ( i % 10 == 0                            // 0,10,20,30,40,50,60,70,80,90
                || i % 10 == (Board.BoardSize - 1)      // 9,19,29,39,49,59,69,79,89,99
                || (0 < i && i < (Board.BoardSize - 1)) // 1~8
                || ((Board.BoardSize - 1) * Board.BoardSize < i && i < (Board.BoardSize * Board.BoardSize - 1)))    // 91~98
            {   
                lowPriority.Add(i);     // 가장자리일 경우 낮은 우선 순위에 추가
            }
            else
            {
                highPriority.Add(i);    // 그 외의 경우는 높은 우선 순위에 추가
            }
        }

        // highPriority를 섞기(피셔 예이츠 알고리즘 사용)
        int[] temp = highPriority.ToArray();
        Utils.Shuffle<int>(temp);        
        highPriority = new List<int>(temp);     // 셔플이 끝난 배열을 다시 리스트로 변환해서 highPriority에 추가

        // lowPriority도 섞기(highPriority와 같음)
        temp = lowPriority.ToArray();
        Utils.Shuffle<int>(temp);
        lowPriority = new List<int>(temp);

        // 함선마다 배치작업 처리
        foreach (var ship in ships)
        {
            if (ship.IsDeployed)            // 배치 완료된 배는 다시 배치하지 않음
                continue;

            // 함선 회전 시키기
            ship.RandomRotate();

            // 위치 결정하기
            Vector2Int pos;                 // 배의 머리부분의 위치(그리드 좌표)
            Vector2Int[] shipPositions;     // 배가 배치될 예정인 위치들(그리드 좌표)
            bool failDeployment = true;     // 함선 배치가 성공인지 실패인지 나타낼 변수
            int counter = 0;                // 무한 루프를 방지하고 낮은 우선 순위의 위치도 한번씩 선택되게 하기 위한 카운터

            // high 쪽 탐색
            do
            {
                // 리스트의 첫번째 데이터를 꺼내기
                int headIndex = highPriority[0];    // 첫번째 숫자 저장
                highPriority.RemoveAt(0);           // 리스트에서 제거
                pos = Board.IndexToGrid(headIndex); // 인덱스를 그리드 좌표로 변환

                failDeployment = !board.IsShipDeployment(ship, pos, out shipPositions); // 해당 위치에 배를 배치할 수 있는지 확인
                if (failDeployment)
                {
                    // 배치할 수 없음. 원래 리스트에 인덱스 다시 넣기
                    highPriority.Add(headIndex);
                }
                else
                {
                    // 머리는 배치 성공. 머리를 제외한 나머지 부분이 전부 high에 있는지 확인
                    for (int i = 1; i < shipPositions.Length; i++)
                    {
                        int bodyIndex = Board.GridToIndex(shipPositions[i]);
                        if (!highPriority.Exists((x) => x == bodyIndex))    // bodyIndex가 high에 있는지 확인
                        {
                            // 하나의 바디라도 high에 없으면 실패.
                            highPriority.Add(headIndex);    // 원래 리스트에 인덱스 다시 넣기
                            failDeployment = true;          // 배치 실패로 표시
                            break;
                        }
                    }
                }
                counter++;  // 무한 루프 방지 용도
            } while (failDeployment && counter < 5 && highPriority.Count > 0);  // 배치가 성공했거나, 5회 이상 찾았거나, highPriority가 비었으면 루프 탈출

            // 필요할 경우 low쪽 탐색
            while (failDeployment)  // 배치가 실패했는데 이곳에 왔으면 counter가 5 이상이거나 highPriority가 비었다.
            {
                //Debug.Log("Low 확인 중");
                int headIndex = lowPriority[0];
                lowPriority.RemoveAt(0);
                pos = Board.IndexToGrid(headIndex);

                failDeployment = !board.IsShipDeployment(ship, pos, out shipPositions); // 배치할 수 있는 위치면 무조건 배치(우선순위 생각하지 않음)
                if (failDeployment)
                {
                    // 배치 실패. 원래 리스트에 되돌리기
                    lowPriority.Add(headIndex);
                }
            }

            // 위치는 골라졌다.=> 함선 배치
            board.ShipDeployment(ship, pos);        // 함선 배치

            // 배 모델 위치도 이동시키기
            ship.transform.position = board.GridToWorld(pos);   // 배치된 그리드 좌표를 월드 좌표로 변환해서 이동
            ship.gameObject.SetActive(true);        // 실제로 보여주기

            // 함선이 차지하는 영역은 모든 리스트에서 제거
            List<int> tempList = new List<int>(shipPositions.Length);
            foreach (var tempPos in shipPositions)   // 배치할 위치(그리드 좌표)를 index로 변환
            {
                tempList.Add(Board.GridToIndex(tempPos));
            }
            foreach (var tempIndex in tempList)
            {
                highPriority.Remove(tempIndex);     // 전체 목록에서 제거
                lowPriority.Remove(tempIndex);
            }

            // 함선 주변 지역을 high에서 low로 이동
            List<int> toLowList = new List<int>(ship.Size * 2 + 6);
            
            // 함선 주변 위치 계산            
            if( ship.Direction == ShipDirection.NORTH || ship.Direction == ShipDirection.SOUTH)
            {
                // 배가 세로 방향이다.
                foreach (var tempPos in shipPositions)
                {
                    // 배 위치의 좌우 위치를 low로 이동                    
                    toLowList.Add(Board.GridToIndex(tempPos + new Vector2Int(1, 0)));
                    toLowList.Add(Board.GridToIndex(tempPos + new Vector2Int(-1, 0)));
                }

                Vector2Int headFrontPos;    // 머리 앞쪽 위치
                Vector2Int tailRearPos;     // 꼬리 뒤쪽 위치
                if ( ship.Direction == ShipDirection.NORTH )
                {
                    // 북쪽을 바라보고 있다. => 머리 위치 -y
                    headFrontPos = shipPositions[0] + Vector2Int.down;
                    tailRearPos = shipPositions[^1] + Vector2Int.up;                    
                }
                else
                {
                    // 남쪽을 바라보고 있다 -> 머리 위치 +y
                    headFrontPos = shipPositions[0] + Vector2Int.up;
                    tailRearPos = shipPositions[^1] + Vector2Int.down;
                }
                // 머리 앞쪽과 머리 앞쪽의 양옆과 꼬리 뒤쪽의 양옆을 low로 이동
                toLowList.Add(Board.GridToIndex(headFrontPos));
                toLowList.Add(Board.GridToIndex(headFrontPos + new Vector2Int(1, 0)));
                toLowList.Add(Board.GridToIndex(headFrontPos + new Vector2Int(-1, 0)));
                toLowList.Add(Board.GridToIndex(tailRearPos));
                toLowList.Add(Board.GridToIndex(tailRearPos + new Vector2Int(1, 0)));
                toLowList.Add(Board.GridToIndex(tailRearPos + new Vector2Int(-1, 0)));
            }
            else
            {
                // 배가 가로 방향 이다.
                foreach (var tempPos in shipPositions)
                {
                    // 배 위치의 위아래 위치를 low로 이동
                    toLowList.Add(Board.GridToIndex(tempPos + new Vector2Int(0, 1)));
                    toLowList.Add(Board.GridToIndex(tempPos + new Vector2Int(0, -1)));
                }

                Vector2Int headFrontPos;    // 머리 앞쪽 위치
                Vector2Int tailRearPos;     // 꼬리 뒤쪽 위치
                if (ship.Direction == ShipDirection.EAST)
                {
                    // 동쪽을 바라보고 있다. => 머리 위치 +x
                    headFrontPos = shipPositions[0] + Vector2Int.right;
                    tailRearPos = shipPositions[^1] + Vector2Int.left;
                }
                else
                {
                    // 서쪽을 바라보고 있다 => 머리 위치 -x
                    headFrontPos = shipPositions[0] + Vector2Int.left;
                    tailRearPos = shipPositions[^1] + Vector2Int.right;
                }
                // 머리 앞쪽과 머리 앞쪽의 양옆과 꼬리 뒤쪽의 양옆을 low로 이동
                toLowList.Add(Board.GridToIndex(headFrontPos));
                toLowList.Add(Board.GridToIndex(headFrontPos + new Vector2Int(0, 1)));
                toLowList.Add(Board.GridToIndex(headFrontPos + new Vector2Int(0, -1)));
                toLowList.Add(Board.GridToIndex(tailRearPos));
                toLowList.Add(Board.GridToIndex(tailRearPos + new Vector2Int(0, 1)));
                toLowList.Add(Board.GridToIndex(tailRearPos + new Vector2Int(0, -1)));
            }

            // Low로 보낼 인덱스들을 Low로 보내기
            foreach(var index in toLowList)
            {
                // highPriority에서 이미 제거된 위치를 다시 제거할 수 있으므로 highPriority에 있는 인덱스만 이동
                if ( highPriority.Exists((x) => x == index))    
                {
                    highPriority.Remove(index); // High에서 제거하기
                    lowPriority.Add(index);     // Low에 추가하기
                }
            }
        }
    }

    /// <summary>
    /// 모든 함선의 배치 취소
    /// </summary>
    public void UndoAllShipDeployment()
    {        
        foreach(var ship in ships)
        {
            board.UndoShipDeployment(ship);
        }
    }

    /// <summary>
    /// 자동 공격 함수
    /// 플레이어가 타임 오버 되었을 때나 적이 공격할 때 사용
    /// </summary>
    public void AutoAttack()
    {
        int target = -1;
        // 공격지점 선택 조건
        // 공격이 한줄로 연속으로 명중했으면 그 줄의 앞 뒤 중 하나를 공격한다.
        // 이전 공격이 명중했으면 명중위치의 동서남북 중 하나를 공격한다.

        // 중복없는 랜덤으로 고른다.
        target = attackCandidateIndice[0];
        attackCandidateIndice.RemoveAt(0);

        Attack(target);
    }

    public void Attack(Vector3 worldPos)
    {
        Attack(oppenent.Board.WorldToGrid(worldPos));
    }

    public void Attack(Vector2Int gridPos)
    {
        int indexPos = Board.GridToIndex(gridPos);
        RemoveHighCandidate(indexPos);  // 일단 삭제 시도

        bool result = oppenent.Board.Attacked(gridPos);

        // 공격 했다는 표시
        // 성공했으면 추가로 성공 표시

        // 우선 순위가 높은 지역 표시
        if(result)
        {
            //공격이 성공했으면 공격한 지점의 동서남북에 highCandidatePrefab 생성해서 표시하기
            Vector2Int[] neighbor = { new(-1, 0), new(1, 0), new(0, 1), new(0, -1) };

            foreach (var side in neighbor)
            {
                Vector2Int n = gridPos + side;
                int index = Board.GridToIndex(n);
                AddHighCandidate(index);
            }
        }
    }

    public void Attack(int index)
    {
        Attack(Board.IndexToGrid(index));
    }

    void AddHighCandidate(int index)
    {
        attackHighCandidateIndice.Add(index);

#if UNITY_EDITOR
        // highCandidatePrefab 생성하기
        GameObject obj = Instantiate(highCandidatePrefab);
        obj.transform.position = oppenent.board.IndexToWorld(index) + Vector3.up;
        highCandidateMark[index] = obj;
#endif
    }

    void RemoveHighCandidate(int index)
    {
        if (attackHighCandidateIndice.Exists((x) => x == index))
        {
            attackHighCandidateIndice.Remove(index);
#if UNITY_EDITOR
            Destroy(highCandidateMark[index]);
            highCandidateMark[index] = null;
            highCandidateMark.Remove(index);
#endif
        }
    }


    // 테스트 용도(플레이어의 상태 설정)
    public void Test_SetState(PlayerState state)
    {
        this.state = state;
    }
}
