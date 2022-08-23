using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_Seamless : MonoBehaviour
{
    private const int Height = 3;
    private const int Width = 3;

    AsyncOperation[,] asyncs;
    string[,] scenenNames;
    bool[,] sceneLoaded;    // true면 로딩 되었음. false면 되지 않음

    void Start()
    {
        string sceneNameBase = "Test_Seamless";
        asyncs = new AsyncOperation[Height, Width];
        scenenNames = new string[Height, Width];

        asyncs[1, 1] = SceneManager.LoadSceneAsync($"{sceneNameBase}_1_1", LoadSceneMode.Additive);
        asyncs[1, 1].completed += (AsyncOperation _) => Debug.Log($"{sceneNameBase}_1_1");
        asyncs[1, 1].priority = 5;
        for (int y = 0; y< Height; y++)
        {
            for(int x = 0; x< Width; x++)
            {
                // 동기(Synchronous) 방식 로딩. 로딩중 다른 작업은 할 수 없음.
                //SceneManager.LoadScene($"{sceneNameBase}_{x}_{y}", LoadSceneMode.Additive);
                string temp = $"{sceneNameBase}_{x}_{y}";
                scenenNames[y, x] = temp;

                if (y == 1 && x == 1)
                {
                    continue;
                }

                // 비동기방식 로딩.
                asyncs[y,x] = SceneManager.LoadSceneAsync(temp, LoadSceneMode.Additive);
                //asyncs[y, x].completed    //씬 로딩이 완료되면 실행될 델리게이트

                // 델리게이트에 전달되는 변수는 힙으로 옮겨진다.
                int tempX = x;
                int tempY = y;
                asyncs[y, x].completed += (AsyncOperation _) => Debug.Log($"{sceneNameBase}_{tempX}_{tempY}");
                asyncs[y, x].completed += (_) => sceneLoaded[tempY, tempX] = true;

                // allowSceneActivation = false를 중첩으로 하는 것은 절대로 하면 안됨
                //asyncs[y, x].allowSceneActivation = false;    
            }
        }

        //asyncs[0, 0].allowSceneActivation = false;
    }

    private void Update()
    {
        if( Keyboard.current.digit1Key.wasPressedThisFrame )
        {
            //asyncs[0, 0].allowSceneActivation = true;
            if (sceneLoaded[0, 0])
            {
                AsyncOperation async = SceneManager.UnloadSceneAsync(scenenNames[0, 0]);
                async.completed += (_) => sceneLoaded[0, 0] = false;
            }
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SceneManager.UnloadSceneAsync(scenenNames[0, 1]);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SceneManager.UnloadSceneAsync(scenenNames[0, 2]);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            SceneManager.UnloadSceneAsync(scenenNames[1, 0]);
        }

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            SceneManager.UnloadSceneAsync(scenenNames[1, 1]);
        }

        if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            SceneManager.UnloadSceneAsync(scenenNames[1, 2]);
        }

        if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            SceneManager.UnloadSceneAsync(scenenNames[2, 0]);
        }

        if (Keyboard.current.digit8Key.wasPressedThisFrame)
        {
            SceneManager.UnloadSceneAsync(scenenNames[2, 1]);
        }

        if (Keyboard.current.digit9Key.wasPressedThisFrame)
        {
            SceneManager.UnloadSceneAsync(scenenNames[2, 2]);
        }
    }
}
