using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipScript : MonoBehaviour
{
    Transform player;
    public GameObject equipMenu;
    public GameObject[] weapons;
    private GameObject weapon;
    private GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        //chcecking input for opening and closing equip station UI
        if ((player.position - transform.position).magnitude < 2 && Input.GetKeyDown(OptionsScript.keys[9]) && !GameData.inMenu)
        {
            GameData.inMenu = true;
            equipMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(OptionsScript.keys[11]) && equipMenu.activeSelf)
        {
            Exit();
        }
    }

    //closes inventory and gives control to the player
    public void Exit()
    {
        AudioManager.instance.Play("click");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        equipMenu.SetActive(false);
        GameData.inMenu = false;
    }

    //switch selected weapon with current weapon
    public void EquipWeapon(int index)
    {
        AudioManager.instance.Play("click");
        weapon = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<WeaponScript>().gameObject;
        Destroy(weapon);
        weapon = Instantiate(weapons[index], cam.transform);
        GameData.equipedWeaponNum = index;
    }
}
