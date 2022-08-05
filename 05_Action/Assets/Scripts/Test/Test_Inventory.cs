using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inventory : MonoBehaviour
{
    private void Start()
    {
        //Test_AddRemoveMove();
        //Test_Stack();
        //Test_DummyInventory();

        //ItemFactory.MakeItem(ItemIDCode.Coin_Gold, new(1, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.Coin_Copper, new(2, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.Coin_Silver, new(3, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.Bone, new(4, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.Egg, new(5, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.HealingPotion, new(6, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.HealingPotion, new(6, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.ManaPotion, new(7, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.ManaPotion, new(7, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.ManaPotion, new(7, 0, 0));
        ItemFactory.MakeItem(ItemIDCode.OneHandSword1);
        ItemFactory.MakeItem(ItemIDCode.OneHandSword2);
        ItemFactory.MakeItem(ItemIDCode.OneHandSword1);
        ItemFactory.MakeItem(ItemIDCode.OneHandSword2);
        ItemFactory.MakeItem(ItemIDCode.HealingPotion);
        ItemFactory.MakeItem(ItemIDCode.HealingPotion);
        ItemFactory.MakeItem(ItemIDCode.HealingPotion);
        ItemFactory.MakeItem(ItemIDCode.ManaPotion);
        ItemFactory.MakeItem(ItemIDCode.ManaPotion);
        ItemFactory.MakeItem(ItemIDCode.ManaPotion);
        ItemFactory.MakeItem(ItemIDCode.ManaPotion);
        ItemFactory.MakeItem(ItemIDCode.ManaPotion);



        GameManager.Inst.MainPlayer.HP = 50;
        GameManager.Inst.MainPlayer.MP = 50;

    }

    private static void Test_DummyInventory()
    {
        Inventory inven = new Inventory();
        InventoryUI invenUI = FindObjectOfType<InventoryUI>();
        invenUI.InitializeInventory(inven);
        invenUI.InventoryOnOffSwitch();
        inven.AddItem(ItemIDCode.HealingPotion);
        inven.AddItem(ItemIDCode.HealingPotion);
        inven.AddItem(ItemIDCode.HealingPotion);
        inven.AddItem(ItemIDCode.ManaPotion);
        inven.AddItem(ItemIDCode.ManaPotion);
        inven.AddItem(ItemIDCode.ManaPotion);
        inven.AddItem(ItemIDCode.ManaPotion);
        inven.AddItem(ItemIDCode.ManaPotion);
        inven.AddItem(ItemIDCode.ManaPotion, 5);
        inven.AddItem(ItemIDCode.ManaPotion, 5);
        inven.AddItem(ItemIDCode.ManaPotion, 5);
        //inven.AddItem(ItemIDCode.ManaPotion, 4);
        //inven.AddItem(ItemIDCode.ManaPotion, 4);
        //inven.AddItem(ItemIDCode.ManaPotion, 4);
        //inven.TempRemoveItem(1, 3);
        //inven.TempToSlot(5);
        //ItemFactory.MakeItem(ItemIDCode.Egg, new(1, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.Bone, new(2, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.HealingPotion, new(3, 0, 0));
        //ItemFactory.MakeItem(ItemIDCode.ManaPotion, new(4, 0, 0));

        ItemFactory.MakeItem(ItemIDCode.Coin_Gold);

        //Camera.main.ScreenPointToRay(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
    }

    private static void Test_Stack()
    {
        Inventory inven = new Inventory();
        inven.AddItem(ItemIDCode.Egg);
        inven.AddItem(ItemIDCode.Bone);

        InventoryUI invenUI = FindObjectOfType<InventoryUI>();
        invenUI.InitializeInventory(inven);
        invenUI.InventoryOnOffSwitch();

        //inven.RemoveItem(0);
        //inven.AddItem(ItemIDCode.Egg);
        //inven.AddItem(ItemIDCode.Egg);

        inven.AddItem(ItemIDCode.HealingPotion);
        inven.AddItem(ItemIDCode.HealingPotion);
        inven.AddItem(ItemIDCode.HealingPotion);
        inven.AddItem(ItemIDCode.HealingPotion);
        inven.PrintInventory(); //[달걀(1),뼈다귀(1),힐링포션(3),힐링포션(1),(빈칸),(빈칸)]
        inven.AddItem(ItemIDCode.ManaPotion);
        inven.AddItem(ItemIDCode.ManaPotion, 5);
        inven.PrintInventory();
        inven.AddItem(ItemIDCode.ManaPotion, 5);
        inven.AddItem(ItemIDCode.ManaPotion, 5);
        inven.AddItem(ItemIDCode.ManaPotion, 5);
        inven.AddItem(ItemIDCode.ManaPotion, 5);
        inven.AddItem(ItemIDCode.ManaPotion, 5);
        inven.AddItem(ItemIDCode.ManaPotion);
        inven.PrintInventory(); //[달걀(1),뼈다귀(1),힐링포션(3),힐링포션(1),마나포션(2),마나포션(5)]
        inven.RemoveItem(5, 4);
        inven.RemoveItem(5);
        inven.ClearItem(4);
        inven.PrintInventory(); //[달걀(1),뼈다귀(1),힐링포션(3),힐링포션(1),(빈칸),(빈칸)]
        inven.AddItem(ItemIDCode.HealingPotion, 5);
        inven.AddItem(ItemIDCode.HealingPotion, 5);
    }



    // 1칸에 1개 테스트
    private static void Test_AddRemoveMove()
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

        inven.PrintInventory(); //[달걀,달걀,달걀,(빈칸),뼈다귀,뼈다귀]

        inven.AddItem(ItemIDCode.Egg, 3);
        inven.AddItem(ItemIDCode.Bone, 3);
        inven.PrintInventory(); //[달걀,달걀,달걀,달걀,뼈다귀,뼈다귀]

        inven.ClearInventory();
        inven.PrintInventory(); //[(빈칸),(빈칸),(빈칸),(빈칸),(빈칸),(빈칸)]

        inven.AddItem(ItemIDCode.Egg);
        inven.AddItem(ItemIDCode.Egg);
        inven.AddItem(ItemIDCode.Bone);
        inven.PrintInventory(); //[달걀,달걀,뼈다귀,(빈칸),(빈칸),(빈칸)]

        inven.MoveItem(1, 4);
        inven.PrintInventory(); //[달걀,(빈칸),뼈다귀,(빈칸),달걀,(빈칸)]
        inven.MoveItem(0, 1);   //[(빈칸),달걀,뼈다귀,(빈칸),달걀,(빈칸)]
        inven.MoveItem(4, 5);   //[(빈칸),달걀,뼈다귀,(빈칸),(빈칸),달걀]
        inven.MoveItem(5, 0);   //[달걀,달걀,뼈다귀,(빈칸),(빈칸),(빈칸)]
        inven.MoveItem(0, 5);   //[(빈칸),달걀,뼈다귀,(빈칸),(빈칸),달걀]
        inven.PrintInventory(); // - OK

        inven.MoveItem(2, 3);   //[(빈칸),달걀,(빈칸),뼈다귀,(빈칸),달걀]
        inven.MoveItem(3, 3);   //[(빈칸),달걀,(빈칸),뼈다귀,(빈칸),달걀]
        inven.MoveItem(1, 3);   //[(빈칸),뼈다귀,(빈칸),달걀,(빈칸),달걀]
        inven.MoveItem(1, 1);   //[(빈칸),뼈다귀,(빈칸),달걀,(빈칸),달걀]
        inven.PrintInventory(); // - OK

        inven.MoveItem(0, 1);   // 실패 - OK
        inven.MoveItem(2, 1);   // 실패 - OK
        inven.MoveItem(7, 0);   // 실패 - OK
        inven.MoveItem(1, 10);  // 실패 - OK


        //inven.MoveItem(1, 3);
        //inven.PrintInventory(); //실패 [달걀,(빈칸),뼈다귀,(빈칸),달걀,(빈칸)]

        // 테스트할 장소 : 시작부분, 끝나는부분, 중간부분 (시작과 끝에서 보통 문제가 많이 발생)
    }
}
