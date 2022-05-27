using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject shoot = null;

    private void Start()
    {
        StartCoroutine(TestCoroutine());            // TestCoroutine을 실행        
    }

    IEnumerator TestCoroutine()     // 코루틴을 정의
    {
        yield return new WaitForSeconds(1.0f);      // 1초 대기

        while(true)
        {
            GameObject obj = Instantiate(shoot);    // 총알 만들기
            obj.transform.position = this.transform.position;
            yield return new WaitForSeconds(0.5f);  // 0.5초 대기
        }
    }
}
