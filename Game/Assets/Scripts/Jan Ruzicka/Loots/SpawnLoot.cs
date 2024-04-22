using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    Transform lootPoints;

    public int minChance;
    public int maxChance;

    //List of loots which can spawn
    public GameObject[] lootList;

    //List of all loot points in parent the gameobject
    public List<Transform> listLootPoints = new List<Transform>();

    void Start()
    {
        //Get the gameobject who has this script
        lootPoints = this.gameObject.transform;

        //Get from parent the gameobject all loot points for spawning loots
        foreach (Transform lootpoints in lootPoints)
        {
            listLootPoints.Add(lootpoints);
        }

        SpawnLoots();
    }
    //Spawn loots with certain chance in certain loot points
    public void SpawnLoots()
    {   
        for(int i = 0; i < listLootPoints.Count();i++)
        {
            //Get value true or true on based chance
            bool value = GlobalFunctions.GambleBool(minChance, maxChance);

            //If is true then spawn on certain loot from list of loots
            if(value == true)
            {
                Instantiate(lootList[GlobalFunctions.RandomValue(lootList.Count())], new Vector3(listLootPoints[i].transform.position.x, listLootPoints[i].transform.position.y + 1, listLootPoints[i].transform.position.z), Quaternion.Euler(GlobalFunctions.GetRotation(listLootPoints[i].transform)));
            }
        }
    }


}
