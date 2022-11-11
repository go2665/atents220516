using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Cell : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler
{
    // 상수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// ID가 설정되지 않은 것을 알리기 위한 상수
    /// </summary>
    const int ID_NOT_SET = -1;

    const int NOT_OPEN = -1;

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
    /// 주변의 지뢰 숫자. 안열렸으면 NOT_OPEN, 열렸는데 지뢰가 아니면 주변 지뢰 숫자
    /// </summary>
    int aroundMineCount = NOT_OPEN;
    
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

    /// <summary>
    /// 셀이 눌러지면 실행되는 델리게이트
    /// </summary>
    public Action<int> onCellPress;

    /// <summary>
    /// 눌렀던 셀을 때면 실행되는 델리게이트
    /// </summary>
    public Action<int> onCellRelease;

    /// <summary>
    /// 마우스 포인터가 셀에 들어갔을 실행되는 델리게이트
    /// </summary>
    public Action<int> onCellEnter;
    
    
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
    /// 열린 셀인지 확인하는 프로퍼티.
    /// </summary>
    public bool IsOpen => isOpen;

    /// <summary>
    /// 주변 지뢰 갯수를 확인하는 프로퍼티. NOT_OPEN이면 아직 안열려서 주변 지뢰갯수를 모른다.
    /// </summary>
    public int AroundMineCount => aroundMineCount;

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

        // 함수 등록
        onCellPress += (_) =>
        {
            if(gameManager.IsPlaying)
                gameManager.ResetBtn.SetSurprise();    // 플레이 중에만 놀란 표정으로 바뀌기
        };
        onCellRelease += (_) =>
        {
            if (gameManager.IsPlaying)
                gameManager.ResetBtn.SetNormal();       // 플레이 중에만 보통 얼굴로 돌아옴
        };

        CellReset();
        //Debug.Log("Cell start");
    }

    /// <summary>
    /// 지뢰 설정 함수
    /// </summary>
    public void SetMine() => hasMine = true;

    /// <summary>
    /// 셀이 열렸을 때 지뢰 숫자 이미지 설정용 함수
    /// </summary>
    /// <param name="type">보일 이미지 종류</param>
    public void SetOpenCellImage(int count)
    {
        aroundMineCount = count;
        cellImage.sprite = cellImages[OpenCellType.Empty+count]; 
    }

    /// <summary>
    /// 셀이 열렸는데 게임 종료시 셀에 표시할 이미지 설정용 함수
    /// </summary>
    /// <param name="type">설정할 이미지 종류</param>
    public void SetOpenCellImage(OpenCellType type)
    {
        isOpen = true;
        cellImage.sprite = cellImages[type];
    }

    /// <summary>
    /// 셀을 여는 함수
    /// </summary>
    public bool OpenCell()
    {
        bool result = false;
        if (!isOpen && cellState == CloseCellType.Normal)        // 열리지 않았고 기본 상태인 셀만 열기
        {
            isOpen = true;  // 열렸다고 표시

            if( hasMine )
            {
                // 지뢰가 있다.
                SetOpenCellImage(OpenCellType.MineExplosion);   // 지뢰가 터진 스프라이트로 변경
                gameManager.GameOver(); // 게임오버 처리
            }
            else
            {
                // 지뢰가 없다. 
                SetOpenCellImage(0);                // 주변 갯수와 상관없이 기본값 용도로 설정(주변에 지뢰가 있으면 이후코드에서 덮어씀)
                
                // 자동으로 열렸을 때 처리
                if(cellState == CloseCellType.Flag) // 깃발이 있는 셀이 열리면
                {
                    onFlagCountChange?.Invoke(1);   // 깃발 갯수 회복
                }

                result = true;
            }
        }
        return result;
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
        // 게임 플레이 상태이고 마우스 오른쪽 클릭을 했을 때
        if (gameManager.IsPlaying && eventData.button == PointerEventData.InputButton.Right)  
        {
            if (!isOpen)   // 셀이 아직 안열린 상태일때만
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
        // GameStart 매번 시도(처음에만 실행됨, 타이머가 처음 셀이 열릴 때 시작되어야 되기 때문에 필요) 
        gameManager.GameStart();

        // 게임 플레이 상태이고 마마우스 왼쪽 클릭을 했을 때
        if (gameManager.IsPlaying && eventData.button == PointerEventData.InputButton.Left)          
        {
            onCellPress?.Invoke(ID);    // 눌렸다고 알림
        }     
    }
        
    /// <summary>
    /// 마우스 버튼을 땠을 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        // 게임 플레이 상태이고 마우스 왼쪽 클릭을 했을 때
        if (gameManager.IsPlaying && eventData.button == PointerEventData.InputButton.Left)
        {
            Cell cell = eventData.pointerCurrentRaycast.gameObject.GetComponent<Cell>();
            if (cell != null)
            {
                onCellRelease?.Invoke(cell.ID);    // 버튼을 땠다고 알림            
            }
        }
    }
        
    /// <summary>
    /// 셀에 마우스 커서가 들어갔을 때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameManager.IsPlaying)          // 게임 플레이 상태이면
        {
            onCellEnter?.Invoke(ID);        // 마우스 포인터가 들어왔다고 알림
        }
    }

    /// <summary>
    /// 셀을 누른 효과를 내고 싶을 때 사용하는 함수
    /// </summary>
    public void CellPress()
    {
        // 셀이 닫혀있고 깃발이나 물음표가 없을 때만 처리
        if (!IsOpen && cellState == CloseCellType.Normal)
        {
            cellImage.sprite = cellImages[OpenCellType.Empty];  // 눌린 이미지로 변경
        }
    }

    /// <summary>
    /// 셀을 누른 효과를 복구 시키고 싶을 때 사용하는 함수
    /// </summary>
    public void CellRelease()
    {
        // 셀이 닫혀있고 깃발이나 물음표가 없을 때만 처리
        if (!IsOpen && cellState == CloseCellType.Normal)
        {
            cellImage.sprite = cellImages[CloseCellType.Normal];    // 원래 이미지로 복구
        }
    }
}
