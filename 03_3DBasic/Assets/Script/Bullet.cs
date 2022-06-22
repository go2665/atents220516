using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public float lifeTime = 3.0f;
    Rigidbody rigid = null;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = transform.forward * speed;
        Destroy(this.gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDead dead = collision.gameObject.GetComponent<IDead>();
        if (dead != null)
        {
            dead.Die();
        }
        Destroy(this.gameObject);
    }
}
