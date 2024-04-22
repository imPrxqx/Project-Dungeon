using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Patrol_Wolf : StateMachineBehaviour
{

    Enter enter;
    Wolf wolf;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get scripts from the gameobject
        enter = animator.GetComponent<Enter>();
        wolf = animator.GetComponent<Wolf>();

        //Call Patrol on Wolf.cs
        wolf.Patrol();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Random patroling on the map
        if (enter.entered == true)
        {
            animator.SetTrigger("Intro");       
        }
        if (wolf.isWandering == false)
        {
            animator.SetTrigger("Intro_Patrol");
        }
        if (wolf.isRotatingR == true)
        {
            wolf.RotR();//Call RotR on Wolf.cs
        }
        if (wolf.isRotatingL == true)
        {
            wolf.RotL();//Call RotL on Wolf.cs
        }
        if (wolf.isWalking == true)
        {
            wolf.Walking();//Call Walking on Wolf.cs
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Reset trigger
        animator.ResetTrigger("Intro_Patrol");
    }

}
