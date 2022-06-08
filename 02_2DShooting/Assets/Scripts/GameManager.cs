using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null; // static이 붙어서 주소가 고정이다 = 이 클래스의 모든 인스턴스가 이 변수를 같이 사용한다.
    private int score = 0;      // 점수 저장용
    public Text scoreText = null;


    // 프로퍼티(속성). 특이한 메서드(함수). 읽기 전용, 쓰기 전용 등으로 설정해서 객체지향적 특성을 유지할 수 있음.
    // 값을 쓸거나 읽을 때 실행되어야 할 기능들을 쉽게 추가할 수 있다.
    public static GameManager Inst 
    {
        //get
        //{
        //    return instance;
        //}
        get => instance;
    }

    public int Score
    {
        get => score;
        set
        {
            score = value;
            Debug.Log($"Score : {score}");
        }
    }

    private void Awake()
    {
        // 디자인 패턴 : ~식으로 코딩을 하니 좋더라 하는 것들
        // 싱글톤(Singleton) : 디자인 패턴 중 하나. 클래스의 인스턴스가 단 하나만 존재하도록 만드는 것
        if( instance == null )
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // 다른 씬이 로드되어도 게임 오브젝트 유지

            scoreText.text = "Test";
        }
        else
        {
            // 먼저 만들어진 GameManager가 있다.
            if(instance != this)    // 먼저 만들어진 것이 내가 아닐 때
            {
                Destroy(this.gameObject);   // 자기 자신이 제거된다.
            }
        }
    }
}
