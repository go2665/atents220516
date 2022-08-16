using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime : MonoBehaviour
{
    Material mainMat;
    private void Start()
    {
        Renderer renderer = GetComponent<SpriteRenderer>();
        mainMat = renderer.material;
        mainMat.SetColor("_Color", Color.red * 5);
        mainMat.SetFloat("_Tickness", 0);
    }

    public void OutlineOnOff(bool on)
    {
        if( on )
            mainMat.SetFloat("_Tickness", 0.005f);
        else
            mainMat.SetFloat("_Tickness", 0.0f);
    }

    //private void Update()
    //{
    //    if( Keyboard.current.digit1Key.wasPressedThisFrame)
    //    {
    //        OutlineOnOff(true);
    //    }
    //    if (Keyboard.current.digit2Key.wasPressedThisFrame)
    //    {
    //        OutlineOnOff(false);
    //    }
    //}
}
