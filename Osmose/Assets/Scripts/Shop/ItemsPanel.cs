using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemsPanel : MonoBehaviour {
    [Header("Canvas Group")]
    public CanvasGroup ItemPanel;

    [Header("UI References")]
    public Button[] ItemsButton;
    public Text[] ItemsName;
    public Text[] ItemsCost;
    public Text[] ItemsOwned;
    public Text Description;

    private Items[] itemsList;
    private string currItem;
    private int currItemIndx;
    private int itemIndx;
    private bool isBuying;
    private bool sellingItem;
    private bool allItems;

    private void Awake() {
        itemsList = null;
        currItem = "";
        currItemIndx = 0;
        itemIndx = 0;
        isBuying = true;
        sellingItem = true;
    }

    // Update is called once per frame
    void Update() {
        if (ItemPanel.interactable && Input.GetButtonDown("Vertical")) {
            Text currSelectedItem = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
            if (currItem == currSelectedItem.text) {
                // scroll
                float buttonInput = Input.GetAxisRaw("Vertical");
                if (buttonInput < -0.5f) {
                    // scroll down
                    if (isBuying && itemIndx + ItemsButton.Length < itemsList.Length - 1) {
                        // is buying
                        itemIndx++;
                        updateItemsList();
                    }
                    if (!isBuying && allItems && itemIndx + ItemsName.Length < GameManager.Instance.GetNumItems() + GameManager.Instance.GetNumEquipment()) {
                        itemIndx++;
                        updateAllList();
                    }
                    if (!isBuying && !allItems && sellingItem && GameManager.Instance.GetItemAt(itemIndx + ItemsButton.Length) != null) {
                        // is selling items
                        itemIndx++;
                        updateItemsList();
                    }
                    if (!isBuying && !allItems && !sellingItem && GameManager.Instance.GetEquipmentAt(itemIndx + ItemsButton.Length) != null) {
                        // is selling equipments
                        itemIndx++;
                        updateItemsList();
                    }
                }
                if (buttonInput > 0.5f && itemIndx > 0) {
                    // scroll up
                    itemIndx--;
                }
            }

            currItem = currSelectedItem.text;
            updateDescription();
        }
    }

    /// <summary>
    /// Set the Items list being listed
    /// </summary>
    /// <param name="itemsList">List of Items that are in the shop</param>
    public void SetItemsList(Items[] itemsList) {
        this.itemsList = itemsList;
        updateItemsList();
    }

    /// <summary>
    /// Get whether or not the player is buying
    /// </summary>
    /// <returns>Whether or not the player is buying</returns>
    public bool GetIsBuying() {
        return isBuying;
    }

    /// <summary>
    /// Set up when first go to Item List
    /// </summary>
    public void OpenItemList(bool buying, bool items) {
        isBuying = buying;
        sellingItem = items;
        allItems = false;
        EventSystem.current.SetSelectedGameObject(ItemsButton[0].gameObject);
        updateItemsList();
        currItem = ItemsName[0].text;
        updateDescription();
    }

    /// <summary>
    /// Set up when selling all items in inventory
    /// </summary>
    public void OpenAllList() {
        isBuying = false;
        sellingItem = false;
        allItems = true;
        EventSystem.current.SetSelectedGameObject(ItemsButton[0].gameObject);
        updateAllList();
        currItem = ItemsName[0].text;
        updateDescription();
    }

    /// <summary>
    /// Set the last clicked button as the current selected
    /// </summary>
    public void SetItem() {
        EventSystem.current.SetSelectedGameObject(ItemsButton[currItemIndx].gameObject);
        if (allItems) {
            updateAllList();
        } else {
            updateItemsList();
        }
        updateDescription();
    }

    /// <summary>
    /// Get the item at the index passed in
    /// </summary>
    /// <param name="itemChoice">Index of which button was clicked</param>
    /// <returns></returns>
    public Items GetItem(int itemChoice) {
        currItemIndx = itemChoice;
        if (isBuying) {
            return itemsList[itemChoice + itemIndx];
        } else {
            Items item = GameManager.Instance.GetItemDetails(ItemsName[itemChoice].text);
            if (item == null) {
                // if not item, is equipment
                item = GameManager.Instance.GetEquipmentDetails(ItemsName[itemChoice].text);
            }
            return item;
        }
    }

    /// <summary>
    /// Get the Y position of the button
    /// </summary>
    /// <param name="index">Index of the button</param>
    /// <returns>Y postiion of the button</returns>
    public float GetYPosOfButton(int index) {
        return ItemsName[index].transform.position.y;
    }

    /// <summary>
    /// Update the Items list
    /// </summary>
    private void updateItemsList() {
        for (int i = 0; i < ItemsName.Length; i++) {
            Items item = null;
            if (isBuying) {
                if (i + itemIndx >= itemsList.Length) {
                    // if there aren't enough items to fill the list, set text inactive
                    ItemsButton[i].gameObject.SetActive(false);
                    ItemsCost[i].gameObject.SetActive(false);
                    ItemsOwned[i].gameObject.SetActive(false);
                    continue;
                }
                item = itemsList[i + itemIndx];
            } else if (!isBuying && sellingItem){
                // get item from inventory
                item = GameManager.Instance.GetItemAt(i + itemIndx);
                if (item == null) {
                    // if there aren't enough items in inventory to fill list, set text inactive
                    ItemsButton[i].gameObject.SetActive(false);
                    ItemsCost[i].gameObject.SetActive(false);
                    ItemsOwned[i].gameObject.SetActive(false);
                    continue;
                }
            } else {
                // get equipment from inventory
                item = GameManager.Instance.GetEquipmentAt(i + itemIndx);
                if (item == null) {
                    // if there aren't enough items in inventory to fill list, set text inactive
                    ItemsButton[i].gameObject.SetActive(false);
                    ItemsCost[i].gameObject.SetActive(false);
                    ItemsOwned[i].gameObject.SetActive(false);
                    continue;
                }
            }
            ItemsButton[i].gameObject.SetActive(true);
            ItemsCost[i].gameObject.SetActive(true);
            ItemsOwned[i].gameObject.SetActive(true);

            ItemsName[i].text = item.ItemName;
            if (isBuying) {
                ItemsCost[i].text = "" + item.Value;
            } else {
                ItemsCost[i].text = "" + Mathf.Max(Mathf.RoundToInt(item.Value * 0.5f), 1);
            }
            if (item.IsItem) {
                ItemsOwned[i].text = "" + GameManager.Instance.GetAmountOfItem(item.ItemName);
            } else {
                ItemsOwned[i].text = "" + GameManager.Instance.GetAmountOfEquipment(item.ItemName);
            }
        }
    }

    // update the list for selling all things in inventory
    private void updateAllList() {
        for (int i = 0; i < ItemsName.Length; i++) {
            Items item = null;
            if (i + itemIndx < GameManager.Instance.GetNumItems()) {
                item = GameManager.Instance.GetItemAt(i + itemIndx);
            } else {
                item = GameManager.Instance.GetEquipmentAt(i + itemIndx - GameManager.Instance.GetNumItems());
            }
            if (item == null) {
                // if there aren't enough items in inventory to fill list, set text inactive
                ItemsButton[i].gameObject.SetActive(false);
                ItemsCost[i].gameObject.SetActive(false);
                ItemsOwned[i].gameObject.SetActive(false);
                continue;
            }
            ItemsButton[i].gameObject.SetActive(true);
            ItemsCost[i].gameObject.SetActive(true);
            ItemsOwned[i].gameObject.SetActive(true);
            ItemsName[i].text = item.ItemName;
            ItemsCost[i].text = "" + Mathf.Max(Mathf.RoundToInt(item.Value * 0.5f), 1);
            if (item.IsItem) {
                ItemsOwned[i].text = "" + GameManager.Instance.GetAmountOfItem(item.ItemName);
            } else {
                ItemsOwned[i].text = "" + GameManager.Instance.GetAmountOfEquipment(item.ItemName);
            }
        }
    }

    /// <summary>
    /// Update the description
    /// </summary>
    private void updateDescription() {
        Items item = GameManager.Instance.GetItemDetails(currItem);
        if (item == null) {
            // if not item, is equipment
            item = GameManager.Instance.GetEquipmentDetails(currItem);
        }
        Description.text = item.Description;
    }
}
