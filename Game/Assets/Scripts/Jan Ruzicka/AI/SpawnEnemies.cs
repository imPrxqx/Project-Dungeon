using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public int enemyCount;

    Transform spawnPoints;

    //List of the enemies which can spawn
    public GameObject[] enemyList;

    //List of all spawpoints in parent gameobject
    public List<Transform> listSpawnPoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        //Get gameobject who has this script
        spawnPoints = this.gameObject.transform;

        //Get from parent  all the gameobjects with spawnpoints for spawning the enemies
        foreach (Transform spawnpoint in spawnPoints)
        {
            listSpawnPoints.Add(spawnpoint);
        }

        SpawnEnemy();
    }
    //Spawn with certain number of enemies in certain spawnpoints
    public void SpawnEnemy()
    {
        for(int i = 0; i < enemyCount; i++)
        {
            int randomSpawnPoint = GlobalFunctions.RandomValue(listSpawnPoints.Count());

            //Spawn the enemy from list and spawn in random point from listSpawnPoints
            Instantiate(enemyList[GlobalFunctions.RandomValue(enemyList.Count())], new Vector3(listSpawnPoints[randomSpawnPoint].transform.position.x, listSpawnPoints[randomSpawnPoint].transform.position.y, listSpawnPoints[randomSpawnPoint].transform.position.z), Quaternion.identity);
        }
    }
}
