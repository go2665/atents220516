using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    public Slider slider;                   // 로딩바
    public TextMeshProUGUI loadingText;     // 로딩 진행중 표시를 위한 텍스트
    public string nextSceneName = "Seamless_Base"; // 다음에 로딩할 씬의 이름

    AsyncOperation async;                   // 비동기씬로딩의 진행상황과 결과를 알 수 있는 클래스

    WaitForSeconds waitSecond;              // loadingText 점찍기 간격 유지 설정용
    IEnumerator loadingTextCoroutine;       // Loading ... 을 위한 코루틴
    IEnumerator loadSceneCoroutine;         // 비동기 씬 로드를 위한 코루틴

    float loadRatio = 0.0f;                 // 비동기 씬 로딩 진행 상황 + 0.1f;
    bool loadCompleted = false;             // 비동기 씬 로딩이 다 준비되었는지 표시(true면 준비완료)

    float sliderUpdateSpeed = 1.0f;         // 로딩바의 최소 시간을 유지하기 위한 값

    PlayerInputActions inputActions;        // 로딩 완료 후 넘어가기 위한 입력용

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();   // UI 액션맵만 활성화
        inputActions.UI.Press.performed += MousePress;  // 마우스 누르는 액션에 함수 등록
    }

    private void OnDisable()
    {
        inputActions.UI.Press.performed -= MousePress;  // 마우스 누르는 액션에 함수 해제
        inputActions.UI.Disable();  // UI 액션맵도 해제
    }

    private void MousePress(InputAction.CallbackContext _)
    {
        if(loadCompleted)   // 씬이 로드가 완료되었을 때만
        {
            async.allowSceneActivation = true;  // 씬 로딩 작업을 완료할 수 있도록 설정
        }
    }

    private void Start()
    {
        waitSecond = new WaitForSeconds(0.2f);  // Loading ... 시간간격 0.2초
        loadingTextCoroutine = LoadingTextProgress();   // Loading ... 용 코루틴 저장
        StartCoroutine(loadingTextCoroutine);           // 코루틴 실행
        loadSceneCoroutine = LoadScene();               // 비동기 씬 로딩을 위한 코루틴 저장
        StartCoroutine(loadSceneCoroutine);             // 코루틴 실행
    }

    private void Update()
    {
        //slider.value = Mathf.Lerp(slider.value, loadRatio, Time.deltaTime * sliderUpdateSpeed);

        // slider의 value가 아직 loadRatio보다 낮으면 빠르게 loadRatio까지 올리는 것이 목적
        if (slider.value < loadRatio)
        {
            // slider가 아직 loadRatio까지 도달하지 않았음.
            slider.value += Time.deltaTime * sliderUpdateSpeed; // slider 증가 시키기
        }
    }

    /// <summary>
    /// Loading ...을 찍기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadingTextProgress()
    {
        int point = 0;  // 0 ~ 5로 변경된 숫자
        while (true)
        {
            string text = "Loading";    // 가비지가 양산되는 문제는 있다.
            for(int i=0; i<point; i++)
            {
                text += " .";
            }
            loadingText.text = text;    // 반복해서 Loading 글자 뒤에 .을 추가로 붙인다.

            yield return waitSecond;    // 정해진 시간동한 대기
            point++;                    // point값 증가
            point %= 6;                 // point값이 0 ~ 5가 되도록 변경
        }
    }

    /// <summary>
    /// 비동기 씬 로딩용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(nextSceneName); // 비동기로 씬 로드 시도
        async.allowSceneActivation = false; // 준비가 완료되어도 바로 로딩하지 않도록 설정

        while(loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f;  // loadRatio를 비동기 진행 상황 + 0.1로 맞추기(현재 async.progress는 최대값이 0.9)

            yield return null;
        }

        loadCompleted = true;           // 로딩이 끝났다고 표시
        Debug.Log("Load Complete!");

        yield return new WaitForSeconds(1.0f);  // 1초 뒤에 점찍는 것도 멈추기
        StopCoroutine(loadingTextCoroutine);
        loadingText.text = "Loading Complete. \nPress Button.";
    }
}
