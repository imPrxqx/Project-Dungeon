using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int idItem;
    public int cena;

    [JsonIgnore]
    public Sprite iconItem;

    public string descriptionItem;
    public string titleItem;

    public bool stackable = false;
    public int maxStack = 1;
    public int amount = 1;

    //construktor for unstackable items
    public Item(int idItem,int cena, string titleItem, string descriptionItem)
    {
        this.idItem = idItem;
        this.cena = cena;
        this.titleItem = titleItem;
        this.descriptionItem = descriptionItem;
        LoadImage();
    }

    //construktor for stackable items
    public Item(int idItem, int cena, string titleItem, string descriptionItem, int maxStack)
    {
        this.idItem = idItem;
        this.cena = cena;
        this.titleItem = titleItem;
        this.descriptionItem = descriptionItem;
        this.stackable = true;
        this.maxStack = maxStack;
        LoadImage();
    }

    //parametrless construktor for savig to json
    public Item()
    {
        this.idItem = 0;
        this.cena = 0;
        this.titleItem = "";
        this.descriptionItem = "";
    }

    //loads image from Resources 
    public void LoadImage()
    {
        this.iconItem = Resources.Load<Sprite>("Sprites/Items/" + titleItem);
    }

    //makes a clone of this item and returns it
    public object Clone()
    {
        return this.MemberwiseClone();
    }

}
