using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManual : Door, IUseable
{
    public void Use()
    {
        Open();
        StartCoroutine(CloseDoor());
    }

    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(3.0f);
        Close();
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }

    protected override void OnTriggerExit(Collider other)
    {
    }
}
