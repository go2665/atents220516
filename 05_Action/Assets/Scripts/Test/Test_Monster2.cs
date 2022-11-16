using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Monster2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var a = (1, 20);
        Debug.Log(a.GetType());

        GameManager.Inst.MainPlayer.Test();
        
    }

}
