using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float HP { get; set; }  // HP를 확인하고 설정할 수 있다.
    float MaxHP { get;  }   // 최대 HP를 확인할 수 있다.

    System.Action onHealthChange { get; set; }   // HP가 변경될 때 실행될 델리게이트
}
