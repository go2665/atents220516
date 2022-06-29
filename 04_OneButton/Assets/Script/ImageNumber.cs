using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public Image[] digitImage;      // 자리별로 사용할 Image UI, 0번째가 1자리, 1번째가 10자리, 2번째가 100자리, 3번째가 1000자리
    public Sprite[] numberImages = new Sprite[10];  // 숫자 이미지

    int number = 0;
    public int Number
    {
        get => number;
        set
        {
            number = value;
            number = Mathf.Clamp(number, 0, 9999);

            int tempNum = number;
            for( int i=0;i<digitImage.Length; i++)
            {
                int rest = tempNum % 10;
                digitImage[i].sprite = numberImages[rest];
                tempNum /= 10;      // tempNum = tempNum / 10;
            }
        }
    }
}
