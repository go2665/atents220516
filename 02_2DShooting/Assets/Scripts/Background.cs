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
        float minusX = transform.position.x - BG_WIDTH; // 백그라운드의 x위치에서 왼쪽으로 BG_WIDTH(그림 한장의 폭) 만큼 이동한 위치
        foreach (Transform bgSlot in bgSlots)
        {
            bgSlot.Translate(-transform.right * scrollingSpeed * Time.deltaTime);
            if (bgSlot.position.x < minusX)
            {
                // 충분히 왼쪽으로 이동한 위치
                Debug.Log("충분히 왼쪽이다.");
                bgSlot.Translate(transform.right * BG_WIDTH * 3.0f);    //오른쪽으로 BG_WIDTH의 3배 만큼 이동
            }
        }
    }
}
