using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Phase : MonoBehaviour
{
    Material material;

    float split = 1.0f;
    float dir = -1.0f;

    float sinDelta = 0.0f;

    private void Awake()
    {
        Renderer temp = GetComponent<SpriteRenderer>();
        material = temp.material;
    }

    private void Start()
    {
        material.SetFloat("_Split", split);
        sinDelta = 0.0f;
    }

    private void Update()
    {
        //split += Time.deltaTime * dir;
        //if( split >1.0f || split < 0.0f)
        //{
        //    dir *= -1;
        //}
        //material.SetFloat("_Split", split);

        sinDelta += Time.deltaTime;
        material.SetFloat("_Split", (Mathf.Cos(sinDelta) + 1) * 0.5f); 
    }
}
