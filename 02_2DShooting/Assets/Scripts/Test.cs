using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject shoot = null;

    private IEnumerator coroutine = null;   // 코루틴 저장하는 변수

    private void Start()
    {
        //Random.Range(0, 10);          // 유니티 랜덤. 0~10까지(0과 10 포함)
        //Random.Range(0.0f, 10.0f);    // 유니티 랜덤. 0.0~10.0까지(0.0과 10.0 포함)


        coroutine = TestCoroutine();
        StartCoroutine(coroutine);      // TestCoroutine을 실행        

        //StopCoroutine(coroutine);     // 코루틴 정지시키기

        
        // ?. : ?앞에 있는 변수가 null이 아니면 접근하고 실행해라.
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
