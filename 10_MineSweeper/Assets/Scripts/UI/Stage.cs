using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    // 상수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 셀 한변의 길이
    /// </summary>
    const int CellSideSize = 64;

    /// <summary>
    /// 잘못된 인덱스임을 표시하기 위한 상수
    /// </summary>
    const int InvalidIndex = -1;


    // 변수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 셀 프리팹
    /// </summary>
    public GameObject cellPrefab;

    /// <summary>
    /// 기본 지뢰 숫자
    /// </summary>
    [Range(1,255)]
    public int mineCount = 10;

    /// <summary>
    /// 한 스테이지에서 가로 셀 갯수
    /// </summary>
    public int width = 16;

    /// <summary>
    /// 한 스테이지에서 세로 셀 갯수
    /// </summary>
    public int height = 16;

    /// <summary>
    /// 생성한 모든 셀
    /// </summary>
    Cell[] cells;

    /// <summary>
    /// 게임 매니저 접근용 변수
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 연 셀의 갯수
    /// </summary>
    int openCount = 0;

    /// <summary>
    /// 남은 깃발 갯수
    /// </summary>
    int flagCount = 0;

    /// <summary>
    /// 전체 셀 갯수
    /// </summary>
    int totalCount;

    /// <summary>
    /// 눌려져 있는 셀들의 ID
    /// </summary>
    List<int> pressedCellIDs = new List<int>(8);

    // 결과 표시용 변수 ----------------------------------------------------------------------------
    
    /// <summary>
    /// 못찾은 지뢰 갯수
    /// </summary>
    int notFoundMineCount = 0;

    /// <summary>
    /// 셀을 열려고 시도한 횟수(자동으로 열린 것은 카운트하지 않음)
    /// </summary>
    int openTryCount = 0;

    /// <summary>
    /// 셀을 열려고 시도한 횟수 확인용 프로퍼티
    /// </summary>
    public int OpenTryCount => openTryCount;

    /// <summary>
    /// 못찾은 지뢰 갯수 확인용 프로퍼티
    /// </summary>
    public int NotFoundMineCount => notFoundMineCount;

    /// <summary>
    /// 발견한 지뢰 갯수 확인용 프로퍼티
    /// </summary>
    public int FoundMineCount => mineCount - notFoundMineCount;


    // 델리게이트 ----------------------------------------------------------------------------------

    /// <summary>
    /// 깃발 갯수가 변경되었다는 델리게이트. 파라메터는 변경된 갯수
    /// </summary>
    public Action<int> onFlagCountChange;


    // 함수들 --------------------------------------------------------------------------------------

    private void Awake()
    {
        // 스테이지 커버 연결
        StageCover cover = GetComponentInChildren<StageCover>();    // 일단 찾고
        cover.onStartClick += ResetAll;                             // 커버가 클릭되면 지뢰를 설치하도록 함수 연결

        // 셀 생성 작업        
        totalCount = width * height;
        cells = new Cell[totalCount];       // 배열 할당 받기        
        for (int i = 0; i < height; i++)    // 가로세로 갯수만큼 셀 생성
        {
            for (int j = 0; j < width; j++)
            {
                GameObject obj = Instantiate(cellPrefab, this.transform);   // 실제 생성
                obj.name = $"Cell_{j:d2}_{i:d2}";                           // 이름 붙이기
                obj.transform.localPosition = new Vector3(j * CellSideSize, -i * CellSideSize, 0);  // 위치 정하기
                int id = i * width + j;
                cells[id] = obj.GetComponent<Cell>();               // cells에 모든 셀 보관
                cells[id].ID = id;     
                cells[id].onFlagCountChange += (x) =>
                {
                    flagCount += x;
                    onFlagCountChange?.Invoke(x);       // 셀의 델리게이트에 스테이지의 델리게이트를 연결. FlagCounter에 알림용
                    CheckGameClear();                   // 깃발 갯수가 바뀔 때마다 게임 클리어 체크
                };
                cells[id].onCellPress += OnCellPressed;     // 셀이 마우스로 눌러졌을 때 처리할 함수 등록
                cells[id].onCellRelease += OnCellRealeased; // 셀에서 마우스 버튼이 떨어졌을 때 처리할 함수 등록
                cells[id].onCellEnter += OnCellEntered;     // 셀에 마우스 커서가 들어왔을 때 처리할 함수 등록
            }
        }
    }

    private void Start()
    {
        //Debug.Log("Stage start");
        gameManager = GameManager.Inst;         // 게임 매니저 찾기
        gameManager.onGameReset += ResetAll;    // 게임이 리셋 될 때 스테이지 전체 리셋(초기화 하고 지뢰 다시 배치)

        // 가장 먼저 OnGameOver가 실행되도록 설정
        gameManager.onGameOver = OnGameOver + gameManager.onGameOver;   // 게임 오버가 되었을 때 각종 처리(잘못 찾은 지뢰와 못찾은 지뢰 표시)

        flagCount = mineCount;                  // 깃발 갯수 초기화
    }


    // 주요 함수들 ---------------------------------------------------------------------------------

    /// <summary>
    /// 셀을 재귀적으로 여는 함수
    /// </summary>
    /// <param name="target">열릴 셀</param>
    private void OpenCell(Cell target)
    {
        if (target.OpenCell())              // True면 성공적으로 열렸다(게임 오버 당하지 않음)
        {
            List<Cell> neighbor = GetAroundCells(target.ID);   // 주변 셀 가져오기
            int neighborMineCount = 0;      // 주변 지뢰 갯수
            foreach (var cell in neighbor)
            {
                if (cell.HasMine)
                    neighborMineCount++;    // 주변 지뢰 갯수 누적
            }

            if (neighborMineCount > 0)
            {
                // 주변에 지뢰가 하나 이상이다.
                target.SetOpenCellImage(neighborMineCount);
            }
            else
            {
                // 주변에 지뢰가 하나도 없다.
                foreach (var cell in neighbor)
                {
                    OpenCell(cell);     // 재귀적으로 계속 열기
                }
            }

            openCount++;        // 연 갯수 증가시키기
            CheckGameClear();   // 셀이 열릴 때마다 게임 클리어 체크
        }
    }

    /// <summary>
    /// 게임 클리어가능한지 확인 후 가능하면 게임 클리어시키는 함수
    /// </summary>
    private void CheckGameClear()
    {
        if (flagCount == 0 && totalCount <= openCount + mineCount)
        {
            notFoundMineCount = 0;      // 게임이 클리어 되었으면 모든 지뢰를 찾은 것
            gameManager.GameClear();    // 게임 매니저에서 게임 클리어 처리
        }
    }

    /// <summary>
    /// 게임 오버시 지뢰들 표시하기
    /// </summary>
    private void OnGameOver()
    {
        notFoundMineCount = 0;              // 게임 오버되면 항상 새로 계산
        foreach (var cell in cells)
        {
            if (cell.IsFlaged)
            {
                // 잘못 찾은 지뢰 처리(깃발이 설치되어있고 지뢰가 없다.)
                if (!cell.HasMine)
                {
                    cell.SetOpenCellImage(OpenCellType.MineFindMistake);    // 잘못찾은 지뢰용 이미지(X표시 되어있는 지뢰)로 변경
                }
            }
            else
            {
                // 못찾은 지뢰 처리(깃발이 설치되어있지 않고 셀이 닫혀 있고 지뢰가 있다.)
                if (!cell.IsOpen && cell.HasMine)
                {
                    notFoundMineCount++;    // 못찾은 지뢰가 발견될 때마다 1씩 증가
                    cell.SetOpenCellImage(OpenCellType.MineNotFound);       // 못찾은 지뢰용 이미지(일반 지뢰)로 변경
                }
            }
        }
    }

    /// <summary>
    /// 게임 전체 리셋(셀 재활용)
    /// </summary>
    private void ResetAll()
    {
        flagCount = mineCount;      // 깃발 갯수 초기화
        foreach (var cell in cells) // 모든 셀을 리셋
        {
            cell.CellReset();
        }
        ResetMine();                // 지뢰 다시 설치
    }

    /// <summary>
    /// 지뢰만 다시 설치하는 함수
    /// </summary>
    private void ResetMine()
    {
        int totalCount = width * height;

        // 전체 갯수만큼 인덱스 리스트 만듬
        List<int> indexList = new List<int>(totalCount);
        for (int i = 0; i < totalCount; i++)
        {
            indexList.Add(i);
        }

        // 피셔 예이츠 알고리즘으로 섞기
        int[] suffleArray = indexList.ToArray();
        for (int i = 0; i < totalCount; i++)
        {
            int size = totalCount - 1 - i;
            int randIndex = UnityEngine.Random.Range(0, size - 1);
            (suffleArray[size], suffleArray[randIndex]) = (suffleArray[randIndex], suffleArray[size]);
        }

        // 사용할 지뢰 수만큼만 설치
        for (int i = 0; i < mineCount; i++)
        {
            int cellIndex = suffleArray[i]; // 랜덤으로 섞인 인덱스 가져와서
            cells[cellIndex].SetMine();     // 지뢰설치
            //Debug.Log($"{IndexToGrid(cellIndex)}에 지뢰 추가");
        }
    }

    // 입력 관련 함수 ------------------------------------------------------------------------------
    /// <summary>
    /// 셀에 마우스 커서가 들어왔을 때 실행될 함수
    /// </summary>
    /// <param name="cellId">들어간 셀의 ID</param>
    private void OnCellEntered(int cellId)
    {        
        if (pressedCellIDs.Count > 0)           // 눌러진 셀이 있다면
        {
            foreach(var id in pressedCellIDs)
            {
                cells[id].CellRelease();        // 눌러진 셀들을 전부 원상복구
            }

            Cell target = cells[cellId];        // 지금 들어간 셀이
            if (target.IsOpen)
            {                
                AroundCellPress(target);        // 열려있으면 주변의 셀을 누르기
            }
            else
            {
                pressedCellIDs.Clear();         // 닫혀 있으면 목록을 비우고 이 셀의 ID만 추가한 후 누르기
                pressedCellIDs.Add(cellId);
                target.CellPress();
            }
        }
    }

    /// <summary>
    /// 셀에서 마우스 버튼이 떨어졌을 때 실행되는 함수
    /// </summary>
    /// <param name="cellId">대상 셀</param>
    private void OnCellRealeased(int cellId)
    {
        Debug.Log($"Release : {cellId}");        

        Cell targetCell = cells[cellId];
        if(targetCell.IsOpen)
        {
            // 주변 깃발 갯수 세기
            int flagCount = 0;
            List<Cell> cellList = GetAroundCells(cellId);
            foreach (var cell in cellList)
            {
                if (cell.IsFlaged)
                    flagCount++;
            }

            if (targetCell.AroundMineCount == flagCount)
            {
                // 주변의 깃발 수와 지뢰의 숫자가 같으면

                openTryCount++;         // 열기 시도 횟수 증가

                // 눌려져 있던 셀들 전부 열기
                foreach (var id in pressedCellIDs)
                {
                    OpenCell(cells[id]);
                }
            }
            else
            {
                // 주변의 깃발 수와 지뢰의 숫자가 다르면
                // 눌린 셀들 원상 복귀
                foreach (var id in pressedCellIDs)
                {
                    cells[id].CellRelease();
                }
            }
        }
        else
        {
            // 닫혀있던 셀이면
            openTryCount++;         // 열기 시도 횟수 증가
            OpenCell(targetCell);   // 열기
        }
        pressedCellIDs.Clear();     // 목록 초기화
    }

    /// <summary>
    /// 셀이 마우스로 눌러졌을 때 실행할 함수
    /// </summary>
    /// <param name="cellId">대상 셀</param>
    private void OnCellPressed(int cellId)
    {
        Debug.Log($"Press : {cellId}");
        pressedCellIDs.Clear();
        

        Cell target = cells[cellId];
        if(target.IsOpen)
        {
            // 열려있고 주변에 지뢰게 있는 셀이면 주변 셀을 누르기
            AroundCellPress(target);
        }
        else
        {
            // 닫힌 셀이면 자신만 누르기
            pressedCellIDs.Add(cellId);
            target.CellPress();
        }        
    }

    // 유틸리티 함수 -------------------------------------------------------------------------------

    /// <summary>
    /// 주변에 닫힌 셀을 전부 누르는 함수
    /// </summary>
    /// <param name="target">기준 셀</param>
    private void AroundCellPress(Cell target)
    {
        if (target.AroundMineCount > 0) // 주변에 지뢰가 있으면(숫자가 씌여져 있는 셀이면)
        {
            List<Cell> cells = GetAroundCells(target.ID);   // 주변셀 다 가져와서
            foreach (var cell in cells)
            {
                if (!cell.IsOpen)                           // 닫혀있으면
                {
                    pressedCellIDs.Add(cell.ID);            // 전부 기록하고 누르기
                    cell.CellPress();
                }
            }
        }
    }

    /// <summary>
    /// 대상 주변의 모든 셀을 리턴하는 함수
    /// </summary>
    /// <param name="cellId">중심이 되는 대상 셀</param>
    /// <returns>대상 셀의 주변 셀 목록</returns>
    private List<Cell> GetAroundCells(int cellId)
    {
        //Debug.Log(this.gameObject.name);
        List<Cell> cellList = new List<Cell>(8);
        Vector2Int grid = IndexToGrid(cellId);                      // 셀의 그리드 좌표 구하기
        for (int i = -1; i < 2; i++)                                // 주변 8칸 탐색
        {
            for (int j = -1; j < 2; j++)
            {
                int index = GridToIndex(j + grid.x, i + grid.y);    // 그리드 좌표를 인덱스로 다시 변환
                if (index != InvalidIndex && !(i == 0 && j == 0))   // 해당 좌표가 적절한지, 자신은 아닌지 확인
                {
                    cellList.Add(cells[index]);
                }
            }
        }
        return cellList;
    }

    /// <summary>
    /// 인덱스 값을 그리드 좌표로 변환하는 유틸리티 함수
    /// </summary>
    /// <param name="index">변환할 인덱스</param>
    /// <returns>변환된 그리드 좌표</returns>
    private Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % width, index / height);
    }

    /// <summary>
    /// 그리드 좌표를 인덱스 값으로 변환하는 유틸리티 함수
    /// </summary>
    /// <param name="x">변환할 그리드X</param>
    /// <param name="y">변환할 그리드Y</param>
    /// <returns>변환된 인덱스 값</returns>
    private int GridToIndex(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return x + y * height;

        return InvalidIndex;
    }


    //public void Test_SetMines()
    //{
    //    for(int i=0;i<16;i++)     // 첫째줄에 지뢰 설치
    //    {
    //        cells[i].SetMine();
    //    }
    //}       
}
