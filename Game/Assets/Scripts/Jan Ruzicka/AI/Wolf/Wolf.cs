using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : EnemyAi
{
    PlayerManagerScript playerManagerScript;

    public float movSpeed;
    public float rotSpeed = 100f;
    public float attackRange = 2;
    
    public GameObject particleDamage;

    [HideInInspector]
    public bool isWandering = false;
    [HideInInspector]
    public bool isRotatingL = false;
    [HideInInspector]
    public bool isRotatingR = false;
    [HideInInspector]
    public bool isWalking = false;
    
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

    private void Update()
    {
        //If it is not wandering then stop substracting timer
        if(isWandering == false)
        {
            coolDownTimer -= Time.deltaTime;
        }          
    }
    //Get base method from abstract class EnemyAi
    public override void ApplyForce(Vector3 force)
    {
        base.ApplyForce(force);
    }
    //Get base method from abstract class EnemyAi
    public override void TakeDemage(float demage)
    {
        base.TakeDemage(demage);

        //If health is 0 or lower destroy gameobject
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    //Get base method from abstract class EnemyAi
    public override void LookAtPlayer()
    {
        base.LookAtPlayer();
    }
    //Attack the player 
    public void Attack()
    {
        LookAtPlayer();
        
        //Get distance in type float between the player and the gameobject
        distance = GlobalFunctions.GetPositionDistance(player, transform);

        rb.AddForce(distance.x * 2, 0, distance.z * 2, ForceMode.Impulse);
        //Start attack coroutine to check if the gameobject hit the player
        StartCoroutine(AttackHit(demage));
    }
    IEnumerator AttackHit(float demage)
    {
        yield return new WaitForSeconds(0.5f);

        //If it hit collission the player and it is the player then hit the player
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
            }
        }
    }
    //Patrol on the map
    public void Patrol()
    {
        StartCoroutine(Patroling());
    }
    //Walk on the map
    public void Walking()
    {
        transform.position += transform.forward * movSpeed * Time.deltaTime;
    }
    //Patroling on the map
    IEnumerator Patroling()
    {
        int rotTime = Random.Range(1, 3);
        int rotWait = Random.Range(1, 3);
        int rotate = Random.Range(1, 3);
        int walktime = Random.Range(1, 3);

        isWandering = true;
        isWalking = true;
        //Walk for some time
        yield return new WaitForSeconds(walktime);
        isWalking = false;
        
        yield return new WaitForSeconds(rotWait);
        //Rotate left or right
        if (rotate == 1)
        {
            isRotatingR = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingR = false;
        }
        if (rotate == 2)
        {
            isRotatingL = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingL = false;
        }

        isWandering = false;
    }

    //Follow the player
    public void Follow()
    {
        LookAtPlayer();
        Walking();
    }
    //Rotate right
    public void RotR()
    {
        transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
    }
    //Rotate left
    public void RotL()
    {
        transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
    }
}
