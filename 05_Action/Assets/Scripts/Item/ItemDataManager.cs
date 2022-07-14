using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public ItemData[] itemDatas;

    public ItemData this[int i] // 인덱서
    {
        get => itemDatas[i];
    }

    public ItemData this[ItemIDCode code]
    {
        get => itemDatas[(int)code];
    }

    public int Length
    {
        get => itemDatas.Length;
    }
}
