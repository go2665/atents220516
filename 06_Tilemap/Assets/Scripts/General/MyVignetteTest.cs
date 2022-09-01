using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MyVignetteTest : MonoBehaviour
{
    [Range(0,1)]
    public float vignetteIntensity = 0.0f;
    Vignette vignette;

    private void Awake()
    {
        Volume volume = GetComponent<Volume>();
        if ( volume.profile.TryGet<Vignette>(out vignette) )
        {
            vignette.intensity.value = 1.0f;
        }
    }

    private void OnValidate()
    {
        if( vignette != null )
        {
            vignette.intensity.value = vignetteIntensity;
        }
    }

}
