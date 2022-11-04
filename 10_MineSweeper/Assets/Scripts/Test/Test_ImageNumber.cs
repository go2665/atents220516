using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ImageNumber : TestBase
{
    public ImageNumber imageNumber;

    [Range(-99,999)]
    public int TestValue = 0;

    private void Start()
    {
        imageNumber = FindObjectOfType<ImageNumber>();
    }

    protected override void OnTest1(InputAction.CallbackContext obj)
    {
        imageNumber.Number = 1;
    }

    protected override void OnTest2(InputAction.CallbackContext obj)
    {
        imageNumber.Number = 12;    // 012로 출력되기       
    }

    protected override void OnTest3(InputAction.CallbackContext obj)
    {
        imageNumber.Number = 123;
    }

    protected override void OnTest4(InputAction.CallbackContext obj)
    {
        imageNumber.Number = -100;
    }

    private void OnValidate()
    {
        if (imageNumber)
        {
            imageNumber.Number = TestValue;
        }
    }
}
