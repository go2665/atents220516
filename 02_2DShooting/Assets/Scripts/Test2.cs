using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    private void Awake()
    {
        // 이 게임 오브젝트의 자식 오브젝트 중에서 같은 종류의 컴포넌트를 찾음
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.flipX = true;  // flipX 설정(체크함)
        renderer.flipY = false; // flipY 해제(체크해제됨)
    }
}
