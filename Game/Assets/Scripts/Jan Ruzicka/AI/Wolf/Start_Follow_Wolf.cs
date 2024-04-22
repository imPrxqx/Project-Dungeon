using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Follow_Wolf : StateMachineBehaviour
{
    Wolf wolf;
   
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get script from the gameobject
        wolf = animator.GetComponent<Wolf>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If the gameobject is close to the player and is not on cool down then start Attack state
        if (GlobalFunctions.DistanceRange(wolf.player, wolf.transform) <= 2.5f)
        {
            animator.SetTrigger("Attack");
        } else
        {
            wolf.Follow(); //Call Follow on Wolf.cs
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Reset trigger
        animator.ResetTrigger("Attack");
    }


}
