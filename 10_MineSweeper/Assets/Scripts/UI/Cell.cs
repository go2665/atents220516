using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    // 상수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// ID가 설정되지 않은 것을 알리기 위한 상수
    /// </summary>
    const int ID_NOT_SET = -1;

    // 변수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 이 셀의 ID.(위치를 표현하기도 할 예정)
    /// </summary>
    int id = ID_NOT_SET;

    /// <summary>
    /// 이 셀의 이미지
    /// </summary>
    Image cellImage;

    // 상태 확인용 플래그 --------------------------------------------------------------------------
    
    /// <summary>
    /// 이 셀이 열렸는지 표시(true면 열렸음)
    /// </summary>
    bool isOpen = false;

    /// <summary>
    /// 이 셀에 지뢰가 있는지 표시(true면 지뢰가 있다.)
    /// </summary>
    public bool isMine = false;

    /// <summary>
    /// 이 셀에 우클릭을 얼마나 했는지 표시
    /// </summary>
    CloseCellType cellState = CloseCellType.Normal;

    // 캐싱용 --------------------------------------------------------------------------------------
    CellImageManager cellImages;    // 이미지 받아오기 위한 매니저
    GameManager gameManager;    // 게임 매니저

    // 델리게이트 ----------------------------------------------------------------------------------
    
    /// <summary>
    /// 이 셀이 지뢰없이 열렸을 때 실행되는 델리게이트
    /// </summary>
    public Action<Cell> onSafeOpen;

    // 프로퍼티 ------------------------------------------------------------------------------------
    
    /// <summary>
    /// ID 설정 및 확인용 프로퍼티(설정은 한번만 가능)
    /// </summary>
    public int ID
    {
        get => id;
        set
        {
            if(id == ID_NOT_SET)    // 단 한번만 설정 가능하도록 구현
            {
                id = value;
            }
            else
            {
                Debug.LogWarning("ID는 한번만 설정 가능합니다.");
            }
        }
    }

    /// <summary>
    /// 지뢰가 있는지 여부를 확인하는 프로퍼티
    /// </summary>
    public bool HasMine => isMine;

    // 함수 ---------------------------------------------------------------------------------------

    private void Awake()
    {
        // 컴포넌트 찾기
        cellImage = GetComponent<Image>();
    }

    private void Start()
    {
        // 각종 초기화
        gameManager = GameManager.Inst;
        cellImages = gameManager.CellImage;

        CellReset();
        //Debug.Log("Cell start");
    }

    /// <summary>
    /// 지뢰 설정 함수
    /// </summary>
    public void SetMine() => isMine = true;

    /// <summary>
    /// 셀의 열린 이미지 설정
    /// </summary>
    /// <param name="type">보일 이미지 종류</param>
    public void SetOpenImage(int count)
    { 
        cellImage.sprite = cellImages[OpenCellType.Empty+count]; 
    }

    /// <summary>
    /// 셀을 여는 함수
    /// </summary>
    public void OpenCell()
    {
        if (!isOpen)        // 열리지 않은 셀만 열기
        {
            isOpen = true;  // 열렸다고 표시

            if( isMine )
            {
                // 지뢰가 있다.
                cellImage.sprite = cellImages[OpenCellType.MineExplosion];  // 지뢰가 터진 스프라이트로 변경
                gameManager.GameOver(); // 게임오버 처리
            }
            else
            {
                // 지뢰가 없다. => 주변 8칸 검사
                onSafeOpen?.Invoke(this);
            }
        }
    }

    /// <summary>
    /// 이 셀을 초기화 하는 함수
    /// </summary>
    public void CellReset()
    {
        cellState = CloseCellType.Normal;                           // 기본 상태 설정
        cellImage.sprite = cellImages[global::CloseCellType.Normal];    // 기본 스프라이트 설정

        isOpen = false; // 닫혔다고 표시
        isMine = false; // 지뢰가 없다고 표시
    }

    /// <summary>
    /// 마우스 클릭을 했을 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if( eventData.button == PointerEventData.InputButton.Left)          // 마우스 왼쪽 클릭을 했을 때
        {            
            if (cellState == CloseCellType.Normal)  // 셀 우클릭 상태가 노멀이면
            {
                OpenCell(); // 해당 셀을 열기
            }
        }
        else if ( eventData.button == PointerEventData.InputButton.Right )  // 마우스 오른쪽 클릭을 했을 때
        {
            if( !isOpen )   // 셀이 아직 안열린 상태일때만
            {
                switch (cellState)  // 셀 우클릭 상태에 따라 처리
                {
                    // Normal -> Flag -> Question -> Normal .... 순으로 계속 반복
                    case CloseCellType.Normal:
                        cellState = CloseCellType.Flag;                        
                        break;
                    case CloseCellType.Flag:
                        cellState = CloseCellType.Question;
                        break;
                    case CloseCellType.Question:
                        cellState = CloseCellType.Normal;
                        break;
                    default:
                        break;
                }

                cellImage.sprite = cellImages[cellState];   // 셀 우클릭 상태에 맞는 이미지 표현
            }
        }
    }
}
