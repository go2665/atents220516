using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 닫힌 셀의 상태
/// </summary>
public enum CloseCellType
{
    Normal = 0, // 평범한 커버
    Flag,       // 깃발
    Question    // 물음표
}

/// <summary>
/// 열린 셀의 상태
/// </summary>
public enum OpenCellType
{
    MineNotFound = 0,   // 못찾은 지뢰
    MineFindMistake,    // 지뢰가 아닌데 찾은 곳
    MineExplosion,      // 밟은 지뢰
    Empty,              // 빈칸
    MineCount_1,        // 주변에 지뢰가 1개
    MineCount_2,        // 주변에 지뢰가 2개
    MineCount_3,        // 주변에 지뢰가 3개
    MineCount_4,        // 주변에 지뢰가 4개
    MineCount_5,        // 주변에 지뢰가 5개
    MineCount_6,        // 주변에 지뢰가 6개
    MineCount_7,        // 주변에 지뢰가 7개
    MineCount_8         // 주변에 지뢰가 8개
}
