using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    /// <summary>
    /// 상태별 버튼 스프라이트
    /// </summary>
    public Sprite[] buttonSprites;
    
    /// <summary>
    /// 버튼의 현재 상태
    /// </summary>
    public enum ButtonState
    {
        Normal = 0,
        Surprise,
        GameClear,
        GameOver
    }

    /// <summary>
    /// 버튼의 현재 상태를 나타낼 변수
    /// </summary>
    ButtonState state = ButtonState.Normal;

    // 필요 컴포넌트
    Image image;
    Button button;

    /// <summary>
    /// 상태용 프로퍼티. 읽기 전용
    /// </summary>
    public ButtonState State
    {
        get => state;
        private set 
        {
            if( state != value )    // 값에 변경이 있을 때만
            {
                state = value;      // 값을 변경하고
                image.sprite = buttonSprites[(int)state];   // 변경된 상태에 맞게 스프라이트 ㅕㄴ경
                if (state == ButtonState.Normal)
                {
                    // Normal 상태면 SpriteSwap으로 트랜지션 표시
                    button.transition = Selectable.Transition.SpriteSwap;                    
                }
                else
                {
                    // Normal 상태가 아니면 ColorTint로 트랜지션 표시
                    button.transition = Selectable.Transition.ColorTint;                    
                }
            }
        }
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameClear += () => State = ButtonState.GameClear; // 게임 상태에 따라 표시할 이미지 변경
        gameManager.onGameOver += () => State = ButtonState.GameOver;   

        button.onClick.AddListener(() =>
        {
            State = ButtonState.Normal;
            gameManager.GameReset(); 
        });        // 버튼 클릭했을 때 게임 리셋 수행
    }

    /// <summary>
    /// 이미지를 놀란 이미지로 변경
    /// </summary>
    public void SetSurprise()
    {
        State = ButtonState.Surprise;
    }

    /// <summary>
    /// 이미지를 일반 이미지로 변경
    /// </summary>
    public void SetNormal()
    {
        State = ButtonState.Normal;
    }
}
