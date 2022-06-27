using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    public float scrollSpeed = 5.0f;
    public float width = 7.2f;

    //const float backWidth = 7.15f;
    //const float groundWidth = 8.35f;

    Transform[] children;

    private void Awake()
    {
        children = new Transform[transform.childCount];
        for(int i=0;i<transform.childCount;i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {
        foreach(Transform t in children)
        {
            t.Translate(Vector2.left * scrollSpeed * Time.deltaTime, Space.World);
            if( t.position.x < transform.position.x - width)
            {
                t.Translate(Vector2.right * width * transform.childCount, Space.World);
            }
        }
    }
}
