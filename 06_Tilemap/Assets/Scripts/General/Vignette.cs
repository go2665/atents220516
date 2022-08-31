using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Vignette : MonoBehaviour
{
    private void Awake()
    {
        Volume volume =GetComponent<Volume>();
        if( volume.profile.TryGet<Bloom>(out Bloom vignette) )
        {

        }
    }
}
