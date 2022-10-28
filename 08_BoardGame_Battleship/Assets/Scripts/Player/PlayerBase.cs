using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 기본 클래스
/// </summary>
public class PlayerBase : MonoBehaviour
{
    // 플레이어 기본정보 ---------------------------------------------------------------------------
    
    /// <summary>
    /// 플레이어의 상태
    /// </summary>
    protected GameState state = GameState.Title;

    /// <summary>
    /// 이 플레이어의 게임판( 보드는 플레이어의 자식 )
    /// </summary>
    protected Board board;

    /// <summary>
    /// 플레이어가 가지고 있는 배(Start할 때 자동 생성)
    /// </summary>
    protected Ship[] ships;

    /// <summary>
    /// 남아 있는 배의 대수
    /// </summary>
    protected int remainShipCount;

    /// <summary>
    /// 행동 완료 표시. true면 이번턴에 행동 완료
    /// </summary>
    bool isActionDone = false;

    /// <summary>
    /// 성공한 공격 횟수
    /// </summary>
    int successAttackCount = 0;

    /// <summary>
    /// 실패한 공격 횟수
    /// </summary>
    int failAttackCount = 0;

    // 플레이어의 상대 정보 -------------------------------------------------------------------------
    
    /// <summary>
    /// 대전 상대
    /// </summary>
    protected PlayerBase opponent;

    // 후보지역(우선순위가 높은 공격 목표) 정보 ------------------------------------------------------

    /// <summary>
    /// 보드에 후보지역으로 표시할 프리팹
    /// </summary>
    public GameObject highCandidatePrefab;

    /// <summary>
    /// 랜덤 공격을 위한 전체 Board 좌표
    /// </summary>
    private List<int> attackCandidateIndice;

    /// <summary>
    /// 후보지역 리스트
    /// </summary>
    private List<int> attackHighCandidateIndice;

    /// <summary>
    /// 마지막에 공격 성공한 위치
    /// </summary>
    private Vector2Int lastAttackSuccessPos;

    /// <summary>
    /// invalid한 좌표 표시용. 이전에 공격 성공한 적 없음을 표시.
    /// </summary>
    readonly private Vector2Int NOT_SUCCESS_YET = -Vector2Int.one;

    /// <summary>
    /// 이번턴에 상대방의 배가 부서졌는지 표시.
    /// </summary>
    private bool opponentShipDestroyed = false;

#if UNITY_EDITOR
    /// <summary>
    /// 후보지역 표시용 게임 오브젝트 저장하는 딕셔너리
    /// 키값은 해당 위치의 index
    /// </summary>
    private Dictionary<int, GameObject> highCandidateMark = new Dictionary<int, GameObject>();
#endif

    // 프로퍼티 ------------------------------------------------------------------------------------
    
    /// <summary>
    /// 이 플레이어가 가지고 있는 보드의 읽기전용 프로퍼티
    /// </summary>
    public Board Board => board;

    /// <summary>
    /// 이 플레이어가 가지고 있는 배들의 배열을 읽을 수 있는 읽기전용 프로퍼티
    /// </summary>
    public Ship[] Ships => ships;

    /// <summary>
    /// 이 플레이어가 패배했는지 알려주는 프로퍼티(true면 이 플레이어가 패배)
    /// </summary>
    public bool IsDepeat => remainShipCount < 1;

    /// <summary>
    /// 성공한 공격 횟수를 알려주는 프로퍼티
    /// </summary>
    public int SuccessAttackCount => successAttackCount;

    /// <summary>
    /// 실패한 공격 횟수를 알려주는 프로퍼티
    /// </summary>
    public int FailAttackCount => failAttackCount;

    // 델리게이트 ----------------------------------------------------------------------------------

    /// <summary>
    /// 플레이어의 공격이 실패했음을 알리는 델리게이트
    /// </summary>
    public Action<PlayerBase> onAttackFail;

    /// <summary>
    /// 플레이어의 행동이 끝났음을 알리는 델리게이트
    /// </summary>
    public Action onActionEnd;

