using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpPower = 10.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rigid = collision.gameObject.GetComponent<Rigidbody>();
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}
