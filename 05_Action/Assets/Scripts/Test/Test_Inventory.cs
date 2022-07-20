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

        // 테스트할 장소 : 시작부분, 끝나는부분, 중간부분 (시작과 끝에서 보통 문제가 많이 발생)
    }
}