    /// <summary>
    /// 플레이어가 패배했음을 알리는 델리게이트
    /// </summary>
    public Action<PlayerBase> onDefeat;

    // 함수들 --------------------------------------------------------------------------------------

    // 유니티 이벤트 함수들 -------------------------------------------------------------------------
    protected virtual void Awake()
    {
        board = GetComponentInChildren<Board>();        // 보드캐싱
        attackHighCandidateIndice = new List<int>();    // 후보지역 리스트 생성
        lastAttackSuccessPos = NOT_SUCCESS_YET;         // 마지막 공격 성공 위치의 초기값으로 공격 성공한 적이 없음을 표시.
    }

    protected virtual void Start()
    {
        // 배 종류별로 하나씩 생성
        int shipTypeCount = ShipManager.Inst.ShipTypeCount;
        ships = new Ship[shipTypeCount];
        for ( int i=0;i< shipTypeCount; i++)
        {
            ships[i] = ShipManager.Inst.MakeShip((ShipType)(i + 1), this);  // 배 종류별로 생성
            ships[i].onSinking += OnShipDestroy;                               // 배가 침몰될 때 실행될 함수 연결
            board.onShipAttacked[(ShipType)(i + 1)] = ships[i].OnAttacked;  // 배 종류별로 공격 당할 때 실행될 함수 연결
        }
        board.onShipAttacked[ShipType.None] = null; // 키값 추가용.
        remainShipCount = shipTypeCount;            // 남아있는 배의 댓수 설정


        // 랜덤 공격을 위한 전체 Board 좌표 생성
        int fullSize = Board.BoardSize * Board.BoardSize;
        int[] tempCandidate = new int[fullSize];
        for(int i=0;i< fullSize;i++)        // 순서대로 들어간 것 만들기(0~99)
        {
            tempCandidate[i] = i;
        }
        Utils.Shuffle<int>(tempCandidate);  // 배열 섞고
        attackCandidateIndice = new List<int>(tempCandidate);   // 섞은 배열을 기반으로 리스트 만들기        

        // 공격 횟수 초기화
        successAttackCount = 0;
        failAttackCount = 0;
    }

    // 일반 이벤트 함수들 --------------------------------------------------------------------------

