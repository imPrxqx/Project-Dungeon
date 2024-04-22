 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Loot
{
    [HideInInspector]
    public bool canOpen = false;
    [HideInInspector]
    public bool open = false;

    public GameObject particleOpen;
    private void Start()
    {
        //Set xp,money for Loot from default 
        (xp, money) = GlobalFunctions.CalcLoot(tier, xp, money);
    }

    private void Update()
    {
        //If player is inside of radius chest and was not opened then add xp and money 
        if(canOpen == true && open == false && Input.GetKeyDown(OptionsScript.keys[9]))
        {
            AddXp(xp);
            AddMoney(money);
            GameObject particle = GameObject.Instantiate(particleOpen, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(particle, 3);
            open = true;
            AudioManager.instance.Play("chest");
        }
    }
    //Get base method from abstract class Loot 
    public override void AddXp(int xp)
    {
        base.AddXp(xp);
    }
    //Get base method from abstract class Loot 
    public override void AddMoney(int money)
    {
        base.AddMoney(money);  
    }
    //OnTriggerEnter is called when the gameobject is in collision trigger 
    private void OnTriggerEnter(Collider other)
    {
        //If it is the player then can open chest
        if(other.CompareTag("Player"))
        {
            canOpen = true;
        }        
    }
    //OnTriggerExit is called when is the gameobject is not in collision trigger
    private void OnTriggerExit(Collider other)
    {
        //If it is the player then it can not open chest
        if(other.CompareTag("Player"))
        {
            canOpen = false;
        }   
    }
}
