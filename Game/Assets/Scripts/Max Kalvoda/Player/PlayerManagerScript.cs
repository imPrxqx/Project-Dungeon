using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManagerScript : MonoBehaviour
{
    //Player stats
    public float health;
    public float mana;
    private const float maxHealth = 100;

    //HUD sliders
    public Slider healthSlider;
    public Slider manaSlider;

    //inventory variables
    public InventoryItems inventory;
    public UiItem[] equipSlots;

    //Player animator
    private Animator anim;

    //amount of potions
    private int potions = 0;

    // Start is called before the first frame update
    void Start()
    {
        (health, mana) = GlobalFunctions.CalcPlayer(health, mana);
 
        //setting up HUD sliders
        healthSlider.maxValue = health;
        healthSlider.value = health;
        manaSlider.maxValue = mana;
        manaSlider.value = mana;

        //getting animator and potions
        anim = gameObject.GetComponent<Animator>();
        potions = GameData.potionLvl; //mozna se jeste zmeni

        //initiate itemactions
        ItemActions.init(this);
    }

    private void Update()
    {
        if (GameData.inMenu)
        {
            return;
        }

        if (Input.GetKeyDown(OptionsScript.keys[10]))//use heal potion
        {
            if (potions > 0)
            {
                health += ((GameData.potionLvl - 1) / 3) * 100 + 100;
                if(health > maxHealth) 
                    health = maxHealth;
                healthSlider.value = health;
                potions--;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(OptionsScript.keys[i+14]))//use item
            {
                if(equipSlots[i].item != null)
                {
                    try
                    {
                        ItemActions.DoAction(equipSlots[i].item.idItem);
                        inventory.RemoveItem(equipSlots[i].item);
                    }
                    catch
                    {
                        Debug.Log("Item cant be used");
                    }
                }
            }
        }
    }

    //Player takes demage and potentially dies
    public void TakeHit(float demage)
    {
        //take demage
        health -= demage;

        //if health id below 0, die
        if(health <= 0)
        {
            PlayerMovement PM = gameObject.GetComponent<PlayerMovement>();
            PM.enabled = false;
            WeaponScript WS = gameObject.GetComponentInChildren<WeaponScript>();
            WS.enabled = false;
            anim.enabled = true;
        }

        //set the health slider
        healthSlider.value = health;
    }

    //called when player dies and the death animation ends
    public void Die()
    {
        GameData.floor = 0;
        GameData.levelfloor = -1;
        GameData.version = 0;
        GameData.characterItems = new List<Item>();
        GameData.SaveGameData();
        GameData.ResetMissions();
        SceneManager.LoadScene(2);
    }
}