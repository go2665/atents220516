using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inventory : MonoBehaviour
{
    private void Start()
    {
        //Test_AddRemoveMove();

        Inventory inven = new Inventory();
        inven.AddItem(ItemIDCode.Egg);
        inven.AddItem(ItemIDCode.Bone);
        InventoryUI invenUI = FindObjectOfType<InventoryUI>();
        invenUI.InitializeInventory(inven);

        int i = 0;
    }

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
