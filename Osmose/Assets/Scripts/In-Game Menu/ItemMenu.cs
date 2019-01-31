using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMenu : MonoBehaviour {
    private EventSystem eventSystem;

    public Button[] ItemType;

    public CanvasGroup ItemList;

    public Text[] Items;

    public Image ItemImage;
    public Text ItemAmount;
    public Text Description;
    public Button UseButton;
    public Button DiscardButton;

    //private Text currentItem;
    private string currentItem;
    private int itemIndx;
    private int itemTypeIndx;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        currentItem = Items[0].text; // set to first item
        itemIndx = 0;
        itemTypeIndx = 0;
        showItems();
    }

    // Update is called once per frame
    void Update() {
        if (ItemList.gameObject.activeSelf && ItemList.interactable) {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                // only update if item hud is active and the item list is interactable
                if (currentItem == eventSystem.currentSelectedGameObject.GetComponent<Text>().text) {
                    // if was at the is on the last item, scroll down
                    int i = itemIndx + Items.Length;
                    Items item = getItem(itemIndx + Items.Length);
                    if (item != null) {
                        // more items, so scroll
                        itemIndx++;
                        showItems();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                // only update if item hud is active and the item list is interactable
                if (currentItem == eventSystem.currentSelectedGameObject.GetComponent<Text>().text) {
                    // if was at the is on the first item, scroll up
                    if (itemIndx > 0) {
                        // more items, so scroll
                        itemIndx--;
                        showItems();
                    }
                }
            }
            Text highlightedItem = null;
            if (eventSystem.currentSelectedGameObject != null) {
                highlightedItem = eventSystem.currentSelectedGameObject.GetComponent<Text>();
            }

            if (highlightedItem != null && currentItem != highlightedItem.text) {
                // not null
                // is different item, change description
                currentItem = highlightedItem.text;
                changeDescription();
            }
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

    private void showItems() {
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

    private void changeDescription() {
        // change the description to the current highlighted item
        string itemName = currentItem;
        Items item = null;
        switch(itemTypeIndx) {
            case 0:
                // is item
                item = GameManager.Instance.GetItemDetails(itemName);
                break;
            case 1:
                // is equipment
                item = GameManager.Instance.GetEquipmentDetails(itemName);
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
            ItemAmount.text = "" + GameManager.Instance.GetAmountOfItem(itemName); // change the amount of the item
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

    public void SetItemType(int itemTypeIndx) {
        // called when choosing which type of item to view
        // when called, show the items and description of current item
        this.itemTypeIndx = itemTypeIndx;

        currentItem = eventSystem.currentSelectedGameObject.GetComponent<Text>().text;
        DiscardButton.gameObject.SetActive(true);
        showItems();
        changeDescription();
    }

    public void ExitItemList() {
        UseButton.gameObject.SetActive(false);
        DiscardButton.gameObject.SetActive(false);
        ItemAmount.text = "";
        Description.text = "";

        eventSystem.SetSelectedGameObject(ItemType[itemTypeIndx].gameObject);
    }
}