    public void OnStateChange(GameState gameState)
    {
        state = gameState;

        switch (state)
        {
            case GameState.Title:
                break;
            case GameState.ShipDeployment:
                break;
            case GameState.Battle:
                break;
            case GameState.GameEnd:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 턴이 시작될 때 실행될 함수
    /// </summary>
    public virtual void OnPlayerTurnStart(int turnNumber)
    {
        isActionDone = false;
    }

    /// <summary>
    /// 턴이 종료될 때 실행될 함수
    /// </summary>
    public virtual void OnPlayerTurnEnd()
    {

    }

    /// <summary>
    /// 이 플레이어의 배가 부서질 때 실행될 함수
    /// </summary>
    /// <param name="ship">파괴된 배</param>
    private void OnShipDestroy(Ship ship)
    {
        // ship는 당장 사용하지 않음. 나중에 침몰 연출 등을 위해 가지고 있음.

        opponent.opponentShipDestroyed = true;  // 이번턴에 이 플레이어의 배가 부서졌음을 표시

        remainShipCount--;          // 함선 댓수 하나 줄이기
        if( remainShipCount <= 0 )  // 모두 침몰 되었으면 
        {
            OnDefeat();             // 패배 처리용 함수 실행
        }
    }

    /// <summary>
    /// 이 플레이어가 패배했을때 실행될 함수
    /// </summary>
    private void OnDefeat()
    {
        Debug.Log($"{gameObject.name} 패배");
        onDefeat?.Invoke(this);
    }

    // 함선 배치용 함수 ----------------------------------------------------------------------------

    /// <summary>
    /// 완전히 랜덤으로 함선을 자동으로 배치하는 함수. (사용안함)
    /// </summary>
    public void AutoShipDeployment_CompletelyRandom()
    {
        foreach (var ship in ships) // 모든 배를 돌아가며 배치
        {
            if (ship.IsDeployed)    // 배치된 배는 스킵
                continue;

            int rotateCount = UnityEngine.Random.Range(0, ShipManager.Inst.ShipDirectionCount); // 회전 회수 랜덤으로 결정
            bool isCCW = (UnityEngine.Random.Range(0, 10) % 2) == 0;    // 회전 방향 랜덤으로 결정
            for (int i=0;i< rotateCount;i++)
            {
                ship.Rotate(isCCW); // 결정난 대로 회전 시키기
            }

            bool result;
            Vector2Int randPos;
            do
            {
                randPos = Board.RandomPosition();
                result = board.IsShipDeployment(ship, randPos);
            } while (!result);  // 배치할 수 있는 위치가 나올 때까지 계속 랜덤으로 위치 뽑기

            board.ShipDeployment(ship, randPos);    // 랜덤 위치가 나오면 배치
        }        
    }
            
    /// <summary>
    /// 자동 함선 배치 함수
    /// 조건 : 가능한 배끼리 붙지 않고, 가능한 벽부분에 배가 배치되지 않도록 설정
    /// </summary>
    public void AutoShipDeployment(bool isShowShips = false)
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
            {
                // 배의 그리드 좌표를 인덱스로 변경
                int[] shipIndice = new int[ship.Size];                
                for (int i = 0; i < ship.Size; i++)
                {
                    shipIndice[i] = Board.GridToIndex(ship.Positions[i]);
                }
                // 배의 그리드 좌표를 변환한 인덱스를 목록에서 제거
                foreach (var index in shipIndice)
                {
                    lowPriority.Remove(index);
                    highPriority.Remove(index);
                }

                // 함선 주변 위치 구하기
                List<int> toLow = ShipAroundPositions(ship);
                // Low로 보낼 인덱스들을 Low로 보내기
                foreach (var index in toLow)
                {
                    // highPriority에서 이미 제거된 위치를 다시 제거할 수 있으므로 highPriority에 있는 인덱스만 이동
                    if (highPriority.Exists((x) => x == index))
                    {
                        highPriority.Remove(index); // High에서 제거하기
                        lowPriority.Add(index);     // Low에 추가하기
                    }
                }

                continue;
            }

            // 함선 회전 시키기
            ship.RandomRotate();

            // 위치 결정하기
            Vector2Int pos;                 // 배의 머리부분의 위치(그리드 좌표)
            Vector2Int[] shipPositions;     // 배가 배치될 예정인 위치들(그리드 좌표)
            bool failDeployment = true;     // 함선 배치가 성공인지 실패인지 나타낼 변수                       
            int counter = 0;                // 무한 루프를 방지하고 낮은 우선 순위의 위치도 한번씩 선택되게 하기 위한 카운터

            // 우선 순위가 높은 highPriority쪽 탐색
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

                failDeployment = !board.IsShipDeployment(ship, pos, out _); // 배치할 수 있는 위치면 무조건 배치(우선순위 생각하지 않음)
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
            ship.gameObject.SetActive(isShowShips);             // 보여주고 싶을 때만 보여주기

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

            // 배 주면 위치들 인덱스로 구하기
            List<int> toLowList = ShipAroundPositions(ship);

            // Low로 보낼 인덱스들을 Low로 보내기
            foreach (var index in toLowList)
            {
                // highPriority에서 이미 제거된 위치를 다시 제거할 수 있으므로 highPriority에 있는 인덱스만 이동
                if (highPriority.Exists((x) => x == index))
                {
                    highPriority.Remove(index); // High에서 제거하기
                    lowPriority.Add(index);     // Low에 추가하기
                }
            }
        }
    }

