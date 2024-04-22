using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter : MonoBehaviour
{
    [HideInInspector]
    public bool entered = false;

    //OnTriggerEnter is called if the player is inside collission trigger of the the gameobject and set bool value on true
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            entered = true;
        }
    }
    //OnTriggerExit is called if the player left collission trigger of the gameobject and set bool value on false
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            entered = false;
        }
    }
}
