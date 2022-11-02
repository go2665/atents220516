using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


// Singleton 클래스는 제네릭 클래스이다.
// T 타입은 반드시 Component 클래스를 상속받은 타입이어야 한다.
public class NetworkSingleton<T> : NetworkBehaviour where T : Component
{
    private static T instance = null;
    public static T Inst
    {
        get
        {
            if (instance == null)
            {
                // 아직 싱글톤용 인스턴스가 만들어지지 않았다. 한번도 사용된 적이 없다.
                T obj = FindObjectOfType<T>();                  // 일단 같은 타입이 있는지 찾기
                if (obj != null)
                {
                    instance = obj;                             // 있으면 있는 것을 사용한다.
                }
                else
                {
                    GameObject gameObject = new();             // 없으면 새로 만든다.
                    gameObject.name = $"{typeof(T).Name}";
                    instance = gameObject.AddComponent<T>();
                }
            }
            return instance;    // instance는 무조건 null이 아닌 값이 리턴된다.
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)       
        {
            // 새롭게 만들어진 싱글톤
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;  // 씬이 로딩되면 OnSceneLoaded 함수를 실행시켜라.(SceneManager가 가지고 있는 델리게이트에 함수 추가)
        }
        else
        {
            // 이 타입으로 만들어진 싱글톤이 있다.
            if (instance != this)
            {
                // 이미 만들어진게 내가 아니다.
                Destroy(this.gameObject);   // 나를 삭제.
            }
        }
    }

    /// <summary>
    /// 씬이 로딩될 때 실행될 델리게이트에 등록한 함수
    /// </summary>
    /// <param name="scene">해당 씬 데이터</param>
    /// <param name="mode">씬 추가 모드</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    /// <summary>
    /// 각종 초기화용 함수. 상속받을 클래스에서 override해서 사용할 것.
    /// </summary>
    protected virtual void Initialize()
    {
    }
}