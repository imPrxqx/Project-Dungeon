using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchymistScript : MonoBehaviour
{
    Transform player;
    public GameObject alchymistMenu;
    public Animator buyButtonAnim;

    int selectedItemIndex = 0;

    public Text titleTxt;
    public Text infoTxt;
    public Text priceTxt;
    public Text moneyTxt;

    private const int maxPotionLvl = 5;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        DatabaseItems.addItems();
        Item item = DatabaseItems.GetItem(selectedItemIndex);
        titleTxt.text = item.titleItem;
        infoTxt.text = item.descriptionItem;
        priceTxt.text = "PRICE: " + item.cena;
        moneyTxt.text = GameData.money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //chcecking input for opening and closing alchymist UI
        if ((player.position - transform.position).magnitude < 2 && Input.GetKeyDown(OptionsScript.keys[9]))
        {
            if (!GameData.inMenu)
            {
                GameData.inMenu = true;
                alchymistMenu.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if (Input.GetKeyDown(OptionsScript.keys[11]) && alchymistMenu.activeSelf)
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
        alchymistMenu.SetActive(false);
        GameData.inMenu = false;
    }

    //closes current pannel and switch to new one
    public void SwitchItemPanel(int newIndex)
    {
        AudioManager.instance.Play("click");
        selectedItemIndex = newIndex;
        Item item = DatabaseItems.GetItem(selectedItemIndex);
        titleTxt.text = item.titleItem;
        infoTxt.text = item.descriptionItem;
        priceTxt.text = "PRICE: " + item.cena;
        moneyTxt.text = GameData.money.ToString();
    }

    //buy selected item and add it to inventory
    public void BuyItem()
    {
        Item item = DatabaseItems.GetItem(selectedItemIndex);

        //Health potion upgrade
        if (selectedItemIndex == 0)
        {
            if (GameData.potionLvl < maxPotionLvl && GameData.money >= item.cena)
            {
                GameData.potionLvl++;
                GameData.money -= item.cena;
                item.cena = GameData.potionLvl * GameData.potionLvl * 300;
                buyButtonAnim.Play("Base Layer.SucAnim", 0);
                AudioManager.instance.Play("upgrade");
            }
            else
            {
                buyButtonAnim.Play("Base Layer.ErrAnim", 0);
                AudioManager.instance.Play("error");
            }
        }

        //normal item
        if (GameData.money >= item.cena)
        {
            GameData.money -= item.cena;
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            InventoryItems myInventory = controller.GetComponent<InventoryItems>();
            myInventory.GiveItem(selectedItemIndex);
            buyButtonAnim.Play("Base Layer.SucAnim", 0);
            AudioManager.instance.Play("upgrade");
        }
        else
        {
            buyButtonAnim.Play("Base Layer.ErrAnim", 0);
            AudioManager.instance.Play("error");
        }

        moneyTxt.text = GameData.money.ToString();
        GameData.SaveGameData();
    }

}

