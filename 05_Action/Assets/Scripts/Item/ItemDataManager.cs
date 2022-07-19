using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager에서 관리할 ItemDataManager. 아이템 종류별 데이터만 가지고 있음.
/// </summary>
public class ItemDataManager : MonoBehaviour
{
    /// <summary>
    /// 아이템 종류별 데이터
    /// </summary>
    public ItemData[] itemDatas;    

    public ItemData this[uint i]     // 인덱서. 
    {
        get => itemDatas[i];
    }

    public ItemData this[ItemIDCode code]   // 인덱서를 통해 편리하게 아이템 종류별 데이터에 접근(enum으로 배열접근하게 변경)
    {
        get => itemDatas[(int)code];
    }

    /// <summary>
    /// 아이템 종류 개수
    /// </summary>
    public int Length
    {
        get => itemDatas.Length;
    }
}
