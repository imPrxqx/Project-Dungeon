using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItems : MonoBehaviour
{

    public UiInventory invetoryUi;

    Item itemAdd;
    

    // Start is called before the first frame update
    void Start()
    {
        invetoryUi.LoadInventory();

        foreach (Item item in GameData.characterItems)
        {
            item.LoadImage();
            invetoryUi.AddNewItem(item);
        }
    }

    //Metoda na vlozeni Itemu + spritu do inventare
    public void GiveItem(int id)
    {
        itemAdd = DatabaseItems.GetItem(id);
        if (itemAdd.stackable)
        {
            foreach (Item item in GameData.characterItems)
            {
                if (item.titleItem == itemAdd.titleItem)
                {
                    item.amount += itemAdd.amount;
                    if (item.amount > item.maxStack)
                    {
                        itemAdd.amount = item.amount - item.maxStack;
                        item.amount = item.maxStack;
                    }
                    else
                    {
                        itemAdd.amount = 0;
                        break;
                    }
                }
            }
            if(itemAdd.amount > 0)
            {
                GameData.characterItems.Add(itemAdd);
                invetoryUi.AddNewItem(itemAdd);
            }
            invetoryUi.UpdateItems(itemAdd);
        }
        else
        {
            GameData.characterItems.Add(itemAdd);
            invetoryUi.AddNewItem(itemAdd);
        }
    }

    //Removes given item from inventory
    public void RemoveItem(Item item)
    {
        // if the item is stackable just lower the amount by 1
        if (item.stackable)
        {
            item.amount--;
            invetoryUi.UpdateItems(item);
        }

        //removing the item
        if (!item.stackable || item.amount <= 0)
        {
            GameData.characterItems.Remove(item);
            invetoryUi.RemoveItem(item);
            Debug.Log("Smazan item: " + itemAdd);
        }
    }
}
