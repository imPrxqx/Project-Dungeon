using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used to store all information about the weapon they are currently holding
public class WeaponData
{
    public int level;
    public int xp;

    public readonly int[] upgradeCosts = { 100, 300 };
    public readonly float[] demage = { 30, 50, 100 };

    //0 = chain finisher, 1 = special, 2 = special finisher
    public bool[][] unlocks = { new bool[] { false, false }, new bool[] { false } };
    public readonly int[][] unlockCosts = { new int[] { 50, 100 }, new int[] { 120 } };

    public WeaponData(int level, int xp, bool[][] unlocks)
    {
        this.xp = xp;
        this.level = level;
        this.unlocks = unlocks;
    }

    public WeaponData()
    {
        this.xp = 0;
        this.level = 0;
        this.unlocks = new bool[][] { new bool[] { false, false }, new bool[] { false } };
    }
}

