using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    InputController input;

    public InputController Input { get => input; }    

    protected override void Awake()
    {
        base.Awake();

        input = GetComponent<InputController>();
    }

}
