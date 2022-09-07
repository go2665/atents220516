using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEffect : MonoBehaviour
{
    public float damagePerSecond;
    IHit hitTarget = null;

    private void Update()
    {
        if(hitTarget != null)
        {
            hitTarget.HP -= damagePerSecond * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if( !other.CompareTag("Player") )
        {
            hitTarget = other.gameObject.GetComponent<IHit>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            hitTarget = null;
        }
    }
}
