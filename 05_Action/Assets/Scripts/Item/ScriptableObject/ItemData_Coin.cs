using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터를 저장하는 데이터 파일을 만들게 해주는 스크립트
/// </summary>
[CreateAssetMenu(fileName = "New Coin Item Data", menuName = "Scriptable Object/Item Data - Coin", order = 2)]
public class ItemData_Coin : ItemData, IConsumable
{
    public void Consume(Player player)
    {
        player.Money += (int)value;
    }
}
