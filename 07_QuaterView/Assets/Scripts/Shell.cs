using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    public float initialSpeed = 5.0f;
    public GameObject explosionPrefab;

    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = transform.forward * initialSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = Instantiate(explosionPrefab, 
            collision.contacts[0].point,                            // 충돌지점
            Quaternion.LookRotation(collision.contacts[0].normal)); // 충돌지점의 노멀백터를 forward로 지정

        Destroy(this.gameObject);
    }
}
