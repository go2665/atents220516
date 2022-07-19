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

        inven.PrintInventory(); //[달걀,달걀,달걀,,뼈다귀,뼈다귀]

        inven.AddItem(ItemIDCode.Egg, 3);
        inven.AddItem(ItemIDCode.Bone, 3);
        inven.PrintInventory();

        inven.ClearInventory();
        inven.PrintInventory();

    }
}