    /// <summary>
    /// 함선 주변 지역 위치 인덱스 구하는 함수
    /// </summary>
    /// <param name="ship">주변을 찾을 배</param>
    /// <returns>함선 주변 위치의 인덱스 리스트</returns>
    private List<int> ShipAroundPositions(Ship ship)
    {        
        // 함선 주변 지역을 high에서 low로 이동
        List<int> toLowList = new List<int>(ship.Size * 2 + 6);

        // 함선 주변 위치 계산            
        if (ship.Direction == ShipDirection.NORTH || ship.Direction == ShipDirection.SOUTH)
        {
            // 배가 세로 방향이다.
            foreach (var tempPos in ship.Positions)
            {
                // 배 위치의 좌우 위치를 low로 이동                    
                toLowList.Add(Board.GridToIndex(tempPos + new Vector2Int(1, 0)));
                toLowList.Add(Board.GridToIndex(tempPos + new Vector2Int(-1, 0)));
            }

            Vector2Int headFrontPos;    // 머리 앞쪽 위치
            Vector2Int tailRearPos;     // 꼬리 뒤쪽 위치
            if (ship.Direction == ShipDirection.NORTH)
            {
                // 북쪽을 바라보고 있다. => 머리 위치 -y
                headFrontPos = ship.Positions[0] + Vector2Int.down;
                tailRearPos = ship.Positions[^1] + Vector2Int.up;
            }
            else
            {
                // 남쪽을 바라보고 있다 -> 머리 위치 +y
                headFrontPos = ship.Positions[0] + Vector2Int.up;
                tailRearPos = ship.Positions[^1] + Vector2Int.down;
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
            foreach (var tempPos in ship.Positions)
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
                headFrontPos = ship.Positions[0] + Vector2Int.right;
                tailRearPos = ship.Positions[^1] + Vector2Int.left;
            }
            else
            {
                // 서쪽을 바라보고 있다 => 머리 위치 -x
                headFrontPos = ship.Positions[0] + Vector2Int.left;
                tailRearPos = ship.Positions[^1] + Vector2Int.right;
            }
            // 머리 앞쪽과 머리 앞쪽의 양옆과 꼬리 뒤쪽의 양옆을 low로 이동
            toLowList.Add(Board.GridToIndex(headFrontPos));
            toLowList.Add(Board.GridToIndex(headFrontPos + new Vector2Int(0, 1)));
            toLowList.Add(Board.GridToIndex(headFrontPos + new Vector2Int(0, -1)));
            toLowList.Add(Board.GridToIndex(tailRearPos));
            toLowList.Add(Board.GridToIndex(tailRearPos + new Vector2Int(0, 1)));
            toLowList.Add(Board.GridToIndex(tailRearPos + new Vector2Int(0, -1)));
        }

        return toLowList;
    }

    /// <summary>
    /// 모든 함선의 배치 취소
    /// </summary>
    public void UndoAllShipDeployment()
    {        
        foreach(var ship in ships)              // 모든 함선들을
        {
            board.UndoShipDeployment(ship);     // Board에서 제거
        }
    }

    // 공격용 함수 ---------------------------------------------------------------------------------

    /// <summary>
    /// 자동 공격 함수. 플레이어가 타임 오버 되었을 때나 적이 공격할 때 사용
    /// </summary>
    public void AutoAttack()
    {
        if (!isActionDone)
        {
            int target; // 공격할 인덱스

            if (attackHighCandidateIndice.Count > 0)
            {
                // 공격할 후보지역이 있는 경우
                target = attackHighCandidateIndice[0];  // 첫번째 후보지역을 뽑고
                RemoveHighCandidate(target);            // 후보지역에서 제거
                attackCandidateIndice.Remove(target);   // 전체 Board좌표에서도 제거
            }
            else
            {
                // 공격할 후보지역이 없는 경우 
                target = attackCandidateIndice[0];  // 전체 Board 좌표 중 첫번째 선택(중복없는 랜덤으로 골라진다.)
                attackCandidateIndice.RemoveAt(0);  // 전체 Board 좌표에서 제거
            }

            Attack(target); // 선택된 target 지점을 공격
        }
    }

