using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IBreakable
{
    public float health;
    float maxHealth;

    void Start()
    {
        //Set max health for the shield
        maxHealth = health;
    }
    //Substract health from the shield 
    public void TakeDamage(float demage)
    {
        health -= demage;
        //If shield is 0 or lower, disable the shield and set same heatlh from start
        if(health <= 0)
        {
            gameObject.SetActive(false);
            health = maxHealth;
        }
    }
}
