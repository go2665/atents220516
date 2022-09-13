using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shell Data(Cluster)", menuName = "Sctipable Object/Shell Data(Cluster)", order = 2)]
public class ShellData_Cluster : ShellData
{
    public float upPower = 20.0f;   // 위로 올라가는 힘(-9.8보다는 커야 한다.)
    public float lifeTime = 1.0f;   // 스스로 터질때까지 걸리는 시간
}
