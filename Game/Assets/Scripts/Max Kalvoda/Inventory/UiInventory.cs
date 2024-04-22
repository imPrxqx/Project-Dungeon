using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInventory : MonoBehaviour
{
    public List<UiItem> uiitems = new List<UiItem>();
    public List<Transform> slotPanel = new List<Transform>();
    public GameObject slotPrefab;

    //sets up the inventory
    public void LoadInventory()
    {
        for(int i = 0; i < slotPanel.Count;i++)
        {
            foreach (Transform slots in slotPanel[i])
            {
                try
                {
                    UiItem slot = slots.GetComponentInChildren<UiItem>();
                    slot.Init();
                    uiitems.Add(slot);
                }
                catch
                {
                    Debug.LogWarning("slot is null");
                }
            }
        }
      
    }


    //updates selected slot to given item
    public void UpdateSlot(int slot, Item item)
    {
        uiitems[slot].UpdateItem(item);
    }

    //goes trought all slots and updates every slot with same item as given
    public void UpdateItems(Item item)
    {
        foreach( UiItem itemSlot in uiitems){
            if (itemSlot.item != null)
                if(itemSlot.item.titleItem == item.titleItem)
                    itemSlot.UpdateItem(itemSlot.item); 
        }
    }

    //adds new item to first empty slot
    public void AddNewItem(Item item)
    {
        UpdateSlot(uiitems.FindIndex(i => i.item == null), item);
    }

    //removes item from inventory, by setting it to null and updates nessesary slot
    public void RemoveItem(Item item)
    {
        UpdateSlot(uiitems.FindIndex(i => i.item == item), null);
    }
}
