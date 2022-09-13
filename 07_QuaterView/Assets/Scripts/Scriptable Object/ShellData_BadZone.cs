using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shell Data(BadZone)", menuName = "Sctipable Object/Shell Data(BadZone)", order = 1)]
public class ShellData_BadZone : ShellData
{
    public float effectDuration = 5.0f; // 바닥 데미지를 주는 시간
    public GameObject badEffectPrefab;  // 바닥 데미지용 이팩트

    /// <summary>
    /// 터지는 이팩트 처리용 함수 + 바닥에 깔리는 이팩트 처리
    /// </summary>
    /// <param name="position">이팩트가 생성되는 위치</param>
    /// <param name="normal">터진 면의 normal 벡터</param>
    public override void Explosion(Vector3 position, Vector3 normal)
    {
        // 터지는 프리팹 생성
        base.Explosion(position, normal);

        // 바닥 데미지용 이팩트 생성
        GameObject obj = Instantiate(badEffectPrefab, position, Quaternion.LookRotation(normal));

        // 바닥 데미지용 이팩트 기간 설정
        ParticleSystem[] pss = obj.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in pss)
        {
            var main = ps.main; 
            main.duration = effectDuration; // 자식 파티클 시스템에도 시간 설정
        }
        pss[0].Play();  // 전체 재생

        // 바닥 데미지용 이팩트가 주는 초당 데미지 설정
        BadEffect bedEffect = obj.GetComponent<BadEffect>();
        bedEffect.damagePerSecond = damage / effectDuration;    // 전체 데미지 / 전체시간
    }
}
