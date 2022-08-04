using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMP_Bar : MonoBehaviour
{
    IMana target;
    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        target = GameManager.Inst.MainPlayer.GetComponent<IMana>();
        target.onManaChange += SetMP_Value;
        slider.value = target.MP / target.MaxMP;
    }

    private void SetMP_Value()
    {
        if (target != null)
        {
            float ratio = target.MP / target.MaxMP;
            slider.value = ratio;
        }
    }
}
