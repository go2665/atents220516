using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터를 저장하는 데이터 파일을 만들게 해주는 스크립트
/// </summary>
[CreateAssetMenu(fileName ="New Item Data", menuName = "Scriptable Object/Item Data", order = 1)]
public class ItemData : ScriptableObject    // 내가 원하는 데이터를 저장할 수 있는 데이터파일을 설계할 수 있게 해주는 클래스
{
    [Header("기본 데이터")]
    public uint id = 0;                     // 아이템 ID
    public string itemName = "아이템";       // 아이템 이름
    public Sprite itemIcon;                 // 아이템 아이콘
    public GameObject prefab;               // 아이템의 프리팹
    public uint value;                      // 아이템의 가치
    public uint maxStackCount = 1;          // 아이템 최대 누적 수
}
