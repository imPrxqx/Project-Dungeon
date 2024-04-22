using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : Loot, ILootBreakable
{
    public GameObject particleBreak;
   

     
    void Start()
    {
        //Set xp,money for Loot from default 
        (xp, money) = GlobalFunctions.CalcLoot(tier, xp, money);
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
    //Implementated interface from ILootBreakable for breaking loots
    public void BreakLoot()
    {
        AddXp(xp);
        AddMoney(money);

        //Spawn particle on destroyed the gameobject
        GameObject particle = GameObject.Instantiate(particleBreak, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        Destroy(particle, 3);
        AudioManager.instance.Play("vaseBreak");

        Destroy(gameObject);
    }
}
