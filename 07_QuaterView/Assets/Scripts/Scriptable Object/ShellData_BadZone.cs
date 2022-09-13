using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shell Data(BadZone)", menuName = "Sctipable Object/Shell Data(BadZone)", order = 1)]
public class ShellData_BadZone : ShellData
{
    public float effectDuration = 5.0f;
    public GameObject badEffectPrefab;

    public override void Explosion(Vector3 position, Vector3 normal)
    {
        // 터지는 프리팹 생성
        base.Explosion(position, normal);

        // 안좋은 이팩트 생성
        GameObject obj = Instantiate(badEffectPrefab, position, Quaternion.LookRotation(normal));

        // 않좋은 이팩트 기간 설정
        ParticleSystem[] pss = obj.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in pss)
        {
            var main = ps.main;
            main.duration = effectDuration;
        }
        pss[0].Play();

        // 않좋은 이팩트가 주는 데미지 설정
        BadEffect bedEffect = obj.GetComponent<BadEffect>();
        bedEffect.damagePerSecond = damage / effectDuration;    // 전체 데미지 / 전체시간
    }
}
