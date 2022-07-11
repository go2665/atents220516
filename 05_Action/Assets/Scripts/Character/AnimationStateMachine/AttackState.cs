using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        GameManager.Inst.MainPlayer.TurnOnAura(true);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        GameManager.Inst.MainPlayer.TurnOnAura(false);
        animator.ResetTrigger("Attack");
    }
}
