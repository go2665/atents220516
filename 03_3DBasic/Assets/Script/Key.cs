using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public event Action onKeyPickup = null;

    private void OnTriggerEnter(Collider other)
    {
        onKeyPickup?.Invoke();
        Destroy(this.gameObject, 0.1f);
    }
}
