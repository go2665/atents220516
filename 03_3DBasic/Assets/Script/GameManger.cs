using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManger : MonoBehaviour
{
    // 디자인패턴 : (~하게 코딩하니 좋더라. 편하더라.)

    // 싱글톤 : 하나만 만드는 것 -> 클래스의 인스턴스를 하나만 만드는 것
    //  1. 생성되는 타이밍을 조절 가능하다.
    //  2. 객체지향에 적합하다.
    //  3. 힙에 저장된다.
    //  4. 아무데서나 접근이 가능하다.

    // static : 정적 -> 정해져 있는 -> 메모리 주소가 정해져 있는(런타임 전에 모두 결정되어 있음)
    //  1. 스택에 저장됨.
    //  2. 전역 함수애 가까움.(일반적인 클래스와는 좀 거리가있다 -> 객체지향과 약간 맞지 않다)

    // 싱글톤 vs Static
    //  1. 싱글톤은 상속이 된다.(static은 상속이 안됨)

    float timer = 0.0f;         // 누적해서 보여 줄 시간
    TextMeshProUGUI timerText;  // 시간을 글자로 표현할 UI
    ClearTime clearTime;        // 클리어 시간을 표시할 UI
    int oldSceneIndex = 0;      // 이전에 불려진 씬의 인덱스

    // 싱글톤 관련 코드 ----------------------------------------------------------------------------
    static GameManger instance = null;
    public static GameManger Inst { get => instance; }

    private void Awake()    // 게임 오브젝트가 만들어진 직후
    {
        if( instance == null )
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);     // 씬이 변경되더라도 게임오브젝트가 사라지지 않게 해주는 함수
        }
        else
        {
            // 씬에 GameManger가 여러번 생성됬다.
            if( instance != this )
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Initialize()
    {
        SceneManager.sceneLoaded += OnStageStart;   // 씬의 로딩이 끝났을 때 실행될 델리게이트에 OnStageStart 등록
    }
    //---------------------------------------------------------------------------------------------

    
    /// <summary>
    /// 씬 로딩이 끝나면 실행될 함수
    /// </summary>
    /// <param name="arg0">씬 정보</param>
    /// <param name="arg1">씬 로딩 방식</param>
    private void OnStageStart(Scene arg0, LoadSceneMode arg1)
    {
        // UI찾기
        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        clearTime = FindObjectOfType<ClearTime>();
        clearTime.gameObject.SetActive(false);  // ClearTime은 끝날 때만 보이면 되니까 시작할 때는 꺼놓기

        if (oldSceneIndex != arg0.buildIndex)   // 이전에 열린 씬의 인덱스와 지금 열린 씬의 인덱스를 비교(재시작인지 다음 스테이지 시작인지 확인)
        {
            oldSceneIndex = arg0.buildIndex;    // 옛날 씬 인덱스를 지금 씬 인덱스로 변경
            ResetTimer();                       // 다음 스테이지 시작이면 타이머 리셋
        }
    }

    private void Update()
    {
        // 시간 누적 시키면서 UI 갱신
        timer += Time.deltaTime;
        timerText.text = $"{timer:f2}"; // 가비지가 무지막지하게 생성된다. => 메모리 가용량이 줄 수도 있다.

        // C# 문자열 타입의 특징 
        //  1. 수정 불가능한 타입이다. -> 항상 새로 만들어진다.
    }

    /// <summary>
    /// 시간 리셋
    /// </summary>
    void ResetTimer()
    {
        timer = 0.0f;
        timerText.text = $"{timer:f2}";
    }

    /// <summary>
    /// ClearTime UI에 시간 지정하고 보여주기
    /// </summary>
    public void ShowClearTime()
    {
        clearTime.SetTime(timer);
    }
    
}
