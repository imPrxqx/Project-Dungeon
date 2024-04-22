using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private Transform PlayerTrans;

    //index of scene to load
    public int sceneNum = 1;

    public bool exitBoss = false;
    public bool finalFloor = false;

    void Start()
    {
        //finds Player and saves his Transform component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerTrans = player.transform;

       
    }

    void Update()
    {
        //If Boss exit, then portal is closed, if not then is opened
        if (GameObject.FindGameObjectWithTag("Boss"))
        {
            exitBoss = true;
        } else
        {
            exitBoss = false;
        }

        //if Player is near and presses "Interact" button - load next scene
        if (Input.GetKey(OptionsScript.keys[9]))
        {
            Vector3 distance = PlayerTrans.transform.position - transform.position;
            if (distance.magnitude <= 2)
            {
                if (finalFloor)
                    GameData.ResetMissions();

                GameData.SaveGameData();
                SceneManager.LoadScene(sceneNum);
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //if Player will enter Trigger - load next scene

        if (other.CompareTag("Player") && exitBoss == false)
        {
            if (finalFloor)
                GameData.ResetMissions();

            GameData.SaveGameData();
            SceneManager.LoadScene(sceneNum);
        }
    }
}
