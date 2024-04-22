using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Slime : EnemyAi
{

    PlayerManagerScript playerManagerScript;

    public float attackRange = 2;

    public GameObject particleDamage;
    public AudioSource jumpSfx;
    public AudioSource dieSfx;
    public AudioSource hitSfx;

    public Vector3 hitBox = new Vector3(1, 1, 1);
    Vector3 distance;

    [HideInInspector]
    public Transform player;
 
    // Start is called before the first frame update
    void Start()
    {
        //Get compoments 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();

        //Set calculated on based player level damage, heatlh, xp and money
        (health, demage, xp, money) = GlobalFunctions.CalcEnemy(tier, health, demage, xp, money);

    }

    // Update is called once per frame
    void Update()
    {    
        //Substract timer
        coolDownTimer -= Time.deltaTime;      
    }
    //Get base method from abstract class EnemyAi with if 
    public override void TakeDemage(float demage)
    {
        base.TakeDemage(demage);

        //If health is 0 or lower destroy the gameobject
        if(health <= 0)
        {
            dieSfx.Play();
            Destroy(gameObject);
        }
    }
    //Get base method from abstract class EnemyAi
    public override void ApplyForce(Vector3 force)
    {
        base.ApplyForce(force);
    }
    //Get base method from abstract class EnemyAi
    public override void LookAtPlayer()
    {
        base.LookAtPlayer();
       
        transform.Rotate(new Vector3(1, 0, 0), -90);
    }
    //Follow the player
    public void Follow()
    {
        LookAtPlayer();

        //Get distance in type float between the the player and the gameobject
        distance = GlobalFunctions.GetPositionDistance(player, transform);

        //Jump
        rb.AddForce(0, 5f, 0, ForceMode.Impulse);
        rb.AddForce(distance.x / 3, 0, distance.z / 3, ForceMode.Impulse);
        jumpSfx.Play();

        //Set timer between min and max time
        coolDownTimer = GlobalFunctions.WaitTime(minTime, maxTime);
    }
    //Patrol on the map
    public void Patrol()
    {     
        //Jump
        rb.AddForce(0, 5f, 0, ForceMode.Impulse);

        //Set way which will the gameobject jump
        int move = Random.Range(-5, 5);

        if (move == 0)
        {
            move += 1;
        }
        
        Vector3 look = new Vector3(transform.position.x + move * 2, transform.position.y, transform.position.z + move * 2);
        transform.LookAt(look);
        transform.Rotate(new Vector3(1, 0, 0), -90);
        rb.AddForce(move, 0, move, ForceMode.Impulse);
        jumpSfx.Play();

        //Set timer between min and max time
        coolDownTimer = GlobalFunctions.WaitTime(minTime, maxTime);
    }
    //Attack the player 
    public void Attack()
    {
        LookAtPlayer();

        //Get distance in type float between the player and the gameobject
        distance = GlobalFunctions.GetPositionDistance(player, transform);

        rb.AddForce(0, 3f, 0, ForceMode.Impulse);
        rb.AddForce(distance.x * 2, 0, distance.z * 2, ForceMode.Impulse);
        //Start attack coroutine to check if the gameobject hit the player
        StartCoroutine(AttackHit(demage));
    }
    //Attacking the player
    IEnumerator AttackHit(float demage)
    {
        //Wait 0,5 second
        yield return new WaitForSeconds(0.5f);

        //If it hit collission of the player and it is the player then hit the player
        Collider[] cols = Physics.OverlapBox(transform.position + transform.forward * hitBox.z, hitBox, transform.rotation);
        foreach (Collider col in cols)
        {
            if (col.CompareTag("Player"))
            {
                //Particle
                GameObject particle = Instantiate(particleDamage, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                Destroy(particle, 1);

                playerManagerScript = col.gameObject.GetComponent<PlayerManagerScript>();
                playerManagerScript.TakeHit(demage);
                hitSfx.Play();
            }
        }
        //Set timer between min and max time
        coolDownTimer = GlobalFunctions.WaitTime(minTime, maxTime);
    }

}
