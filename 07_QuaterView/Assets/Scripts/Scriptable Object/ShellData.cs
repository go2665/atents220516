using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shell Data", menuName = "Sctipable Object/Shell Data", order = 0)]
public class ShellData : ScriptableObject
{
    [ColorUsage(true, true)]
    public Color shellColor;            // 포탄에 입힐 색상

    public float initialSpeed = 20.0f;  // 생성되면 즉시 적용될 속도
    public float coolTime = 1.0f;       // 이 포탄 종류의 쿨타임
    public float damage = 50.0f;        // 포탄 1발의 데미지

    public GameObject explosionPrefab;  // 폭팔 이팩트 프리팹

    /// <summary>
    /// 터지는 이팩트 처리용 함수
    /// </summary>
    /// <param name="position">이팩트가 생성될 위치</param>
    /// <param name="normal">터진 면의 normal 벡터</param>
    public virtual void Explosion(Vector3 position, Vector3 normal)
    {
        // 이팩트 생성
        Instantiate(explosionPrefab, 
            position,                           // 생성 위치는 충돌지점
            Quaternion.LookRotation(normal));   // 생성될 때의 회전은 충돌지점의 노멀백터를 forward로 지정하는 회전
    }

    /// <summary>
    /// 데미지 입히기
    /// </summary>
    /// <param name="target">데미지를 입을 대상</param>
    public virtual void TakeDamage(IHit target)
    {
        if (target != null)
        {
            target.HP -= damage;    // HP 감소. 사망처리 등은 프로퍼티 내부에서 처리
        }
    }
}
