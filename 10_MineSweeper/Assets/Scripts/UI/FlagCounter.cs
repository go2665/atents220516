using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCounter : MonoBehaviour
{
    /// <summary>
    /// 현재 깃발 갯수
    /// </summary>
    int flagCount;

    /// <summary>
    /// 깃발 갯수를 보여줄 이미지 넘버
    /// </summary>
    ImageNumber imageNumber;

    /// <summary>
    /// 깃발 갯수용 프로퍼티. 갯수에 변경이 있으면 자동으로 이미지 넘버도 변경
    /// </summary>
    public int FlagCount
    {
        get => flagCount;
        set
        {
            if( flagCount != value )
            {
                flagCount = value;
                imageNumber.Number = flagCount; // 이미지 넘버에 적용
            }
        }
    }

    private void Awake()
    {
        // 컴포넌트 찾기
        imageNumber = GetComponent<ImageNumber>();
    }

    private void Start()
    {
        Stage stage = FindObjectOfType<Stage>();
        stage.onFlagCountChange += (x) => { FlagCount += x; };  // 스테이지에서 깃발 갯수 변경 신호가 들어오면 FlagCount에 적용
        
        FlagCount = stage.mineCount;                            // 최초 깃발 갯수 설정
    }
}
