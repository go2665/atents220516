using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAura : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Inst.MainPlayer.TurnOnAura(false);
    }
}
