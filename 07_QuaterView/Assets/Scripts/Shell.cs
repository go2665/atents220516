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
        Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
        Time.timeScale = 0.1f;
        Destroy(this.gameObject);
    }
}
