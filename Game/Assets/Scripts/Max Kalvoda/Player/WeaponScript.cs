using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    //weapon params
    public float normalDemage;
    public float specialDemage;
    public float knockback;

    //size of weapon hitbox
    public Vector3 normalHitBox;
    public float normalHitBoxOffset;
    public Vector3 specialHitBox;
    public float specialHitBoxOffset;

    //particle objects to instantiate
    public GameObject hitParticle;
    public GameObject demageParticle;

    [HideInInspector]
    public Animator weaponAnim;

    [HideInInspector]
    private GameObject cam;


    //Awake is called when the script instance is being loaded.
    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        weaponAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //check attack input 
        if (Input.GetKeyDown(OptionsScript.keys[7]) && !GameData.inMenu)
        {
            weaponAnim.SetTrigger("Attack");
        }

        //check special attack input 
        if (Input.GetKeyDown(OptionsScript.keys[8]) && !GameData.inMenu)
        {
            weaponAnim.SetTrigger("Special");
        }
    }

    //checks if some enemyes are overlaping with hitbox and demages them
    public void Attack(int special)
    {
        //set hitBox and demage variables
        Vector3 hitBox;
        float demage;
        float offset;
        if (special == 1)
        {
            hitBox = specialHitBox;
            demage = specialDemage;
            offset = specialHitBoxOffset;
        }
        else
        {
            hitBox = normalHitBox;
            demage = normalDemage;
            offset = normalHitBoxOffset;
        }

        //get all overlapping colliders
        Vector3 center = cam.transform.position + cam.transform.forward * offset;
        Collider[] cols = Physics.OverlapBox(center, hitBox, cam.transform.rotation);

        foreach (Collider col in cols)
        {
            //if its enemy, hit him
            if (col.CompareTag("Enemy") || col.CompareTag("Boss"))
            {
                if (!col.isTrigger)
                {
                    EnemyAi enemy = col.gameObject.GetComponent<EnemyAi>();
                    enemy.TakeDemage(demage);
                    Vector3 dir = (enemy.transform.position - cam.transform.position).normalized;
                    enemy.ApplyForce(dir * knockback);

                    Vector3 pos = cam.transform.position + offset * cam.transform.forward;
                    Vector3 eul = new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y - 90, cam.transform.eulerAngles.z);
                    Quaternion rot = Quaternion.Euler(eul);
                    GameObject particle = GameObject.Instantiate(demageParticle, pos, rot);
                    GameObject.Destroy(particle, 1);
                }
            }
            //else just spawn particles
            else if (!col.CompareTag("Player"))
            {
                Vector3 pos = cam.transform.position + offset * cam.transform.forward;
                Vector3 eul = new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y - 90, cam.transform.eulerAngles.z);
                Quaternion rot = Quaternion.Euler(eul);
                GameObject particle = GameObject.Instantiate(hitParticle, pos , rot);
                GameObject.Destroy(particle, 1);
            }
            //if its loot that is breakle, break it
            if(col.CompareTag("Lootbreak"))
            {
                ILootBreakable lootBreakable = col.gameObject.GetComponent<ILootBreakable>();
                lootBreakable.BreakLoot();
            }
            if(col.CompareTag("Breakable")) {
                IBreakable breakable = col.gameObject.GetComponent<IBreakable>();
                breakable.TakeDamage(demage);
            
            }
        }
        //playing sfx
        AudioManager.instance.Play("slash");
    }

    //adds demage for given amount of time
    public IEnumerator BoostWeapon(int duration, float demage)
    {
        this.normalDemage += demage;
        this.specialDemage += demage;
        yield return new WaitForSeconds(duration);
        this.normalDemage -= demage;
        this.specialDemage -= demage;
    }

    //reloads weapon skills
    internal void UpdateUnlocks()
    {
        WeaponData equipedWeapon = GameData.weapons[GameData.equipedWeaponNum];
        weaponAnim.SetBool("Chain Finisher", equipedWeapon.unlocks[0][0]);
        weaponAnim.SetBool("Special Attack", equipedWeapon.unlocks[0][1]);
        weaponAnim.SetBool("Special Finisher", equipedWeapon.unlocks[1][0]);
    }

}
