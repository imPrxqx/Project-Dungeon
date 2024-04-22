using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class Crystall : EnemyAi
{   
    //enum type with stages
    enum Stage
    {
        Stage_1,
        Stage_2,
        Stage_3
    }

    public int laserDemage;
    [HideInInspector]
    public int randomRange = 1;
    int randomSpawnPoint;

    public HealthBar healthBar;
    PlayerManagerScript playerManager;

    public Transform laserPosition;
    Transform spawnPoints;
    Transform player;
 
    //Particle, prefabs, list of enemies
    public GameObject particleLaser;
    public GameObject projectilPrefab;
    public GameObject shieldPrefab;
    public GameObject[] enemyList;
    public GameObject text;
    GameObject projectilBall;

    [HideInInspector]
    public float maxHealth;
    float rotatePosition;

    RaycastHit reycastLaser;
    Stage stage;

    //List of all spawnpoints
    public List<Transform> listSpawnPoints = new List<Transform>();

    void Start()
    {
        //Set calculated on based player level damage, heatlh, xp and money
        (health, demage, xp, money) = GlobalFunctions.CalcEnemy(tier, health, demage, xp, money);

        //Set max health value for the gameobject
        maxHealth = health;
        healthBar.SetMaxHealth(maxHealth);

        //Get compoments 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManagerScript>();
        spawnPoints = GameObject.FindGameObjectWithTag("Spawnpoints").transform;

        //Get all the gameobjects with tag Spawnpoints and add to list
        foreach (Transform spawnpoint in spawnPoints)
        {
            listSpawnPoints.Add(spawnpoint);
        }

        AudioManager.instance.Play("bassDrop");
    }
    //Every frame substract timer and set updated value health bar
    void FixedUpdate()
    {
        healthBar.SetHealth(health);
        coolDownTimer -= Time.deltaTime;
    }
    //Get base method from abstract class EnemyAi and overrided base method from EnemyAi with no ApplyForce 
    public override void ApplyForce(Vector3 force)
    {

    }
    //Get base method from abstract class EnemyAi 
    public override void TakeDemage(float demage)
    {
        base.TakeDemage(demage);
    }
    //Get base method from abstract class EnemyAi
    public override void LookAtPlayer()
    {
        base.LookAtPlayer();
    }
    //Check stage which is the gameobject
    public void CheckStage()
    {
        switch (stage)
        {
            case Stage.Stage_1:
                //If the gameobject has 70% then go next stage and spawn shield
                if (GlobalFunctions.GetBossHealth(health, maxHealth) <= 0.7f)
                {                
                    SpawnShield();
                    NextStage();
                }
                break;
            case Stage.Stage_2:
                //If the gameobject has 30% then go next stage and spawn shield
                if (GlobalFunctions.GetBossHealth(health, maxHealth) <= 0.3f)
                {
                    SpawnShield();
                    NextStage();
                }
                break;
        }
    }
    //Calls next stage from previous stage
    public void NextStage()
    {
        switch (stage)
        {
            case Stage.Stage_1:
                randomRange += 1;
                stage = Stage.Stage_2;
                break;
            case Stage.Stage_2:
                randomRange += 1;
                stage = Stage.Stage_3;
                break;
        }
    }
    //Calls die when the gameobject has 0 health
    public void Die()
    {
        GameObject[] listProjectil = GameObject.FindGameObjectsWithTag("Projectil");

        //Destroy all active projectils
        foreach (GameObject listprojectil in listProjectil)
        {
            Destroy(listprojectil);
        }
        //Destroy the gameboject
        Destroy(gameObject);
        AudioManager.instance.Play("crystalLogo");
        text.SetActive(true);
        GameObject.Destroy(text,5);

    }
    //Spawn shield for the gameobject
    public void SpawnShield()
    {
        shieldPrefab.SetActive(true);
    }
    //Spawn 1 enemy on certain spawn point
    public void SpawnEnemies()
    {
        //Get certain spawnpoints and spawn on it enemy
        randomSpawnPoint = Random.Range(0, listSpawnPoints.Count);
        Instantiate(enemyList[Random.Range(0, enemyList.Length)], new Vector3(listSpawnPoints[randomSpawnPoint].transform.position.x + Random.Range(-2, 2), listSpawnPoints[randomSpawnPoint].transform.position.y, listSpawnPoints[randomSpawnPoint].transform.position.z + Random.Range(-2, 2)), Quaternion.identity);
    }
    //Start laser
    public void LaserAttack()
    {
        StartCoroutine(StartLaserAttack());
    }
    //Laser is being prepared for attack
    IEnumerator StartLaserAttack()
    {
        //Aim the player 
        laserPosition.LookAt(player);

        GameObject particle = GameObject.Instantiate(particleLaser, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        Destroy(particle, 3);

        yield return new WaitForSecondsRealtime(0.2f);
        //If it is the player then hit player
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out reycastLaser, Mathf.Infinity, -1, QueryTriggerInteraction.Ignore))
        {
            if (reycastLaser.collider.CompareTag("Player"))
            {
                playerManager.TakeHit(laserDemage); 
            }        
        }
    }
    //Range attack
    public void RangeAttack()
    {
        StartCoroutine(SpawnRangeAttack());
    }
    //Spawn projectils with in pre-prepared trajectory
    IEnumerator SpawnRangeAttack()
    {
        for (int i = 0; i <= 5; i++)
        {
            rotatePosition = Random.Range(-50, 50);
            projectilBall = Instantiate(projectilPrefab, transform.position, Quaternion.identity);
            projectilBall.transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, rotatePosition));
            projectilBall.GetComponent<Destroying>().demage = demage;
            //Wait 1 second
            yield return new WaitForSeconds(1);
        }
    }
}