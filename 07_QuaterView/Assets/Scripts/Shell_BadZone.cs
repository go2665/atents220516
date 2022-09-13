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

        data.Explosion(position, Vector3.up);

        Destroy(this.gameObject);
    }
}
