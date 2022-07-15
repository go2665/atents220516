using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 생성용 클래스(생성만)
/// </summary>
public class ItemFactory
{
    static int itemCount = 0;   // 이때까지 생성된 총 아이템 개수. (각 아이템별 고유 아이디 용도로 사용)

    /// <summary>
    /// 아이템 생성
    /// </summary>
    /// <param name="code">생성할 아이템의 종류</param>
    /// <returns>생성한 게임오브젝트</returns>
    public static GameObject MakeItem(ItemIDCode code)
    {
        GameObject obj = new GameObject();              // 빈 오브젝트 만들고
        Item item = obj.AddComponent<Item>();           // Item 컴포넌트 추가

        item.data = GameManager.Inst.ItemData[code];    // ItemData 설정
        string[] itemName = item.data.name.Split("_");  // 내가 생성하는 종류에 맞게 이름 변경
        obj.name = $"{itemName[1]}_{itemCount}";        // 고유 아이디도 추가
        itemCount++;    // 생성할 때마다 값을 증가시켜서 중복이 없도록 처리

        return obj;     // 생성완료된 것 리턴
    }
}
