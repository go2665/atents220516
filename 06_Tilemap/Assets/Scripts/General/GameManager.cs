using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    // 심리스용 -----------------------------------------------------------------------------------------------------
    MapManager mapManager;      // 심리스용으로 서브씬들 로드/언로드 처리
    public MapManager MapManager => mapManager;
    //--------------------------------------------------------------------------------------------------------------

    Player player;
    public Player Player => player;

    Volume postProcessVolume;
    public Volume PostProcessVolume => postProcessVolume;

    const int LoadingScene_Index = 0;
    const int Seamless_Base_Index = 1;


    static GameManager instance = null;
    public static GameManager Inst => instance;

    private void Awake()
    {
        if( instance == null )
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;            
        }
        else
        {
            if( instance != this )
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // Seamless_Base가 로딩 되었을 때만 초기화 함수 실행
        if (arg0.buildIndex == Seamless_Base_Index)
        {
            Initialize();
        }
    }   

    /// <summary>
    /// 초기화 함수라 무조건 한번만 실행될 것으로 가정하고 만든 함수
    /// </summary>
    public void Initialize()
    {
        postProcessVolume = FindObjectOfType<Volume>();
        player = FindObjectOfType<Player>();    // mapManager의 초기화보다 앞에 있어야 한다.

        if (mapManager == null)     // 없으면 찾는다.
        {
            mapManager = GetComponent<MapManager>();
        }
        mapManager.Initialize();
    }
}
