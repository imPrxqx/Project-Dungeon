using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for weapon animation events
public class AttackScript : MonoBehaviour
{
    WeaponScript ws;

    // Start is called before the first frame update
    private void Start()
    {
        //gets the WeaponScript component
        ws = gameObject.GetComponentInParent<WeaponScript>();
    }

    //calls the attack method on WeaponScript
    public void Attack()
    {
        ws.Attack(0);
    }


    //calls the attack method on WeaponScript
    public void Special()
    {
        ws.Attack(1);
    }

    //resets the Attack trigger on weapon animator
    public void ResetTrig()
    {
        ws.weaponAnim.ResetTrigger("Attack");
        ws.weaponAnim.ResetTrigger("Special");
    }
}
