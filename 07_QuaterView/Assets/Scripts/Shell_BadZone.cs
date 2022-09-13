using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_BadZone : Shell
{
    protected override void OnCollisionEnter(Collision collision)
    {
        // 이팩트가 나올 바닥면 위치 구하기
        Ray ray = new Ray(collision.contacts[0].point, Vector3.down);
        Vector3 position;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")))
        {
            position = hit.point;
        }
        else
        {
            position = collision.contacts[0].point;
        }
        position += new Vector3(0, 0.1f, 0);

        // 바닥에서 이팩트 터트리기
        data.Explosion(position, Vector3.up);


        // 아래는 중복코드지만 넘어감.

        // 맞은 대상이 HP가 깎일 수 있는 대상이면 HP를 감소시킨다.
        IHit hitTarget = collision.gameObject.GetComponent<IHit>();
        data.TakeDamage(hitTarget);

        // 스스로 없어지기
        Destroy(this.gameObject);
    }
}
