using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    /// <summary>
    /// 셀 한변의 길이
    /// </summary>
    const int CellSideSize = 64;

    /// <summary>
    /// 기본 지뢰 숫자
    /// </summary>
    const int DefaultMineCount = 40;

    const int InvalidIndex = -1;

    /// <summary>
    /// 셀 프리팹
    /// </summary>
    public GameObject cellPrefab;

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

    private void Awake()
    {
        //Debug.Log(Time.realtimeSinceStartup);
        // 배열 할당 받기
        cells = new Cell[width * height];

        // 가로세로 갯수만큼 셀 생성
        for(int i=0;i<height;i++)
        {
            for(int j=0;j<width;j++)
            {
                GameObject obj = Instantiate(cellPrefab, this.transform);   // 실제 생성
                obj.name = $"Cell_{j:d2}_{i:d2}";                           // 이름 붙이기
                obj.transform.localPosition = new Vector3(j * CellSideSize, -i * CellSideSize, 0);  // 위치 정하기
                cells[i * width + j] = obj.GetComponent<Cell>();            // cells에 모든 셀 보관
                cells[i * width + j].onSafeOpen += GetAroundMineCount;
            }
        }
        //Debug.Log(Time.realtimeSinceStartup);

        // 테스트용 코드
        for(int i=0;i<16;i++)
        {
            cells[i].SetMine();
        }

    }

    private void GetAroundMineCount(Cell cell)
    {
        int mineCount = 0;
        List<Cell> neighbor = new List<Cell>(8);
        Vector2Int grid = IndexToGrid(cell.ID);
        for(int i=-1;i<2;i++)
        {
            for(int j = -1;j<2;j++)
            {
                int index = GridToIndex(j + grid.x, i + grid.y);
                if( index != InvalidIndex && (i==0 && j==0))
                {
                    if( cells[index].HasMine )
                    {
                        mineCount++;
                    }
                    neighbor.Add(cells[index]);
                }
            }
        }

        cell.SetOpenImage(mineCount);   // 해당 셀에 숫자 설정
        if (mineCount == 0)
        {            
            // 주변 셀 다 열기
            foreach(var ncell in neighbor)
            {
                ncell.OpenCell();
            }
        }
    }

    Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % width, index / height);
    }

    int GridToIndex(int x, int y)
    {
        if( x >= 0 && x < width && y >= 0 && y < height )
            return x + y * height;

        return InvalidIndex;
    }
}
