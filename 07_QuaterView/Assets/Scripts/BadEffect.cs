using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEffect : MonoBehaviour
{
    public float damagePerSecond;       // 자기 영역안에 있는 적에게 가하는 초당 데미지
    List<IHit> targetList = null;       // 공격 대상들을 기록할 리스트

    private void Awake()
    {
        targetList = new List<IHit>(4); // 리스트 생성(4개가 들어갈 수 있는 메모리 확보)
        //targetList.Count;       // 리스트에 들어있는 아이템의 수
        //targetList.Capacity;    // 리스트가 현재 담을 수 있는 최대 아이템의 수
    }

    private void Update()
    {
        foreach(var target in targetList)
        {
            target.HP -= damagePerSecond * Time.deltaTime;  // 매초 damagePerSecond씩 target의 HP 감소
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if( !other.CompareTag("Player") )   // 플레이어가 아닌 대상이 들어왔을 경우
        {
            IHit hit = other.gameObject.GetComponent<IHit>();
            if (hit != null)
            {
                targetList.Add(hit);        // 리스트에 추가
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))    // 플레이어가 아닌 대상이 나갔을 경우
        {
            targetList.Remove(other.gameObject.GetComponent<IHit>());   // 리스트에서 제거
        }
    }
}
