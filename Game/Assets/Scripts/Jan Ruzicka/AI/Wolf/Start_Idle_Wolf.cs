using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Idle_Wolf : StateMachineBehaviour
{
    Enter enter;
    Wolf wolf;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get scripts from the gameobject
        enter = animator.GetComponent<Enter>();
        wolf = animator.GetComponent<Wolf>();

        //Set timer between min and max time
        wolf.coolDownTimer = GlobalFunctions.WaitTime(wolf.minTime, wolf.maxTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If timer is 0 or lower then start Patrol state
        if (wolf.coolDownTimer <= 0)
        {
            animator.SetTrigger("Intro_Idle");
        }
        //If the player enterered inside radius of the gameobject start Intro state
        if(enter.entered == true)
        {
            animator.SetTrigger("Intro");
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Reset trigger
        animator.ResetTrigger("Intro_Idle");
    }

}