    /// <summary>
    /// 수동 공격(핵심)
    /// </summary>
    /// <param name="attackGridPos">공격할 그리드 좌표</param>
    public void Attack(Vector2Int attackGridPos)
    {
        if (!isActionDone 
            && Board.IsValidPosition(attackGridPos)         // 중복 Board.Attacked에서 체크함
            && opponent.Board.IsAttackable(attackGridPos))  // 중복 Board.Attacked에서 체크함
        {            
            RemoveHighCandidate(Board.GridToIndex(attackGridPos));  // 공격을 할것이라 후보지에서 제거

            bool result = opponent.Board.Attacked(attackGridPos);   // 실제로 공격해서 공격 결과 얻기

            if (result)
            {
                // 공격이 성공했다.
                successAttackCount++;
                AttackSuccessProcess(attackGridPos);
            }
            else
            {
                // 공격이 실패했다.
                failAttackCount++;
                lastAttackSuccessPos = NOT_SUCCESS_YET; // 마지막 공격 성공 위치 제거(없어도 상관 없으나 있는 쪽이 연산을 줄일 수 있을 것 같다)
                onAttackFail?.Invoke(this);             // 공격 실패를 알림
            }

            // 이번 공격으로 상대방의 배가 부서졌으면
            if (opponentShipDestroyed)
            {
                RemoveAllHightCandidate();      // 내 후보지들을 모두 제거
                opponentShipDestroyed = false;  // 다시 플래그 off
            }

            isActionDone = true;
            onActionEnd?.Invoke();              // 행동이 끝났음을 알림
        }
    }

    /// <summary>
    /// 수동 공격
    /// </summary>
    /// <param name="worldPos">공격할 월드 좌표</param>
    public void Attack(Vector3 worldPos)
    {
        Attack(opponent.Board.WorldToGrid(worldPos));   // 월드 좌표를 그리드 좌표로 변경해서 실행
    }

    /// <summary>
    /// 수동 공격
    /// </summary>
    /// <param name="index">공격할 위치의 인덱스</param>
    public void Attack(int index)
    {
        Attack(Board.IndexToGrid(index));   // 인덱스를 그리드 좌표로 변경해서 실행
    }

    // 후보지역 관련 함수 --------------------------------------------------------------------------

    /// <summary>
    /// 공격 성공 이후에 후보지역 추가하는 함수
    /// </summary>
    /// <param name="attackPos"></param>
    private void AttackSuccessProcess(Vector2Int attackPos)
    {
        // 직전의 공격이 성공했는지 확인
        if (lastAttackSuccessPos != NOT_SUCCESS_YET)
        {
            // 직전에 공격이 성공한 적이 있다.

            // 지금 공격한지점(attackPos)과 마지막 성공지점(lastAttackSuccessPos)을 기준으로 후보지역 추가
            AddHighCandidateByTwoPosition(attackPos, lastAttackSuccessPos);
        }
        else
        {
            // 직전에 공격이 성공한적이 없다.
            Vector2Int oldAttackSuccessPos;
            if (CheckNeighborSuccess(attackPos, out oldAttackSuccessPos))    // 공격지점 주변에 공격을 성공한 적이 있는지 체크
            {
                // attackPos 근처에 공격 성공지점이 있으면 
                // attackPos와 근처에 있는 공격 성공지점을 기준으로 후보지역 추가
                AddHighCandidateByTwoPosition(attackPos, oldAttackSuccessPos);
            }
            else
            {
                // attackPos 근처에 공격을 성공한 지점이 없다.
                lastAttackSuccessPos = attackPos;       // attackPos를 공격 성공한 지점으로 기록
                AddNeighborToHighCandidate(attackPos);  // 그 위치의 이웃을 후보지에 추가
            }
        }
    }

