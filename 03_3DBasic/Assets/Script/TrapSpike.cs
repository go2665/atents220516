using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    Animator anim = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            anim.SetTrigger("Activate");
            IDead dead = other.GetComponent<IDead>();
            dead?.Die();
        }
    }
}
