
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Follow_Crystall : StateMachineBehaviour
{
    Crystall crystall;

    int random; 
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get script from the gameobject
        crystall = animator.GetComponent<Crystall>();
        //Call CheckStage on Crystall.cs
        crystall.LookAtPlayer();  
        //Get attack which he will use
        random = Random.Range(1, crystall.randomRange + 1);
        //Set timer between min and max time
        crystall.coolDownTimer = GlobalFunctions.WaitTime(crystall.minTime, crystall.maxTime);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Call CheckStage on Crystall.cs
        crystall.CheckStage();

        //If timer is 0 or lower then start pre-prepared attack state
        if (crystall.coolDownTimer <= 0)
        {
            animator.SetTrigger(string.Format("Attack{0:D}", random));
        }     
        //If boss is 0 or lower start die state
        if(GlobalFunctions.GetBossHealth(crystall.health, crystall.maxHealth) <= 0)
        {
            animator.SetTrigger("Die");
        }
        //Call LookAtPlayer on Crystall.cs
        crystall.LookAtPlayer();

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Call LookAtPlayer on Crystall.cs
        crystall.LookAtPlayer();
        //Reset pre-prepared attack
        animator.ResetTrigger(string.Format("Attack{0:D}", random));
    }
  
}
