using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpanwer : MonoBehaviour
{
    public GameObject monster;
    public int maxSpawn = 2;

    private void Update()
    {
        if( transform.childCount < maxSpawn )
        {
            GameObject obj = Instantiate(monster, this.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }
    }
}
