using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiItem : MonoBehaviour, IPointerClickHandler
{

    public Item item;
    public Image spriteIcon;
    public Text amountTxt;

    //tells if its draging slot
    public bool dragItemUi;

    private static Item dragItem = null;

    // Start is called before the first frame update
    void Start()
    {
        spriteIcon = GetComponent<Image>();
    }

    public void Init()
    {
        spriteIcon = GetComponent<Image>();
        //clear the slot
        UpdateItem(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dragItemUi)
            return;
        UpdateItem(dragItem);
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,0);
    }

    
    //sets item and updates slot
    public void UpdateItem(Item item)
    {
        this.item = item;

        //if item == null clears the slot, otherwise sets the slot acording to item
        if (this.item != null)
        {
            //setting the Image
            spriteIcon.color = Color.white;
            spriteIcon.sprite = this.item.iconItem;

            //if the item is stackable set the amountTxt
            if (this.item.stackable)
            {
                this.amountTxt.text = this.item.amount.ToString();
            }
            else
            {
                this.amountTxt.text = "";
            }

        } else
        {
            //clearing the slot
            spriteIcon.color = Color.clear;
            this.amountTxt.text = "";
        }
    }

    //when mouse clicks on a slot
    public void OnPointerClick(PointerEventData eventData)
    {
        //if its draging slot, do nothing and return
        if (dragItemUi)
            return;

        //if no item is being draged, take item from the slot clicked
        if(dragItem == null) {
            if (item != null)
            {
                dragItem = item;
                UpdateItem(null);
                AudioManager.instance.Play("click");
            }
        }
        else
        {
            //if an item is being draged, update the clicked slot to that item and clear the draging slot
            if (item == null)
            {
                UpdateItem(dragItem);
                dragItem = null;
            }
            //if there is some item on clicked slot, just switch items
            else
            {
                var tempItem = item;
                UpdateItem(dragItem);
                dragItem = tempItem;
            }
            AudioManager.instance.Play("click");
        }

    }
}
