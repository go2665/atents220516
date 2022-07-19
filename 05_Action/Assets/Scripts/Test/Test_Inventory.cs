using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inventory : MonoBehaviour
{
    private void Start()
    {
        Inventory inven = new();
        inven.AddItem(ItemIDCode.Egg);
        inven.AddItem(ItemIDCode.Egg);
        inven.AddItem(ItemIDCode.Egg);
        inven.AddItem(ItemIDCode.Bone);
        inven.AddItem(ItemIDCode.Bone);
        inven.AddItem(ItemIDCode.Bone);
        inven.AddItem(ItemIDCode.Bone);

        inven.RemoveItem(3);
        inven.RemoveItem(10);
    }
}
