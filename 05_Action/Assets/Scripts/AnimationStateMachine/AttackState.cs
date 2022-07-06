using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.ResetTrigger("Attack");
    }
}
