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
        int totalCount = width * height;
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
                cells[id].ID = id;                                  // cell에 ID할당
                cells[id].onSafeOpen += GetAroundMineCount;         // 터지지 않고 열렸을 때 실행될 함수 연결(주변 8칸 검사)
                cells[id].onSafeOpen += (_) => 
                { 
                    openCount++; 

                    if(flagCount == 0 && totalCount <= openCount + mineCount)
                    {
                        gameManager.GameClear();
                    }
                };
                cells[id].onFlagCountChange += (x) =>
                {
                    flagCount += x;
                    onFlagCountChange?.Invoke(x); // 셀의 델리게이트에 스테이지의 델리게이트를 연결
                    if (flagCount == 0 && totalCount <= openCount + mineCount)
                    {
                        gameManager.GameClear();
                    }
                };
            }
        }
    }

    private void Start()
    {
        //Debug.Log("Stage start");
        gameManager = GameManager.Inst;         // 게임 매니저 찾기
        gameManager.onGameReset += ResetAll;    // 게임이 리셋 될 때 스테이지 전체 리셋(초기화 하고 지뢰 다시 배치)
        gameManager.onGameOver += OnGameOver;   // 게임 오버가 되었을 때 각종 처리(잘못 찾은 지뢰와 못찾은 지뢰 표시)

        flagCount = mineCount;                  // 깃발 갯수 초기화
    }

    /// <summary>
    /// 게임 전체 리셋(셀 재활용)
    /// </summary>
    public void ResetAll()
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
            //mineCellList.Add(cells[cellIndex]);
            //Debug.Log($"{IndexToGrid(cellIndex)}에 지뢰 추가");
        }
    }

    /// <summary>
    /// 주변 8칸의 지뢰 여부 탐색(지뢰가 아닌 셀을 열었을 때 실행)
    /// </summary>
    /// <param name="cell">지금 연 셀</param>
    private void GetAroundMineCount(Cell cell)
    {
        gameManager.GameStart();    // GameStart 매번 시도(처음에만 실행됨)

        int mineCount = 0;                          // 지뢰 갯수 누적할 변수
        List<Cell> neighbor = new List<Cell>(8);    // 주변 8칸 저장할 리스트
        Vector2Int grid = IndexToGrid(cell.ID);     // 셀의 그리드 좌표 구하기
        for(int i=-1;i<2;i++)                       // 주변 8칸 탐색
        {
            for(int j = -1;j<2;j++)
            {
                int index = GridToIndex(j + grid.x, i + grid.y);    // 그리드 좌표를 인덱스로 다시 변환
                if( index != InvalidIndex && !(i==0 && j==0))       // 해당 좌표가 적절한지, 자신은 아닌지 확인
                {
                    if( cells[index].HasMine )  // 지뢰가 있는지 확인
                    {
                        mineCount++;            // 지뢰가 있으면 갯수 누적
                    }
                    neighbor.Add(cells[index]); // 주변 셀 저장
                }
            }
        }

        cell.SetOpenImage(mineCount);       // 해당 셀에 숫자 설정(지뢰 숫자)
        if (mineCount == 0)                 // 지뢰 갯수가 0이면
        {            
            foreach(var ncell in neighbor)  // 주변 셀 다 열기
            {
                ncell.OpenCell();
            }
        }
    }

    /// <summary>
    /// 인덱스 값을 그리드 좌표로 변환하는 유틸리티 함수
    /// </summary>
    /// <param name="index">변환할 인덱스</param>
    /// <returns>변환된 그리드 좌표</returns>
    Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % width, index / height);
    }

    /// <summary>
    /// 그리드 좌표를 인덱스 값으로 변환하는 유틸리티 함수
    /// </summary>
    /// <param name="x">변환할 그리드X</param>
    /// <param name="y">변환할 그리드Y</param>
    /// <returns>변환된 인덱스 값</returns>
    int GridToIndex(int x, int y)
    {
        if( x >= 0 && x < width && y >= 0 && y < height )
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

    /// <summary>
    /// 게임 오버시 지뢰들 표시하기
    /// </summary>
    void OnGameOver()
    {
        foreach(var cell in cells)
        {
            if( cell.IsFlaged )
            {
                // 잘못 찾은 지뢰 처리(깃발이 설치되어있고 지뢰가 없다.)
                if (!cell.HasMine)
                {
                    cell.SetOpenCellImage(OpenCellType.MineFindMistake);    // 잘못찾은 지뢰용 이미지(X표시 되어있는 지뢰)로 변경
                }
            }
            else
            {
                // 못찾은 지뢰 처리(셀이 닫혀 있고 깃발이 설치되어있지 않고 지뢰가 있다.)
                if (!cell.IsOpen && cell.HasMine)
                {
                    cell.SetOpenCellImage(OpenCellType.MineNotFound);       // 못찾은 지뢰용 이미지(일반 지뢰)로 변경
                }
            }
        }
    }
}
