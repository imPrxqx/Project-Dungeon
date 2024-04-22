using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Start_Intro_Crystall : StateMachineBehaviour
{
    Enter enter;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get script from the gameobject
        enter = animator.GetComponent<Enter>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        //If the player is inside radius then start Intro state
        if (enter.entered == true)
        {
            animator.SetTrigger("Intro");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Find healthbar from the gameobject and enable it 
        foreach (Transform child in animator.gameObject.transform)
        {
            if (child.name == "GUI")
            {
                child.gameObject.SetActive(true);
            }
        }

    }
}
