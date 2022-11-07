using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 셀에 표시할 이미지를 인덱서를 사용하여 받을 수 있을 클래스
/// </summary>
public class CellImageManager : Singleton<CellImageManager>
{
    /// <summary>
    /// 닫혀있는 셀의 이미지
    /// </summary>
    public Sprite[] closeCellImage;

    /// <summary>
    /// 열린 셀의 이미지
    /// </summary>
    public Sprite[] openCellImage;
    

    /// <summary>
    /// 닫힌 셀의 이미지를 돌려주는 인덱서
    /// </summary>
    public Sprite this[CloseCellType type] => closeCellImage[(int)type];

    /// <summary>
    /// 열린 셀의 이미지를 돌려주는 인덱서
    /// </summary>
    public Sprite this[OpenCellType type] => openCellImage[(int)type];
}
