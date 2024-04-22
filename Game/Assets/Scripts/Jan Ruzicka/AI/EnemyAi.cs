using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAi : MonoBehaviour
{

    //Base variables for inhenrited class
    public float minTime;
    public float maxTime;
    [HideInInspector]
    public float coolDownTimer;
    public float health = 100;
    public float demage = 20;
    public int tier;
    public int xp = 20;
    public int money = 10;
    public string enemyName;
    
    [HideInInspector]
    public Rigidbody rb;

    //Base method, which adds money, xp and check mission if enemy has 0 or lower health for inhenrited class
    public virtual void TakeDemage(float demage)
    {
        health -= demage;
        if(health <= 0)
        {
            GameData.money += money;
            GameData.AddXp(xp);
            GameData.CheckMission("hunt", enemyName);               
        }
    }
    //Base method, which apply force for enemy when hit for inhenrited class
    public virtual void ApplyForce(Vector3 force)
    {
        rb.AddForce(new Vector3(force.x,(force.y + 1) * 3, force.z), ForceMode.Impulse);
    }
    //Base method, where enemy look at player for inhenrited class
    public virtual void LookAtPlayer()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(player);
    }
}
