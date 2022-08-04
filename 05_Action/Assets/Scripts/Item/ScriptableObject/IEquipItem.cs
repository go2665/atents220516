using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipItem
{
    void EquipItem(IEquipTarget target);
    void UnEquipItem(IEquipTarget target);
    void ToggleEquipItem(IEquipTarget target);
}
