using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject shoot = null;

    private void Start()
    {
        //Random.Range(0, 10);          // 유니티 랜덤. 0~10까지(0과 10 포함)
        //Random.Range(0.0f, 10.0f);    // 유니티 랜덤. 0.0~10.0까지(0.0과 10.0 포함)

        StartCoroutine(TestCoroutine());            // TestCoroutine을 실행        
    }

    IEnumerator TestCoroutine()     // 코루틴을 정의
    {
        yield return new WaitForSeconds(1.0f);      // 1초 대기

        while(true)
        {
            GameObject obj = Instantiate(shoot);    // 총알 만들기
            obj.transform.position = this.transform.position;
            yield return new WaitForSeconds(0.2f);  // 0.2초 대기
        }
    }
}
