using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMenu : MonoBehaviour {
    [Header("Item Type")]
    public CanvasGroup ItemTypePanel;
    public Button[] ItemType;

    [Header("Item List")]
    public CanvasGroup ItemList;
    public Text[] Items;

    [Header("Item Description")]
    public Image ItemImage;
    public Text ItemAmount;
    public Text Description;
    public Button UseButton;
    public Button DiscardButton;
    
    private string currentItem; // keep track of what item the player is currently on
    private int currItemIndx; // keep track which item the player clicked on
    private int itemIndx; // keep track of where in the items list the player is on (this allows for scrolling)

    private string currentType; // keep track of what type of item player is currently looking at
    private int itemTypeIndx; // keep track of which index of the type of item player is currently looking at

    // Start is called before the first frame update
    void Start() {
        currentItem = Items[0].text; // set to first item
        itemIndx = 0;
        currentType = ItemType[0].GetComponentInChildren<Text>().text; // set to first item type
        itemTypeIndx = 0;
        updateItems();
    }

    // Update is called once per frame
    void Update() {
        if (ItemTypePanel.interactable) {
            // if on type type panel
            Text currHighlightedType = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
            if (currentType != currHighlightedType.text) {
                currentType = currHighlightedType.text;

                switch(currentType) {
                    case "Items":
                        itemTypeIndx = 0;
                        break;
                    case "Equipment":
                        itemTypeIndx = 1;
                        break;
                    case "Key":
                        itemTypeIndx = 2;
                        break;
                }

                updateItems();
            }
        }

        if (ItemList.gameObject.activeSelf && ItemList.interactable && Items[0].text != "") {
            // if itemlist is interactable and there are interactable buttons
            if (Input.GetButtonDown("Vertical")) {
                float buttonInput = Input.GetAxisRaw("Vertical");
                if (buttonInput < -0.5f) {
                    // only update if item hud is active and the item list is interactable
                    if (currentItem == EventSystem.current.currentSelectedGameObject.GetComponent<Text>().text) {
                        // if was at the is on the last item, scroll down
                        int i = itemIndx + Items.Length;
                        Items item = getItem(itemIndx + Items.Length);
                        if (item != null) {
                            // more items, so scroll
                            itemIndx++;
                            updateItems();
                        }
                    }
                }
                if (buttonInput > 0.5f) {
                    // only update if item hud is active and the item list is interactable
                    if (currentItem == EventSystem.current.currentSelectedGameObject.GetComponent<Text>().text) {
                        // if was at the is on the first item, scroll up
                        if (itemIndx > 0) {
                            // more items, so scroll
                            itemIndx--;
                            updateItems();
                        }
                    }
                }
            }

            Text highlightedItem = null;
            if (EventSystem.current.currentSelectedGameObject != null) {
                highlightedItem = EventSystem.current.currentSelectedGameObject.GetComponent<Text>();
            }

            if (currentItem != highlightedItem.text) {
                // not null
                // is different item, change description
                currentItem = highlightedItem.text;
            }
        }

        if (!ItemTypePanel.interactable) {
            // if item type panel is not interactable, can update description
            updateDescription();
        }
    }

    private Items getItem(int indx) {
        Items item = null;
        switch(itemTypeIndx) {
            case 0:
                // item
                item = GameManager.Instance.GetItemAt(indx);
                break;
            case 1:
                // equipment
                item = GameManager.Instance.GetEquipmentAt(indx);
                break;
            case 2:
                // key item
                item = GameManager.Instance.GetKeyItemAt(indx);
                break;
        }
        return item;
    }

    private void updateItems() {
        // update which item is being shown
        for (int i = 0; i < Items.Length; i++) {
            Text itemText = Items[i];
            itemText.GetComponent<Button>().interactable = true;
            Items item = getItem(i + itemIndx);
            if (item == null) {
                // if there is no item at this slot, disable it
                itemText.text = "";
                itemText.GetComponent<Button>().interactable = false;
                continue;
            }
            itemText.text = item.ItemName;
        }
    }

    private void updateDescription() {
        // change the description to the current highlighted item
        string itemName = currentItem;
        Items item = null;
        int amount = 0;
        switch(itemTypeIndx) {
            case 0:
                // is item
                item = GameManager.Instance.GetItemDetails(itemName);
                amount = GameManager.Instance.GetAmountOfItem(itemName);
                break;
            case 1:
                // is equipment
                item = GameManager.Instance.GetEquipmentDetails(itemName);
                amount = GameManager.Instance.GetAmountOfEquipment(itemName);
                break;
            case 2:
                // is key item
                item = GameManager.Instance.GetKeyItemDetails(itemName);
                break;
        }
        if (item == null) {
            ItemAmount.text = "";
        } else if (itemTypeIndx != 2) {
            // if not key item, get the amount
            ItemAmount.text = "" + amount; // change the amount of the item
        } else {
            // else, amount = 1
            ItemAmount.text = "1";
        }

        if (item != null) {
            Description.text = item.Description;
            if (item.IsItem) {
                // if is item, can use
                UseButton.gameObject.SetActive(true);
            } else {
                UseButton.gameObject.SetActive(false);
            }
        } else {
            Description.text = "";
            UseButton.gameObject.SetActive(false);
            DiscardButton.gameObject.SetActive(false);
        }
    }

    // when go to items list
    public void SetItemType(int itemTypeIndx) {
        // called when choosing which type of item to view
        // when called, show the items and description of current item
        this.itemTypeIndx = itemTypeIndx;
        
        updateItems();
        currentItem = EventSystem.current.currentSelectedGameObject.GetComponent<Text>().text;
        DiscardButton.gameObject.SetActive(true);
        updateDescription();
    }

    public void OpenDescriptionPanel(int item) {
        currItemIndx = item;
        if (UseButton.interactable) {
            EventSystem.current.SetSelectedGameObject(UseButton.gameObject);
        } else {
            EventSystem.current.SetSelectedGameObject(DiscardButton.gameObject);
        }
    }

    public string GetClickedItem() {
        return Items[currItemIndx].text;
    }

    // set to previous item if use up current item
    public void SetNewItem() {
        updateItems();
        currItemIndx--;
        if (currItemIndx < 0) {
            // if on first item, stay on first one
            currItemIndx = 0;
        }
        EventSystem.current.SetSelectedGameObject(Items[currItemIndx].gameObject);
        currentItem = Items[currItemIndx].text;
        updateDescription();
    }

    // exit select menu to the item menu
    public void ExitSelectMenu() {
        updateDescription();
        EventSystem.current.SetSelectedGameObject(UseButton.gameObject);
    }

    public void ExitItemList() {
        if (Items[0].text != "") {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
        }

        UseButton.gameObject.SetActive(false);
        DiscardButton.gameObject.SetActive(false);
        ItemAmount.text = "";
        Description.text = "";

        itemIndx = 0;
        currentItem = "";
        currItemIndx = 0;

        EventSystem.current.SetSelectedGameObject(ItemType[itemTypeIndx].gameObject);
    }

    public void ExitDescriptionPanel() {
        // set the clicked item as selected item
        EventSystem.current.SetSelectedGameObject(Items[currItemIndx].gameObject);
    }
}
