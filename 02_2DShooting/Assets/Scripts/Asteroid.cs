using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int splitCount = 2;



    // 뭔가 계속하는 것은 Update 계열의 함수에서 실행
    private void Update()
    {
        // 오일러 앵글 : 3차원 회전을 x축 y축 z축의 합으로 나타낸 것
        // 예시 : ( 10, 20, 30 ) => x축으로 10도, y축으로 20도, z축으로 30도
        // 오일러 앵글의 문제점 : 짐벌락이라는 현상이 발생
        // 짐벌락 해결을 위해 쿼터니언(Quaternion, 사원수) 등장( 오일러 앵글에 비해 속도도 빠르고 메모리도 덜 차지함)
        // 대신 쿼터니언은 사람이 알아보거나 만들기 어려움

        //transform.rotation *= Quaternion.Euler(0, 0, 30.0f * Time.deltaTime);   // 1초에 30도씩 돌려라(반시계방향)
        transform.Rotate(0, 0, 30.0f * Time.deltaTime);
        //transform.Rotate(new Vector3(0, 0, 30.0f * Time.deltaTime));


    }
}
