using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject enemy;

    //If spawner can spawn enemy;
    bool can = true;

    // Update is called once per frame
    void Update()
    {
        //If spawner can spawn enemy then start coroutine Spawning
        if(can == true)
        {
            StartCoroutine(Spawning());
        }
    }
    //Spawn enemy in position spawner then wait 5s for set bool value on true
    IEnumerator Spawning()
    {
        can = false;
        Instantiate(enemy, transform.position + new Vector3(Random.Range(-5.0f, 5.0f), 1, Random.Range(-5.0f, 5.0f)), Quaternion.identity);
        yield return new WaitForSeconds(5);
        can = true;
    }
}
