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

    private Text currentItem;
    private int itemIndx;
    private int itemTypeIndx;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        currentItem = Items[0]; // set to first item
        itemIndx = 0;
        itemTypeIndx = 0;
        showItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (ItemList.gameObject.activeSelf && ItemList.interactable) {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                // only update if item hud is active and the item list is interactable
                if (currentItem.name == eventSystem.currentSelectedGameObject.name) {
                    // if was at the is on the last item, scroll down
                    Items item = GameManager.Instance.GetItemAt(itemIndx + Items.Length);
                    if (item != null) {
                        // more items, so scroll
                        itemIndx++;
                        showItems();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                // only update if item hud is active and the item list is interactable
                if (currentItem.name == eventSystem.currentSelectedGameObject.name) {
                    // if was at the is on the first item, scroll up
                    if (itemIndx > 0) {
                        // more items, so scroll
                        itemIndx--;
                        showItems();
                    }
                }
            }
            Text highlightedItem = eventSystem.currentSelectedGameObject.GetComponent<Text>();
            if (currentItem.name != highlightedItem.name) {
                // is different item, change description
                currentItem = highlightedItem;
                changeDescription();
            }
        }
    }

    private void showItems() {
        // update which item is being shown
        for (int i = 0; i < Items.Length; i++) {
            Text itemText = Items[i];
            itemText.GetComponent<Button>().interactable = true;
            Items item = GameManager.Instance.GetItemAt(i + itemIndx);
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
        string itemName = currentItem.text;
        Items item = GameManager.Instance.GetItemDetails(itemName);
        ItemAmount.text = "" + GameManager.Instance.GetAmountOfItem(itemName); // change the amount of the item
        Description.text = item.Description;
        if (item.IsItem) {
            // if is item, can use
            UseButton.gameObject.SetActive(true);
        } else {
            UseButton.gameObject.SetActive(false);
        }
    }

    public void SetItemType(int itemTypeIndx) {
        // called when choosing which type of item to view
        // when called, show the items and description of current item
        this.itemTypeIndx = itemTypeIndx;

        currentItem = eventSystem.currentSelectedGameObject.GetComponent<Text>();
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
