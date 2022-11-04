using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ImageNumber : TestBase
{
    public ImageNumber imageNumber;

    private void Start()
    {
        imageNumber = FindObjectOfType<ImageNumber>();
    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        imageNumber.Number = 12;    // 012로 출력되기
    }

}
