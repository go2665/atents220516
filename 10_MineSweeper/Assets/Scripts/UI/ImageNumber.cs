using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public Sprite[] imageNumbers;

    int number;

    Image[] numberImage;

    public int Number
    {
        get => number;
        set
        {
            if( number != value )
            {
                number = value;
                RefreshNumberImage();
            }
        }
    }

    private void Awake()
    {
        numberImage = GetComponentsInChildren<Image>();
    }

    void RefreshNumberImage()
    {

    }
}
