using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 1개를 나타낼 클래스
/// </summary>
public class Item : MonoBehaviour
{
    public ItemData data;   // 이 아이템 종류별로 동일한 데이터

    private void Start()
    {
        // 프리팹 생성. Awake일 때는 data가 없어서 Start에서 실행
        Instantiate(data.prefab, transform.position, transform.rotation, transform);
    }
}
