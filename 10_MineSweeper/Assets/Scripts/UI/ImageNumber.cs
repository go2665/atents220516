using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    /// <summary>
    /// 이미지로 된 숫자 0~9
    /// </summary>
    public Sprite[] numberSprites;

    /// <summary>
    /// 실제로 표현할 숫자
    /// </summary>
    int number;

    /// <summary>
    /// 숫자를 이미지로 표현할 Image 컴포넌트들
    /// </summary>
    Image[] numberImages;

    /// <summary>
    /// 표현할 숫자를 결정할 프로퍼티
    /// </summary>
    public int Number
    {
        get => number;
        set
        {
            if( number != value )
            {                
                number = Mathf.Clamp(value, -99, 999);  // 최소값은 -99, 최대값은 999
                RefreshNumberImage();       // 숫자에 맞춰서 화면 갱신
            }
        }
    }

    Sprite MinusSprite => numberSprites[10];

    private void Awake()
    {
        // 숫자를 그릴 컴포넌트 가져오기
        numberImages = GetComponentsInChildren<Image>();
    }

    /// <summary>
    /// 숫자를 그리는 이미지 컴포넌트 갱신
    /// </summary>
    void RefreshNumberImage()
    {
        bool isMinus = (number < 0);        // 숫자가 -인지 아닌지 확인
        int tempNum = Mathf.Abs(number);    // tempNum에는 부호를 제거한 number 넣기

        List<int> digits = new List<int>(3);    // number에서 각 자리수 숫자를 분리해 보관할 리스트 생성

        // number 자리수별로 분리해서 리스트에 저장
        while (tempNum > 0)
        {
            digits.Add(tempNum % 10);
            tempNum /= 10;  // 나누다보면 0이 된다.
        }

        // 자리수별로 스프라이트 설정
        int index = 0;
        while ( digits.Count > 0 )
        {
            numberImages[index].sprite = numberSprites[digits[0]];  // digits가 가지는 첫번째 값에 알맞은 스트라이트를 선택해서 설정
            digits.RemoveAt(0); // 표시한 digits 제거
            index++;            // 다음 자리수 선택
        }

        // 남은 부분은 0으로 표시
        for(int i=index;i<numberImages.Length;i++)
        {
            numberImages[i].sprite = numberSprites[0];
        }

        // -값이였으면 맨 앞에 -표시 추가
        if( isMinus )
        {
            numberImages[numberImages.Length - 1].sprite = MinusSprite;
        }
    }
}
