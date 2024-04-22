using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{

    public GameObject inventoryCanvas;   


    //list of all subInventories
    public List<GameObject> subInventory = new List<GameObject>();
    int currentSubInventory = 0;

    // Update is called once per frame
    void Update()
    {
        //checks input for opening and closing 

        if (Input.GetKeyDown(OptionsScript.keys[12]))
        {
            if (!inventoryCanvas.activeSelf)
            {
                if (!GameData.inMenu)
                    OpenInventory();
            } else
            {
                CloseInventory();
            }
        }

        if (Input.GetKeyDown(OptionsScript.keys[11]) && inventoryCanvas.activeSelf)
        {
            CloseInventory();
        }
    }

    //opens inventory and takes control from player
    void OpenInventory()
    {
        subInventory[currentSubInventory].SetActive(true);
        GameData.inMenu = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        inventoryCanvas.SetActive(true);
    }

    //closes inventory and gives control to the player
    void CloseInventory()
    {
        subInventory[currentSubInventory].SetActive(false);
        GameData.inMenu = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inventoryCanvas.SetActive(false);
    }

    //switch to next subInventory
    public void Right()
    {
        AudioManager.instance.Play("click");

        subInventory[currentSubInventory].SetActive(false);
  
        if (currentSubInventory == subInventory.Count-1)
        {
            currentSubInventory = 0;
        }
        else
        {
            currentSubInventory += 1;
        }

        subInventory[currentSubInventory].SetActive(true);

    }

    //switch to previous subInventory
    public void Left()
    {
        AudioManager.instance.Play("click");

        subInventory[currentSubInventory].SetActive(false);
       
        if(currentSubInventory == subInventory.Count-subInventory.Count)
        {
            currentSubInventory = subInventory.Count-1;
        } else
        {
            currentSubInventory -= 1;
        }

        subInventory[currentSubInventory].SetActive(true);

    }

}
