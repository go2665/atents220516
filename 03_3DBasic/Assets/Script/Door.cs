using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    protected Animator anim = null;

    public void Open()
    {
        anim.SetBool("IsOpen", true);
    }

    public void Close()
    {
        anim.SetBool("IsOpen", false);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            Open();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Close();
        }
    }    
}
