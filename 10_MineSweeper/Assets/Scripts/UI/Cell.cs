using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Cell : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
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
    bool hasMine = false;

    /// <summary>
    /// 이 셀에 우클릭을 얼마나 했는지 표시
    /// </summary>
    CloseCellType cellState = CloseCellType.Normal;

    // 캐싱용 --------------------------------------------------------------------------------------
    CellImageManager cellImages;    // 이미지 받아오기 위한 매니저
    GameManager gameManager;        // 게임 매니저

    // 델리게이트 ----------------------------------------------------------------------------------
    
    /// <summary>
    /// 이 셀이 지뢰없이 열렸을 때 실행되는 델리게이트
    /// </summary>
    public Action<Cell> onSafeOpen;

    /// <summary>
    /// 이 셀로 인해 깃발 갯수의 변경이 있으면 실행되는 델리게이트
    /// </summary>
    public Action<int> onFlagCountChange;
    
    
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
    public bool HasMine => hasMine;

    /// <summary>
    /// 깃발이 설치되었는지 확인하는 프로퍼티
    /// </summary>
    public bool IsFlaged => (cellState == CloseCellType.Flag);

    /// <summary>
    /// 열린 셀인지 확인하는 프로퍼티
    /// </summary>
    public bool IsOpen => isOpen;

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
    public void SetMine() => hasMine = true;

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
        if (!isOpen && cellState == CloseCellType.Normal)        // 열리지 않았고 기본 상태인 셀만 열기
        {
            isOpen = true;  // 열렸다고 표시

            if( hasMine )
            {
                // 지뢰가 있다.
                cellImage.sprite = cellImages[OpenCellType.MineExplosion];  // 지뢰가 터진 스프라이트로 변경
                gameManager.GameOver(); // 게임오버 처리
            }
            else
            {
                // 지뢰가 없다. 
                if(cellState == CloseCellType.Flag) // 깃발이 있는 셀이 열리면
                {
                    onFlagCountChange?.Invoke(1);   // 깃발 갯수 회복
                }

                onSafeOpen?.Invoke(this);   // 주변 8칸 검사
            }
        }
    }

    /// <summary>
    /// 게임 종료시 셀에 표시할 이미지 설정용 함수
    /// </summary>
    /// <param name="type">설정할 이미지 종류</param>
    public void SetOpenCellImage(OpenCellType type)
    {
        cellImage.sprite = cellImages[type];
    }

    /// <summary>
    /// 이 셀을 초기화 하는 함수
    /// </summary>
    public void CellReset()
    {
        cellState = CloseCellType.Normal;                               // 기본 상태 설정
        cellImage.sprite = cellImages[global::CloseCellType.Normal];    // 기본 스프라이트 설정

        isOpen = false; // 닫혔다고 표시
        hasMine = false; // 지뢰가 없다고 표시
    }

    /// <summary>
    /// 마우스 클릭을 했을 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if ( eventData.button == PointerEventData.InputButton.Right )  // 마우스 오른쪽 클릭을 했을 때
        {
            if( !isOpen )   // 셀이 아직 안열린 상태일때만
            {
                switch (cellState)  // 셀 우클릭 상태에 따라 처리
                {
                    // Normal -> Flag -> Question -> Normal .... 순으로 계속 반복
                    case CloseCellType.Normal:
                        cellState = CloseCellType.Flag;                        
                        onFlagCountChange?.Invoke(-1);      // 깃발 설치했으니 깃발 갯수 감소
                        break;
                    case CloseCellType.Flag:
                        cellState = CloseCellType.Question;
                        onFlagCountChange?.Invoke(1);       // 깃발 설치가 취소되었으니 깃발 갯수 회복
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

    /// <summary>
    /// 마우스 버튼을 눌렀을 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)          // 마우스 왼쪽 클릭을 했을 때
        {
            CellPress();    // 누르는 표시
        }
    }
        
    /// <summary>
    /// 마우스 버튼을 땠을 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)          // 마우스 왼쪽 클릭을 했을 때
        {
            Cell cell = eventData.pointerCurrentRaycast.gameObject.GetComponent<Cell>();
            if (cell != null)
            {
                cell.OpenCell(); // 해당 위치의 셀을 열기
            }
        }
    }
        
    /// <summary>
    /// 셀에 마우스 커서가 들어갔을 때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        CellPress();    // 조건이 맞으면 눌린 표시
    }

    /// <summary>
    /// 셀에서 마우스 커서가 나갔을 때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        CellRelease(); // 조건이 맞으면 원상 복구
    }

    /// <summary>
    /// 셀을 누른 효과를 내고 싶을 때 사용하는 함수
    /// </summary>
    void CellPress()
    {
        // 셀이 닫혀있고, 아무런 표시가 되지 않은 셀이고, 마우스 왼쪽 버튼이 눌러져 있을 때 실행
        if (!IsOpen && cellState == CloseCellType.Normal && Mouse.current.leftButton.isPressed)
        {
            cellImage.sprite = cellImages[OpenCellType.Empty];  // 눌린 이미지로 변경
        }
    }

    /// <summary>
    /// 셀을 누른 효과를 복구 시키고 싶을 때 사용하는 함수
    /// </summary>
    void CellRelease()
    {
        // 셀이 닫혀 있고, 아무런 표시가 되어 있지 않다고 표시된 셀일 때 실행
        if (!IsOpen && cellState == CloseCellType.Normal)
        {
            cellImage.sprite = cellImages[CloseCellType.Normal];    // 원래 이미지로 복구
        }
    }
}
