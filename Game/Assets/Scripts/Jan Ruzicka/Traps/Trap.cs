using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{

    //Base variables for inhenrited class
    PlayerManagerScript playerManagerScript;

    public float minTime;
    public float maxTime;
    [HideInInspector]
    public float coolDownTimer;
    public float demage = 20;
    public int tier;

    //Base method, which hit the player for inhenrited class
    public virtual void TakeDemage(float demage)
    {
        playerManagerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManagerScript>();
        playerManagerScript.TakeHit(demage);
    }
}
