using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{
    static int itemCount = 0;

    public static GameObject MakeItem(ItemIDCode code)
    {
        GameObject obj = new GameObject();
        Item item = obj.AddComponent<Item>();

        item.data = GameManager.Inst.ItemData[ItemIDCode.Coin_Copper];
        string[] itemName = item.data.name.Split("_");
        obj.name = $"{itemName[1]}_{itemCount}";
        itemCount++;

        return obj;
    }
}
