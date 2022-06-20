using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : Door
{
    public Key key = null;          // 먹었을 때 이 문이 열리는 열쇠
    public float closeTime = 5.0f;  // 먹고나서 문이 닫히는데까지 걸리는 시간

    private void Start()
    {
        key.onKeyPickup += OpenAndClose; // 열쇠가 사라질 때 실행될 델리게이트에 함수 연결
    }

    void OpenAndClose()
    {
        Open();                                 // 문을 열고
        StartCoroutine(CloseDoor(closeTime));   // 코루틴 실행
    }

    IEnumerator CloseDoor(float delay)
    {
        yield return new WaitForSeconds(delay); // delay만큼 기다리고
        Close();    // 문 닫기
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }

    protected override void OnTriggerExit(Collider other)
    {
    }
}
