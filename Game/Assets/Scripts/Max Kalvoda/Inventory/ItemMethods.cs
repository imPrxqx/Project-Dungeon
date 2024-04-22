using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemActions
{
    //dictionary of item methods
    private static Dictionary<int, System.Action> itemMethods;

    //player manager reference
    private static PlayerManagerScript playerManager;

    //item prefabs for spawning
    private static GameObject knife;
    private static GameObject bomb;
    private static GameObject trap;

    //loading prefabs and setting up dictionary
    public static void init(PlayerManagerScript playerManager)
    {
        ItemActions.playerManager = playerManager;
        knife = (GameObject) Resources.Load("Prefabs/ThrowingKnife");
        bomb = (GameObject)Resources.Load("Prefabs/Bomb");
        trap = (GameObject)Resources.Load("Prefabs/Trap");

        itemMethods = new Dictionary<int, System.Action>();
        itemMethods.Add(1, ItemActions.StrengthPotion);
        itemMethods.Add(2, ItemActions.StrengthPotion2);
        itemMethods.Add(3, ItemActions.StrengthPotion3);
        itemMethods.Add(4, ItemActions.SpeedPotion);
        itemMethods.Add(5, ItemActions.SpeedPotion2);
        itemMethods.Add(6, ItemActions.SpeedPotion3);
        itemMethods.Add(7, ItemActions.FocusPotion);
        itemMethods.Add(8, ItemActions.FocusPotion2);
        itemMethods.Add(9, ItemActions.FocusPotion3);
        itemMethods.Add(10, ItemActions.ThrowingKnife);
        itemMethods.Add(11, ItemActions.Bomb);
        itemMethods.Add(12, ItemActions.Trap);
    }

    //calls chosen method from dictionary
    public static void DoAction(int index)
    {
        itemMethods[index]();
    }

    //use Strenght potion item
    public static void StrengthPotion()
    {
        GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
        WeaponScript WS = weapon.GetComponent<WeaponScript>();
        WS.StartCoroutine(WS.BoostWeapon(15, 20));
    }

    //use Strenght potion II item
    public static void StrengthPotion2()
    {
        GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
        WeaponScript WS = weapon.GetComponent<WeaponScript>();
        WS.StartCoroutine(WS.BoostWeapon(20, 50));
    }

    //use Strenght potion III item
    public static void StrengthPotion3()
    {
        GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
        WeaponScript WS = weapon.GetComponent<WeaponScript>();
        WS.StartCoroutine(WS.BoostWeapon(30, 100));
    }

    //use Throwing Knife item
    public static void ThrowingKnife()
    {
        GameObject projectile = Object.Instantiate(knife, playerManager.transform.position + new Vector3(0,0.5f,0), playerManager.transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        PlayerMovement pm = playerManager.GetComponent<PlayerMovement>();
        Vector3 forward = pm.myCamera.transform.forward;
        rb.AddForce(forward * 50);
    }

    //use Bomb item
    public static void Bomb()
    {
        GameObject projectile = Object.Instantiate(bomb, playerManager.transform.position + new Vector3(0, 0.5f, 0), playerManager.transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        PlayerMovement pm = playerManager.GetComponent<PlayerMovement>();
        Vector3 forward = pm.myCamera.transform.forward;
        rb.AddForce(forward * 50);
    }

    //use Trap item
    public static void Trap()
    {
        Object.Instantiate(trap, playerManager.transform.position + new Vector3(0, -0.9f, 0), playerManager.transform.rotation);
    }

    //use Speed potion item
    public static void SpeedPotion()
    {
        PlayerMovement PM = playerManager.GetComponent<PlayerMovement>();
        PM.StartCoroutine(PM.BoostSpeed(20, 1.4f));
    }

    //use Speed potion II item
    public static void SpeedPotion2()
    {
        PlayerMovement PM = playerManager.GetComponent<PlayerMovement>();
        PM.StartCoroutine(PM.BoostSpeed(30, 1.8f));
    }

    //use Speed potion III item
    public static void SpeedPotion3()
    {
        PlayerMovement PM = playerManager.GetComponent<PlayerMovement>();
        PM.StartCoroutine(PM.BoostSpeed(30, 2.5f));
    }

    //use Focus potion item
    public static void FocusPotion()
    {
        PlayerMovement PM = playerManager.GetComponent<PlayerMovement>();
        PM.StartCoroutine(PM.Focus(4, 0.8f));
    }

    //use Focus potion II item
    public static void FocusPotion2()
    {
        PlayerMovement PM = playerManager.GetComponent<PlayerMovement>();
        PM.StartCoroutine(PM.Focus(10, 0.6f));
    }

    //use Focus potion III item
    public static void FocusPotion3()
    {
        PlayerMovement PM = playerManager.GetComponent<PlayerMovement>();
        PM.StartCoroutine(PM.Focus(15, 0.4f));
    }
}