    /// <summary>
    /// now와 last의 위치에 따라 후보지역 추가하기
    /// </summary>
    /// <param name="now">지금 공격 성공한 곳</param>
    /// <param name="last">이전에 공격 성공한 곳</param>
    private void AddHighCandidateByTwoPosition(Vector2Int now, Vector2Int last)
    {
        // 이전에 공격을 성공한 적이 있으면                
        if (Mathf.Abs(now.x - last.x) == 1 && (now.y == last.y))
        {
            // 가로로 옆에 공격을 성공 했다.

            // 한 줄로 늘어서 있을 때 선 밖의 후보지 제거
            List<int> dels = new List<int>();
            foreach (var index in attackHighCandidateIndice )
            {
                Vector2Int pos = Board.IndexToGrid(index);
                if( pos.y != now.y)     // y값이 다르면 가로선을 벗어난 것으로 판단 할 수 있다.
                {
                    dels.Add(index);    // 삭제할 목록에 추가
                }
            }
            foreach(var del in dels)    // 삭제할 목록에 있는 후보지역을 모두 삭제
            {
                RemoveHighCandidate(del);
            }

            // 후보지역 선정 기본 원리
            // attackGridPos.x를 계속 증가하고 (감소하고)
            // 보드 끝까지 계속 증가시키다가
            //   공격 실패한 지점이 나오면 취소
            //   공격을 안한 유효구간이 나오면 후보지에 추가

            // 후보지역 추가
            Vector2Int newPos = now;
            for (int i = now.x - 1; i > -1; i--)
            {
                newPos.x = i;   // now.x를 계속 감소시켜서 newPos에 넣기
                if (opponent.board.IsAttackFailPosition(newPos))    // 공격 실패한 지점이 나오면 더 이상 진행안함.
                    break;
                if (Board.IsValidPosition(newPos) && opponent.board.IsAttackable(newPos))
                {
                    // 그리드 영역 안이고 공격이 가능한 지점이다.
                    AddHighCandidate(Board.GridToIndex(newPos));    // 이 지점을 후보지에 추가하고 찾기 중지
                    break;
                }
            }

            for (int i = now.x + 1; i < Board.BoardSize; i++)
            {
                newPos.x = i;   // now.x를 계속 증가시켜서 newPos에 넣기
                if (opponent.board.IsAttackFailPosition(newPos))    // 공격 실패한 지점이 나오면 더 이상 진행안함.
                    break;
                if (Board.IsValidPosition(newPos) && opponent.board.IsAttackable(newPos))
                {
                    // 그리드 영역 안이고 공격이 가능한 지점이다.
                    AddHighCandidate(Board.GridToIndex(newPos));    // 이 지점을 후보지에 추가하고 찾기 중지
                    break;
                }
            }
            lastAttackSuccessPos = now;   // 공격 성공한 지점으로 기록
        }
        else if (Mathf.Abs(now.y - last.y) == 1 && (now.x == last.x))     // 세로로 위아래에 있다.
        {
            // 세로로 공격에 성공했다. (가로와 x,y만 다르고 똑같다.)
            
            // 한 줄로 늘어서 있을 때 선 밖의 후보지 제거
            List<int> dels = new List<int>();
            foreach (var index in attackHighCandidateIndice)
            {
                Vector2Int pos = Board.IndexToGrid(index);
                if (pos.x != now.x)
                {
                    dels.Add(index);
                }
            }
            foreach (var del in dels)
            {
                RemoveHighCandidate(del);
            }

            // 후보지역 추가
            Vector2Int newPos = now;
            for (int i = now.y - 1; i > -1; i--)
            {
                newPos.y = i;
                if (opponent.board.IsAttackFailPosition(newPos))
                    break;
                if (Board.IsValidPosition(newPos) && opponent.board.IsAttackable(newPos))
                {
                    AddHighCandidate(Board.GridToIndex(newPos));
                    break;
                }
            }

            for (int i = now.y + 1; i < Board.BoardSize; i++)
            {
                newPos.y = i;
                if (opponent.board.IsAttackFailPosition(newPos))
                    break;
                if (Board.IsValidPosition(newPos) && opponent.board.IsAttackable(newPos))
                {
                    AddHighCandidate(Board.GridToIndex(newPos));
                    break;
                }
            }
            lastAttackSuccessPos = now;
        }
        else
        {
            // 공격은 성공했지만 옆이 아니라 다른 위치를 공격했다.( 사용이 안될듯? )
            lastAttackSuccessPos = now;         // 공격 성공한 지점 기록
            AddNeighborToHighCandidate(now);    // 그 위치의 4방향을 후보지에 추가
        }
    }

