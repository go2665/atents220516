using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Barrier : MonoBehaviour
{
    public float skillCoolTime = 10.0f;     // 스킬 쿨타임.
    public float skillDuration = 3.0f;      // 스킬 효과 지속시간

    private GameObject skillEffect;         // 배리어 스킬 이팩트
    private float currentCoolTime = 0.0f;   // 현재 스킬 쿨타임. 스킬 효과가 끝나면 다시 skillCoolTime에서 시작 
    private float currentDuration = 0.0f;   // 배리어 스킬을 사용후 스킬이 유지된 시간.( skillDuration에 도달하면 스킬 끝 )

    private float CurrentDuration
    {
        get => currentDuration;
        set
        {
            currentDuration = value;
            onDurationTimeChange?.Invoke(skillDuration - currentDuration, skillDuration);
        }
    }

    private float CurrentCoolTime 
    { 
        get => currentCoolTime; 
        set
        {
            currentCoolTime = value;
            onCoolTimeChange?.Invoke(currentCoolTime, skillCoolTime);
        }
    }
    public bool IsSkillReady { get => (currentCoolTime <= 0.0f); }  // 스킬 사용 가능 여부 확인용 프로퍼티
    public bool IsSkillActivate { get => skillEffect.activeSelf; }  // 스킬 발동 여부 확인용 프로퍼티

    public Action<float, float> onCoolTimeChange;
    public Action<float, float> onDurationTimeChange;
    public Action<bool> onDurationMode;

    private void Awake()
    {
        skillEffect = transform.GetChild(6).gameObject; // 이팩트용 게임 오브젝트 찾기
    }

    private void Start()
    {
        CurrentCoolTime = skillCoolTime;     // 현재 쿨타임 초기화
        CurrentCoolTime = 0.0f;            
    }

    private void Update()
    {
        if (skillEffect.activeSelf)
        {
            // 스킬이 활성화 된 상태면

            CurrentDuration += Time.deltaTime;      // 지속 시간 증가
            if(CurrentDuration >= skillDuration)    // 최대 지속시간을 넘어셔면
            {
                skillEffect.SetActive(false);       // 스킬 비활성화
                ResetCoolTime();                    // 쿨타임 초기화
                Debug.Log("Skill - end");
            }
        }
        else
        {
            // 스킬이 비활성화 된 상태면

            CurrentCoolTime -= Time.deltaTime;      // 쿨타임 감소
        }
    }

    public void ResetCoolTime()
    {
        onDurationMode?.Invoke(false);
        CurrentCoolTime = skillCoolTime;    // 쿨타임 초기화 함수
    }

    /// <summary>
    /// 스킬 사용하는 함수
    /// </summary>
    public void UseSkill()
    {
        if (CurrentCoolTime <= 0.0f)    // 쿨타임이 다되어야만 사용 가능
        {
            Debug.Log("Skill - Start");
            skillEffect.SetActive(true);    // 스킬 활성화
            CurrentDuration = 0.0f;         // 지속 시간 초기화
            onDurationMode?.Invoke(true);
        }
    }
}
