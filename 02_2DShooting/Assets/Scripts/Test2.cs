using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    int i = 10;
    static int j = 20;

    private void Awake()
    {
        // 이 게임 오브젝트의 자식 오브젝트 중에서 같은 종류의 컴포넌트를 찾음
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.flipX = true;  // flipX 설정(체크함)
        renderer.flipY = false; // flipY 해제(체크해제됨)
    }

    public void TestTest()
    {
        i = i + 10;
        j = 30;

        Test2.TestTest2();  // static 함수는 객체를 생성하지 않아도 사용할 수 있다.
    }

    public static void TestTest2()
    {
        //i = 20;   //안됨. static은 static만 접근 가능
        j = j + 30;

        //GameManager.Inst.Test();        
    }
}
