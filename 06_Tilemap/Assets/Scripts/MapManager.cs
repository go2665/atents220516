using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    const string SceneNameBase = "Seamless";

    private const int Height = 3;
    private const int Width = 3;

    string[,] sceneNames;
    bool[,] sceneLoaded;    // true면 로딩 되었음. false면 되지 않음

    public void Initialize()
    {
        sceneNames = new string[Height, Width];
        sceneLoaded = new bool[Height, Width];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                string temp = $"{SceneNameBase}_{x}_{y}";
                sceneNames[y, x] = temp;
                sceneLoaded[y, x] = false;
            }
        }
    }

    public void RequestAsyncSceneLoad(int x, int y)
    {
        if(!sceneLoaded[y, x])
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[y, x], LoadSceneMode.Additive);
            //async.completed
            sceneLoaded[y, x] = true;
        }
    }

    public void RequestAsyncSceneUnload(int x, int y)
    {
        if(sceneLoaded[y,x])
        {
            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[y, x]);
            //async.completed
            // sceneLoaded[y, x] 언로드 상태로 변경
        }
    }

}
