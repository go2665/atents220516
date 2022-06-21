using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpPower = 10.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))   // 플레이이면 
        {
            Rigidbody rigid = collision.gameObject.GetComponent<Rigidbody>();   // 리지드바디 가져와서
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);          // 위쪽으로 급격하게 힘을 가해라.
        }
    }
}
