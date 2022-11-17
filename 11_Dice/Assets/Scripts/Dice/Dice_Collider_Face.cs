using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Collider_Face : MonoBehaviour
{
    public int faceNum;
    public Action<int> onFaceTouch;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Board"))
        {
            onFaceTouch?.Invoke(7-faceNum);
        }
    }
}
