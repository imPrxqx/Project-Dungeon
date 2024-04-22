using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DatabaseItems
{

    public static List<Item> itemsList = new List<Item>();

    //returns Item with given id from itemList
    public static Item GetItem(int id)
    {
        return (Item) itemsList.Find(item => item.idItem == id).Clone();
    }


    public static void addItems()
    {
        //Seznam itemu, ktere existuji
        itemsList =  new List<Item>() {

            //----------------ALCHYMIST ITEMS-----------------------------
            new Item(0, GameData.potionLvl *  GameData.potionLvl * 300, "Health potion", "yummi yummi, player's health goes brrrrrrrrr"),
            new Item(1, 200, "Strength Potion", "REEEEE"),
            new Item(2, 400, "Strength Potion II", "REEEEEEEEEEEEE"),
            new Item(3, 1000, "Strength Potion III", "REEEEEEEEEEEEEEEEEEEEEEEEEEE"),
            new Item(4, 180, "Speed potion", "whosh whosh"),
            new Item(5, 300, "Speed potion II", "whosh whosh zoom"),
            new Item(6, 800, "Speed potion III", "whosh whosh zoom brrrr"),
            new Item(7, 500, "Focus potion", "mmmmmmmmmm"),
            new Item(8, 900, "Focus potion II", "ooooooooooo"),
            new Item(9, 1400, "Focus potion III", "ooooommmmmmm"),
            new Item(10, 50, "Throwing knife", "RATATATA", 20),
            new Item(11, 200, "Bomb", "KABOOM", 5),
            new Item(12, 100, "Trap", "ouch", 8),
            //------------------------------------------------------------

            new Item(9, 50, "Image", "whosh whosh zoom brrrr"),
            new Item(10,100,"Image","Ukovaný meè"),
            new Item(11,120,"Image1","Ukovaný meè1"),
            new Item(12,300,"Image2","Ukovaný meè2"),
            new Item(13,500,"Image3","Ukovaný meè3"),
            new Item(14,250,"Image4","Ukovaný meè4")
        
        };
    }
}
