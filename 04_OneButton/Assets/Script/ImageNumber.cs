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
            number = Mathf.Clamp(number, 0, 9999);      // 4자리까지만 표현 가능하니까 최대값은 9999로 설정

            int tempNum = number;
            for( int i=0;i<digitImage.Length; i++)      // 각 자리수별로 처리
            {
                if (tempNum > 0)
                {
                    int rest = tempNum % 10;                    // rest를 구해서 해당 자리수를 구함
                    digitImage[i].sprite = numberImages[rest];  // 해당 자리수에 맞는 이미지 설정
                    digitImage[i].color = Color.white;          // 보이게 만들기
                    tempNum /= 10;      // tempNum = tempNum / 10;  // 10분의 1로 줄이기(자리수 한칸 줄이기)
                }
                else
                {
                    // 더 이상 나누어도 의미가 없음
                    digitImage[i].color = Color.clear;  // 투명하게 보이도록 처리
                }
            }
        }
    }
}
