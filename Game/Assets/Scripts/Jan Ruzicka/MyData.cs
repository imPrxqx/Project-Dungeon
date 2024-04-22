using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used to store all the information that is then written to the Json file
public class MyData
{
    public int money = 0;
    public int xp = 0;
    public int level = 1;
    public int potionLvl = 1;
    public int skillPoints = 0;

    public int equipedWeaponNum = 0;
    public bool[] unlocks = { false, false, false, false, false };

    public WeaponData[] weapons;
    public List<Item> characterItems;

    public int floor = 1;
    public int levelfloor = 1;
    public int version = 0;


    //Constructor
    public MyData(int money, int xp, int level, int potionLvl, int skillPoints, int equipedWeaponNum, bool[] unlocks, WeaponData[] weapons, List<Item> characterItems, int floor, int levelfloor, int version)
    {
        this.money = money;
        this.xp = xp;
        this.level = level;
        this.skillPoints = skillPoints;
        this.unlocks = unlocks;
        this.equipedWeaponNum = equipedWeaponNum;
        this.weapons = weapons;
        this.potionLvl = potionLvl;
        this.characterItems = characterItems;
        this.floor = floor;
        this.levelfloor = levelfloor;
        this.version = version;
    }

    public MyData()
    {
        this.money = 0;
        this.xp = 0;
        this.level = 1;
        this.potionLvl = 1;
        this.unlocks = new bool[] { false, false, false, false, false };
        this.equipedWeaponNum = 0;
        this.weapons = new WeaponData[] { new WeaponData(), new WeaponData(), new WeaponData() };
        this.characterItems = new List<Item>();
        this.floor = 0;
        this.levelfloor = 0;
        this.version = 0;
    }
}