using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : StateMachineBehaviour
{
    AgentBehavior moves;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moves = animator.GetComponent<AgentBehavior>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moves.Hide();
    }
}