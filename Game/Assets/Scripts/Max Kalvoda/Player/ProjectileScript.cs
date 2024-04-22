using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    //particles
    public GameObject demageParticle; 
    public GameObject hitParticle;
    public GameObject explosionParticle;


    public int demage = 25;

    //projectile is destroyed only on contact with enemy (not ground, walls, trees...)
    public bool EnemyTriggerOnly = false;

    public bool AreaDmg;
    public float areaSize;

    public double liveTime = 8;
    private double startTime;

    // Start is called before the first frame update
    private void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        //destroys the projectile if it didnt hit anything for too long
        if(Time.time - liveTime > startTime)
            GameObject.Destroy(gameObject);
    }

    //OnTriggerEnter is called when some collider enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            //check if projectile hits an enemy
            if (other.CompareTag("Enemy"))
            {
                //if projectile has area-demage, demage all enemies in area
                if (AreaDmg)
                {
                    Vector3 dir = new Vector3(0, 1, 0);
                    RaycastHit[] hits = Physics.SphereCastAll(transform.position, areaSize, dir);
                    foreach (RaycastHit hit in hits)
                    {
                        GameObject hitObj = hit.collider.gameObject;
                        if (hitObj.CompareTag("Enemy"))
                        {
                            EnemyAi enemy = hitObj.GetComponent<EnemyAi>();
                            enemy.TakeDemage(demage);
                        }
                    }

                    //spawn explosion particle
                    GameObject.Instantiate(explosionParticle);
                }
                //otherwise demade just one enemy
                else
                {
                    EnemyAi enemy = other.GetComponent<EnemyAi>();
                    enemy.TakeDemage(demage);

                    //spawn demage particle
                    GameObject.Instantiate(demageParticle);
                }
            }
            //if projectile hits anything other than player or enemy
            else
            {
                // if projectile is destroyed only on contact with enemy, then do nothing and return
                if (EnemyTriggerOnly)
                    return;

                //if projectile has area-demage, demage all enemies in area
                if (AreaDmg)
                {
                    Vector3 dir = new Vector3(0, 1, 0);
                    RaycastHit[] hits = Physics.SphereCastAll(transform.position, areaSize, dir);
                    foreach (RaycastHit hit in hits)
                    {
                        GameObject hitObj = hit.collider.gameObject;
                        if (hitObj.CompareTag("Enemy"))
                        {
                            EnemyAi enemy = hitObj.GetComponent<EnemyAi>();
                            enemy.TakeDemage(demage);
                        }
                    }

                    //spawn explosion particle
                    GameObject.Instantiate(explosionParticle);
                }
                //otherwise just spawn hitParticle
                else
                {
                    GameObject.Instantiate(hitParticle);
                }
            }
            GameObject.Destroy(gameObject);
        }
    }
}
