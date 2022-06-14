using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Vector3 v1 = new Vector3(1, 2, 3);
        Vector3 v2 = v1;
        v1.x = 10;
        Debug.Log(v2);

        //Mathf.Abs(-10.0f);  // 파라메터에서 부호를 제거한 절대값 구하는 함수
    }
}
