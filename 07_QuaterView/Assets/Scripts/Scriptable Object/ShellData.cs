using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shell Data", menuName = "Sctipable Object/Shell Data", order = 0)]
public class ShellData : ScriptableObject
{
    [ColorUsage(true, true)]
    public Color effectColor;

    public float initialSpeed = 20.0f;  // 생성되면 즉시 적용될 속도
    public float coolTime = 1.0f;
    public float damage = 50.0f;

    public GameObject explosionPrefab;  // 폭팔 이팩트 프리팝

    public virtual void Explosion(Vector3 position, Vector3 normal)
    {
        Instantiate(explosionPrefab, position,  // 생성 위치는 충돌지점
            Quaternion.LookRotation(normal));   // 생성될 때의 회전은 충돌지점의 노멀백터를 forward로 지정하는 회전
    }

    public virtual void TakeDamage(IHit target)
    {
        if (target != null)
        {
            target.HP -= damage;
        }
    }
}
