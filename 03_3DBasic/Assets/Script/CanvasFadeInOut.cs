using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFadeInOut : MonoBehaviour
{
    Animator anim = null;
    public System.Action OnFadeOutEnd;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        //SceneManager.sceneLoaded += OnSceneLoaded;        
    }

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    //씬에 있는 모든 오브젝트가 enable이 되면 실행
    //    StartFadeIn();
    //}

    private void Start()
    {
        StartFadeIn();
    }

    public void StartFadeOut()
    {
        // 페이드아웃 애니메이션 실행
        anim.SetTrigger("StageEnd");
    }

    public void StartFadeIn()
    {
        // 페이드 인 애니메이션 실행
        anim.SetTrigger("StageStart");
    }

    public void AnimEnd()
    {
        // 페이드 아웃이 종료될 때 델리게이트 실행
        OnFadeOutEnd?.Invoke();
    }   
}
