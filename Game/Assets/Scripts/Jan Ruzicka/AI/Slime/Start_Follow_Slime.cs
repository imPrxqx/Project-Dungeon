using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Follow_Slime : StateMachineBehaviour
{

    Enter enter;
    Slime slime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get scripts from the gameobject
        enter = animator.GetComponent<Enter>();
        slime = animator.GetComponent<Slime>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If the player is inside radius then start Follow state
        if (enter.entered == true)
        {
            animator.SetTrigger("Follow");
        }

        //If is not on cool downn then call Patrol on Slime.cs
        if(slime.coolDownTimer <= 0)
        {
            slime.Patrol();
        }
     
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Reset trigger
        animator.ResetTrigger("Follow");
    }
}
