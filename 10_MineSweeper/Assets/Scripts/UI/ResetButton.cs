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
    /// 상태용 프로퍼티
    /// </summary>
    public ButtonState State
    {
        get => state;
        set 
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
                    if( state == ButtonState.Surprise )
                    {
                        // 서프라이즈 상태면
                        StopAllCoroutines();
                        StartCoroutine(BackToNormal()); // 일정 시간 후에 다시 Normal로 이동
                    }
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
        button.onClick.AddListener(() => { GameManager.Inst.GameReset(); });        // 버튼 클릭했을 때 게임 리셋 수행
    }

    IEnumerator BackToNormal()
    {
        yield return new WaitForSeconds(0.3f);  // 0.3초 대기 후
        State = ButtonState.Normal;             // 다시 Normal로 돌리기
    }

}
