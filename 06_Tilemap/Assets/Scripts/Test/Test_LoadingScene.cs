using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class Test_LoadingScene : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public string nextSceneName = "Test_Seamless_Base";

    AsyncOperation async;

    WaitForSeconds waitSecond;
    IEnumerator loadingTextCoroutine;
    IEnumerator loadSceneCoroutine;

    float loadRatio = 0.0f;
    bool loadCompleted = false;

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Press.performed += MousePress;
    }

    private void OnDisable()
    {
        inputActions.UI.Press.performed -= MousePress;
        inputActions.UI.Disable();
    }

    private void MousePress(InputAction.CallbackContext _)
    {
        if(loadCompleted)
        {
            async.allowSceneActivation = true;
        }
    }

    private void Start()
    {
        waitSecond = new WaitForSeconds(0.2f);
        loadingTextCoroutine = LoadingTextProgress();
        StartCoroutine(loadingTextCoroutine);
        loadSceneCoroutine = LoadScene();
        StartCoroutine(loadSceneCoroutine);
    }

    IEnumerator LoadingTextProgress()
    {
        int point = 0;
        while(true)
        {
            string text = "Loading";
            for(int i=0; i<point; i++)
            {
                text += " .";
            }
            loadingText.text = text;

            yield return waitSecond;
            point++;
            point %= 6;
        }
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;

        while(loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f;

            yield return null;
        }

        loadCompleted = true;
        Debug.Log("Load Complete!");
    }
}