    /// <summary>
    /// 목표 지점의 이웃들을 모두 후보지에 추가
    /// </summary>
    /// <param name="gridPos">목표지점(그리드좌표)</param>
    private void AddNeighborToHighCandidate(Vector2Int gridPos)
    {
        // 공격이 성공했으면 공격한 지점의 동서남북에 highCandidatePrefab 생성해서 표시하기
        Vector2Int[] neighbor = { new(-1, 0), new(1, 0), new(0, 1), new(0, -1) };

        foreach (var side in neighbor)
        {
            Vector2Int n = gridPos + side;
            // valid한 지점이고 공격 가능한 지점이면 후보지역으로 추가
            if (Board.IsValidPosition(n) && opponent.Board.IsAttackable(n))
            {
                int index = Board.GridToIndex(n);
                AddHighCandidate(index);
            }
        }
    }

    /// <summary>
    /// 후보지역 추가
    /// </summary>
    /// <param name="index">추가할 후보지의 인덱스</param>
    private void AddHighCandidate(int index)
    {        
        if (!attackHighCandidateIndice.Exists((x) => x == index))
        {
            // index가 attackHighCandidateIndice에 없을 때만 실행

            attackHighCandidateIndice.Insert(0, index); // 스택처럼 항상 제일 앞에 추가

#if UNITY_EDITOR
            // 상대방 board에 highCandidatePrefab 생성해서 붙여주기
            GameObject obj = Instantiate(highCandidatePrefab, transform);
            obj.transform.position = opponent.board.IndexToWorld(index) + Vector3.up;
            highCandidateMark[index] = obj; // 삭제를 대비해서 딕셔너리에 추가
#endif
        }
    }

    /// <summary>
    /// 이웃에 공격을 성공한 지역이 있는지 체크
    /// </summary>
    /// <param name="gridPos">이웃을 체크할 위치</param>
    /// <param name="successPos">(out)공격이 성공된 이웃</param>
    /// <returns>true면 공격을 성공한 이웃이 있다. false면 없다.</returns>
    private bool CheckNeighborSuccess(Vector2Int gridPos, out Vector2Int successPos)
    {
        bool result = false;
        Vector2Int[] neighbor = { new(-1, 0), new(1, 0), new(0, 1), new(0, -1) };
        successPos = NOT_SUCCESS_YET;

        foreach (var side in neighbor)
        {
            Vector2Int n = gridPos + side;
            if (Board.IsValidPosition(n) && opponent.Board.IsAttackSuccessPosition(n))
            {
                // valid한 위치이고 공격이 성공한 위치이다.
                // 그러면 out에 기록하고 result에 true를 저장하고 나머지 스킵
                successPos = n;
                result = true;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 특정 후보지역 제거
    /// </summary>
    /// <param name="index">삭제할 후보지역의 인덱스</param>
    private void RemoveHighCandidate(int index)
    {
        if (attackHighCandidateIndice.Exists((x) => x == index))    // 리스트 안에 있는 각 요소를 x라고 했을 때 x가 index와 같으면 true
        {
            // attackHighCandidateIndice에 index가 있으면 제거
            attackHighCandidateIndice.Remove(index);
#if UNITY_EDITOR
            Destroy(highCandidateMark[index]);  // mark 딕셔너리를 이용해 게임오브젝트도 삭제
            highCandidateMark[index] = null;    // null로 삭제했다고 표시
            highCandidateMark.Remove(index);    // 딕셔너리에서 index를 키값으로 쓰는 데이터 제거
#endif
        }
    }

    /// <summary>
    /// 모든 후보지역 제거
    /// </summary>
    private void RemoveAllHightCandidate()
    {
#if UNITY_EDITOR
        foreach (var candidate in attackHighCandidateIndice)    // attackHighCandidateIndice 전체를 하나씩 삭제
        {
            Destroy(highCandidateMark[candidate]);  // mark 딕셔너리를 이용해 게임오브젝트도 삭제
            highCandidateMark[candidate] = null;    // null로 삭제했다고 표시
            highCandidateMark.Remove(candidate);    // 딕셔너리에서 index를 키값으로 쓰는 데이터 제거
        }
#endif
        attackHighCandidateIndice.Clear();          // 다 제거했으니 clear
    }

    // 테스트 함수 ---------------------------------------------------------------------------------


}
