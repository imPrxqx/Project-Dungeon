using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotArrow : MonoBehaviour
{
    PlayerManagerScript playerManagerScript;

    [HideInInspector]
    public float demage;
    public float speed;
    private void Start()
    {
         StartCoroutine(ShotArrowForward());
    }
    //Trajectory for the arrow 
    public IEnumerator ShotArrowForward()
    {
        for(float i = 0; i <= Mathf.Infinity; i += 1)
        {
            transform.Translate(transform.forward * Time.fixedDeltaTime * speed, Space.World);
            yield return new WaitForSeconds(0.01f);
        }
    }
    //OnCollisionEnter is called when the gameobject hit collision
    private void OnCollisionEnter(Collision collision)
    {
        //If it is player then hit player and destroy the gameobject
        if (collision.collider.CompareTag("Player"))
        {
            TakeDemage(demage);
            Destroy(gameObject);
            //If it is ground then destroy gameobject
        } else if(collision.collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
    //Hit the player
    public void TakeDemage(float demage)
    {
        playerManagerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManagerScript>();
        playerManagerScript.TakeHit(demage);
    }
}
