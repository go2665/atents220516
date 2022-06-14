using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Vector3 v1 = new Vector3(1, 2, 3);
        Vector3 v2 = v1;
        v1.x = 10;
        Debug.Log(v2);
    }
}
