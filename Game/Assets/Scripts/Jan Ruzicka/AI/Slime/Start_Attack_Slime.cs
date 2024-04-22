using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Attack_Slime : StateMachineBehaviour
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
        //If the player is inside radius then start Patrol state
        if (enter.entered == false)
        {
            animator.SetTrigger("Outside");
        }

        //Call LookAtPlayer on Slime.cs
        slime.LookAtPlayer();

        //If the gameobject is close to the player and is not on cool down then start Attack state
        if (GlobalFunctions.DistanceRange(slime.player, slime.transform) < slime.attackRange && slime.coolDownTimer <= 0)
        {
            animator.SetTrigger("Attack");
        } else if(slime.coolDownTimer <= 0) //If is not radius then call Follow on Slime.cs
        {
            slime.Follow();
        } else
        {
            animator.ResetTrigger("Attack");
        }            
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Reset triggers 
        animator.ResetTrigger("Outside");
        animator.ResetTrigger("Attack");
    }

}
