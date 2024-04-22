using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroying : MonoBehaviour
{
    Transform player;

    float final;
    float width;

    [HideInInspector]
    public float demage;

    public GameObject particleProjectil;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;    
        StartCoroutine(Balling());       
    }
    //OnTriggerEnter is called when the gameobject is in collision trigger 
    private void OnTriggerEnter(Collider other)
    {
        //If it is ground then destroy the gameobject
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        //If it is player then hit the player and later destroy the gameobject
        if (other.CompareTag("Player"))
        {
            //Spawn particle
            GameObject particle = Instantiate(particleProjectil, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(particle, 1);

            PlayerManagerScript playerHit = player.GetComponent<PlayerManagerScript>();
            playerHit.TakeHit(demage);
            Destroy(gameObject);
        }
       
    }
    //Trajectory for projectil 
    IEnumerator Balling()
    {
        //Go up 
        for (float j = 0; j <= 15; j += 0.2f)
        {
            transform.Translate(transform.up * j * Time.fixedDeltaTime, Space.World);

            yield return new WaitForSeconds(0.001f);
        }
        //Wait 5s and aim the player
        yield return new WaitForSeconds(5);
        transform.LookAt(player);
        //Get distance in type float between player and gameobject
        width = GlobalFunctions.DistanceRange(player, transform);

        //Aim and go forward at goal
        for (float k = 0.1f; k <= Mathf.Infinity; k += 0.1f)
        {
            final = ((width / 10) * (k * k) + 15);
            transform.Translate(transform.forward * final * Time.fixedDeltaTime, Space.World);
            yield return new WaitForSeconds(0.01f);
        }   
       
    }
}
