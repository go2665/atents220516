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
        GameObject obj = new GameObject();              // 빈 오브젝트 만들고 ((0,0,0)에 생성됨)
        Item item = obj.AddComponent<Item>();           // Item 컴포넌트 추가

        item.data = GameManager.Inst.ItemData[code];    // ItemData 설정
        string[] itemName = item.data.name.Split("_");  // 내가 생성하는 종류에 맞게 이름 변경
        obj.name = $"{itemName[1]}_{itemCount}";        // 고유 아이디도 추가
        obj.layer = LayerMask.NameToLayer("Item");      // 레이어 설정
        SphereCollider col = obj.AddComponent<SphereCollider>();    // 코드로 컬라이더 추가
        col.radius = 0.5f;
        col.isTrigger = true;
        itemCount++;    // 생성할 때마다 값을 증가시켜서 중복이 없도록 처리

        GameObject indicator = Resources.Load<GameObject>("MinimapItemIndicator_Item"); // 리소시스 폴더에서 동적으로 로딩
        if (indicator != null)  // 로딩이 안되었을 때를 대비해서 추가
        {
            // 쿼드를 눕히기 위해 90도 회전
            GameObject.Instantiate(indicator,
                obj.transform.position, obj.transform.rotation * Quaternion.Euler(90, 0, 0), obj.transform);    
        }        

        return obj;     // 생성완료된 것 리턴
    }

    /// <summary>
    /// 아이템 생성 위치를 살짝 변동시키 위한 함수
    /// </summary>
    /// <param name="code">생성할 아이템</param>
    /// <param name="position">생성될 위치</param>
    /// <param name="randomNoise">false면 정확한 position에 생성. true면 position에서 살짝 위치 변경</param>
    /// <returns></returns>
    public static GameObject MakeItem(ItemIDCode code, Vector3 position, bool randomNoise = false)
    {
        GameObject obj = MakeItem(code);
        if(randomNoise)
        {
            Vector2 noise = Random.insideUnitCircle * 0.5f;
            position.x += noise.x;
            position.z += noise.y;
        }
        obj.transform.position = position;
        
        return obj;
    }

    /// <summary>
    /// 아이템을 여러개 생성하기 위한 함수
    /// </summary>
    /// <param name="code">생성할 아이템</param>
    /// <param name="position">생성된 아이템의 위치</param>
    /// <param name="count">생성할 갯수</param>
    public static void MakeItems(ItemIDCode code, Vector3 position, uint count)
    {
        for(int i=0;i<count;i++)
        {
            MakeItem(code, position, true);
        }
    }

    public static GameObject MakeItem(uint id)
    {
        return MakeItem((ItemIDCode)id);
    }

    public static GameObject MakeItem(uint id, Vector3 position, bool randomNoise = false)
    {
        return MakeItem((ItemIDCode)id, position, randomNoise);
    }

    public static void MakeItems(uint id, Vector3 position, uint count)
    {
        MakeItems((ItemIDCode)id, position, count);
    }
}
