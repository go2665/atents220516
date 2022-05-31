using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform[] bgSlots = null;
    public float scrollingSpeed = 2.5f;

    const float BG_WIDTH = 13.6f;

    private void Update()
    {
        foreach (Transform bgSlot in bgSlots)
        {
            bgSlot.Translate(-transform.right * scrollingSpeed * Time.deltaTime);
        }
    }
}
