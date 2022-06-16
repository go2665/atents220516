using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IUseable
{
    // 인터페이스? 원래 뜻 : 다른 물체와 상호 작용하기 위해 사용하는 접점 또는 경계면
    // C# 인터페이스 : 상속받을 클래스가 어떤 기능을 가지고 있는지 알려주는 것
    //  1. 함수나 프로퍼티의 선언만 가능하다.
    //  2. 변수를 가질 수 없다.
    //  3. 구현도 가질 수 없다.
    //  4. 만들어지는 모든 것은 public이다.

    void Use();
}
