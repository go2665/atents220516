using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayTestObj : MonoBehaviour, IWaypointUser
{
    void Update()
    {
        transform.Translate(transform.forward * 2.0f * Time.deltaTime, Space.World);        
    }

    public void SetNextWayPoint(Transform newTarget)
    {
        transform.LookAt(newTarget);
    }
}
