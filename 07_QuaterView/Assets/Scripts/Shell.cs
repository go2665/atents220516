using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    public ShellData data;

    protected Rigidbody rigid;
    
    Material material;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        material.SetColor("_EffectColor", data.shellColor);
    }

    protected virtual void Start()
    {
        rigid.velocity = transform.forward * data.initialSpeed;  // 시작하면 앞쪽 방향으로 initialSpeed만큼의 속도로 나간다.
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // 충돌하면 폭팔 이팩트 생성
        data.Explosion(collision.contacts[0].point, collision.contacts[0].normal);

        // 맞은 대상이 HP가 깎일 수 있는 대상이면 HP를 감소시킨다.
        IHit hitTarget = collision.gameObject.GetComponent<IHit>();
        if (hitTarget != null)
        {
            hitTarget.TakeDamage(data.damage);
        }

        Destroy(this.gameObject);   // 포탄 삭제
    }
}
