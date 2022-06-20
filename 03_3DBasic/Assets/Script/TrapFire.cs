using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : MonoBehaviour
{
    ParticleSystem particle = null;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            particle.Play();
            IDead dead = other.GetComponent<IDead>();
            dead?.Die();
        }
    }
}
