using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : StateMachineBehaviour
{
    AgentBehavior moves;
    BlackBoard blackboard;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moves = animator.GetComponent<AgentBehavior>();
        blackboard = animator.GetComponent<BlackBoard>();
        moves.SetState(AgentBehavior.MovementMode.WANDER);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (Vector3.Distance(blackboard.cop.position, blackboard.treasure.transform.position) > blackboard.dist2Steal)
        //    animator.SetTrigger("away");
        //else
        moves.SetState(AgentBehavior.MovementMode.WANDER);
    }
}