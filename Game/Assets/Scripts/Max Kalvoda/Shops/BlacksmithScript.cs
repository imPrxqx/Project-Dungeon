using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithScript : MonoBehaviour
{
    Transform player;

    public GameObject blacksmithMenu;
    public GameObject[] weaponPanels;

    public Text[] upgradeCosts; 
    public Text[] unlockCosts;

    public Text moneyText, xpText;

    private int weaponIndex;

    //gets players transform
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //handles opening and closeing blacksmith window
    void Update()
    {
        if((player.position - transform.position).magnitude < 2  && Input.GetKeyDown(OptionsScript.keys[9]) && !GameData.inMenu)
        {
            updateStatus();
            GameData.inMenu = true;
            blacksmithMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(OptionsScript.keys[11]) && blacksmithMenu.activeSelf)
        {
            Exit();
        }
    }

    //switches betwwen panels with different weapons
    public void SwitchWeaponPanel(int panelIndex)
    {
        AudioManager.instance.Play("click");

        for (int i = 0; i < weaponPanels.Length; i++)
        {
            if (i == panelIndex)
                weaponPanels[i].SetActive(true);
            else
                weaponPanels[i].SetActive(false);
        }
        weaponIndex = panelIndex;

        updateStatus();
    }

    // updates displayed money and xp
    public void updateStatus()
    {
        moneyText.text = GameData.money.ToString();
        xpText.text = GameData.weapons[weaponIndex].xp.ToString();

        if(GameData.weapons[weaponIndex].level < GameData.weapons[weaponIndex].upgradeCosts.Length)
            upgradeCosts[weaponIndex].text = GameData.weapons[weaponIndex].upgradeCosts[GameData.weapons[weaponIndex].level] + "G";
        else
            upgradeCosts[weaponIndex].text = "LEVEL MAX";

        int txtIndex = weaponIndex * 3;
        for (int i = 0; i < GameData.weapons[weaponIndex].unlocks.Length; i++)
        {
            for (int k = 0; k < GameData.weapons[weaponIndex].unlocks[i].Length; k++)
            {
                if (GameData.weapons[weaponIndex].unlocks[i][k])
                    unlockCosts[txtIndex].text = "UNLOCKED";
                txtIndex++;
            }
        }
    }

    // close the blacksmith window
    public void Exit()
    {
        AudioManager.instance.Play("click");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        blacksmithMenu.SetActive(false);
        GameData.inMenu = false;
    }

    //unlock new skill on selected weapon
    public void Unlock(int index)
    {
        WeaponScript weapon = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<WeaponScript>();
        if (GameData.UnlockWeapon(index, weaponIndex))
        {
            AudioManager.instance.Play("upgrade");
            weapon.UpdateUnlocks();
        }
        else
        {
            AudioManager.instance.Play("error");
        }

        updateStatus();
    }

    //upgrades selected weapon
    public void Upgrade(int weaponIndex)
    {
        if (GameData.weapons[weaponIndex].level < GameData.weapons[weaponIndex].upgradeCosts.Length)
        {
            if (GameData.weapons[weaponIndex].upgradeCosts[GameData.weapons[weaponIndex].level] <= GameData.money)
            {
                GameData.money -= GameData.weapons[weaponIndex].upgradeCosts[GameData.weapons[weaponIndex].level];
                GameData.weapons[weaponIndex].level++;
                AudioManager.instance.Play("upgrade");
            }
            else
            {
                AudioManager.instance.Play("error");
            }
        }

        updateStatus();
        GameData.SaveGameData();
    }
}
