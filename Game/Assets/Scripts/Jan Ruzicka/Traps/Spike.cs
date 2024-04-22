using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Trap
{

    private void Start()
    {
        //Set damage for Trap from default 
        (demage) = GlobalFunctions.CalcTrap(tier, demage);
    }
    // Update is called once per frame
    void Update()
    {
        //If timer is >= 0 then stop substract timer
        if(coolDownTimer >= 0)
        {
            coolDownTimer -= Time.deltaTime;
        }     
    }

    //OnTriggerStay is called if the gameobject is inside to spike collission trigger
    private void OnTriggerStay(Collider other)
    {
        //If it is player and timer <=0 then hit player and set new timer from GlobalFunction
        if (other.CompareTag("Player") && coolDownTimer <= 0)
        {
            TakeDemage(demage);
            AudioManager.instance.Play("trap");
            coolDownTimer = GlobalFunctions.WaitTime(minTime, maxTime);
        }
    }
    //Get base method from abstract class Trap 
    public override void TakeDemage(float demage)
    {
        base.TakeDemage(demage);
    }
}
