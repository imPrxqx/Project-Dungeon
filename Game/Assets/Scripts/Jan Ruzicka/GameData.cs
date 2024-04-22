using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//This is a static class that holds all important data about the current state of the game. It can retrieve and store this current state in a json file.
public static class GameData
{
    public static int money = 0;
    public static int xp = 0;
    public static int level = 1;
    public static int potionLvl = 1;
    public static int skillPoints = 0;

    //0-sword, 1-spear, 2-daggers
    public static int equipedWeaponNum = 0;
    public static WeaponData[] weapons = new WeaponData[] { new WeaponData(), new WeaponData(), new WeaponData() };

    public static bool inMenu = false;

    //[double jump, dash, hyperjump, glide, groundpound]
    public static bool[] unlocks = { false, false, false, false, false };
    public static int[] unlockCosts = { 1, 1, 1, 1, 2};

    //List of accepted missions
    public static List<Mission> activeMissions = new List<Mission>();

    //Inventory
    public static List<Item> characterItems = new List<Item>();

    //Level
    public static int floor = 0;
    public static int levelfloor = 0;
    public static int version = 0;


    /*
    //Save position the player
    public static (float, float, float) GetPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return (0,0,0);

        Transform trans = player.transform;

        float x = trans.position.x;
        float y = trans.position.y;
        float z = trans.position.z;

        return (x, y, z);
    }
    */
    //It is served to unlock abilities and then write to the Json file
    public static bool Unlock(int index)
    {
        if (!unlocks[index])
        {
            if (skillPoints >= unlockCosts[index])
            {
                unlocks[index] = true;
                skillPoints -= unlockCosts[index];
                SaveGameData();
                return true;
            }
        }
        return false;
    }
    //It is served to unlock weapons and then write to the Json file
    public static bool UnlockWeapon(int index, int weaponIndex)
    {
        int teir = 0;
        while (index >= weapons[weaponIndex].unlocks[teir].Length)
        {
            index -= weapons[weaponIndex].unlocks[teir].Length;
            teir++;
        }

        if (!weapons[weaponIndex].unlocks[teir][index])
        {
            if (weapons[weaponIndex].xp >= weapons[weaponIndex].unlockCosts[teir][index])
            {
                weapons[weaponIndex].unlocks[teir][index] = true;
                weapons[weaponIndex].xp -= weapons[weaponIndex].unlockCosts[teir][index];
                SaveGameData();
                return true;
            }
        }
        return false;
    }
    //It is served to determine if the player has completed the mission
    public static void CheckMission(string type, string name)
    {
        switch (type)
        {
            case "hunt":
                foreach (Mission m in activeMissions)
                {
                    if (m.amount <= 0)
                    {
                        continue;
                    }

                    if (m.type == "hunt")
                    {
                        if(name == m.enemy)
                        {
                            m.amount--;
                            if(m.amount <= 0)
                            {
                                money += m.price;
                            }
                        }
                        if(m.enemy == "any")
                        {
                            m.amount--;
                            if (m.amount <= 0)
                            {
                                money += m.price;
                            }
                        }
                    }
                }
                break;
        }
    }
    //Delete all active missions
    public static void ResetMissions()
    {
        activeMissions.Clear();
    }
    //Add xp to the player
    public static void AddXp(int x)
    {//todo: mat func pro dosazeni dalsi urovne
        xp += x;
        weapons[equipedWeaponNum].xp += x;
        if (xp >= level * 100)
        {
            level++;
            skillPoints++;
            xp = 0;
        }
    }

    //Reads from the Json file the data that has been saved
    public static void LoadGameData()
    {
        MyData newData = null;
        if (File.Exists("GameData.json"))
        {
            StreamReader fileReader = new StreamReader(File.Open("GameData.json", FileMode.Open));
            string jsonStr = fileReader.ReadToEnd();
            fileReader.Close();

            newData = JsonConvert.DeserializeObject<MyData>(jsonStr);
        }
        else
        {
            newData = new MyData(); 
        }

        money = newData.money;
        xp = newData.xp;
        level = newData.level;
        potionLvl = newData.potionLvl;
        skillPoints = newData.skillPoints;
        equipedWeaponNum = newData.equipedWeaponNum;
        unlocks = newData.unlocks;
        weapons = newData.weapons;
        characterItems = newData.characterItems;
        floor = newData.floor;
        levelfloor = newData.levelfloor;
        version = newData.version;

        SaveGameData();

    }
    //Saves data to a Json file
    public static void SaveGameData()
    {
        string jsonStr = JsonConvert.SerializeObject(new MyData(money, xp, level, potionLvl, skillPoints, equipedWeaponNum, unlocks, weapons, characterItems, floor, levelfloor, version));

        StreamWriter writer = new StreamWriter(File.Open("GameData.json", FileMode.Create));

        writer.WriteLine(jsonStr);

        writer.Close();
    }
}
