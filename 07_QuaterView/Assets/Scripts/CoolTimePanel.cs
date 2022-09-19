using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoolTimePanel : MonoBehaviour
{
    CoolTimeSlot[] coolTimeSlots;
    public CoolTimeSlot this[int index]
    {
        get => coolTimeSlots[index];
    }

    public int slotLength { get => coolTimeSlots.Length; }

    private void Awake()
    {
        coolTimeSlots = GetComponentsInChildren<CoolTimeSlot>();
    }
}
