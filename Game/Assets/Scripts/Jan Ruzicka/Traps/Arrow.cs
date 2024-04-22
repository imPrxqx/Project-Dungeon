using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arrow : Trap
{
    Transform arrowPoints;

    public float timer;
    float timerTime;

    public GameObject arrowPrefab;

    [HideInInspector]
    public bool triggered = false;
    [HideInInspector]
    public bool active = false;

    //List of all arrow points in parent gameobject
    public List<Transform> listArrowPoints = new List<Transform>();

    private void Start()
    {
        //Set damage for Trap from default 
        (demage) = GlobalFunctions.CalcTrap(tier, demage);

        //Set max time from pre-set timer
        timerTime = timer;

        //Get the gameobject who has this script
        arrowPoints = this.gameObject.transform;

        //Get from parent the gameobject all arrow points where will be shooting arrow
        foreach (Transform lootpoints in arrowPoints)
        {
            listArrowPoints.Add(lootpoints);
        }
    }

    void Update()
    { 
        //If timer <= then stop shooting arrows or else substract time from timer
        if(timer <= 0)
        {
            triggered = false;
        } else
        {
            timer -= Time.deltaTime;
        }

        //If the player triggered trap then start shooting arrows
        if (triggered == true && active == false)
        {
            StartCoroutine(FireArrow());
        }  
    }
    //OnTriggerEnter is called when the gameobject entered on collission trigger
    private void OnTriggerEnter(Collider other)
    {
        //If it is the player then start shooting arrows
        if (other.CompareTag("Player"))
        {
            if(triggered == false)
            {
                triggered = true;
                timer = timerTime;
            }       
        }
    }
    //Shoots arrows 
    public IEnumerator FireArrow()
    {
        //Active trap
        active = true;

        //Set time when it will shot next arrow
        coolDownTimer = GlobalFunctions.WaitTime(minTime, maxTime);

        for(int i = 0; i < listArrowPoints.Count;i++)
        {
            GameObject arrow = Instantiate(arrowPrefab, listArrowPoints[i].position, Quaternion.Euler(GlobalFunctions.GetRotation(listArrowPoints[i].transform)));
            arrow.GetComponent<ShotArrow>().demage = demage;
        }
        AudioManager.instance.Play("arrow");

        yield return new WaitForSeconds(coolDownTimer);
       
        //Deactive trap
        active = false;
    }

}
