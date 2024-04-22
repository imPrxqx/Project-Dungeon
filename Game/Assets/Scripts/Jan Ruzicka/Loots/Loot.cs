using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    //Base variables for inhenrited class
    public int xp = 20;
    public int money = 10;
    public int tier;

    //Base method, which add xp for inhenrited class
    public virtual void AddXp(int xp)
    {
        GameData.AddXp(xp);
    }
    //Base method, which add money for inhenrited class
    public virtual void AddMoney(int money)
    {
        GameData.money += money;
    }
}
