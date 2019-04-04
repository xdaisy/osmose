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
    private int itemIndx;
    private bool isBuying;
    private bool sellingItem;

    private void Awake() {
        itemsList = null;
        currItem = "";
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
                        itemIndx++;
                        updateItemsList();
                    }
                    if (!isBuying && sellingItem && GameManager.Instance.GetItemAt(itemIndx + ItemsButton.Length) != null) {
                        itemIndx++;
                        updateItemsList();
                    }
                    if (!isBuying && !sellingItem && GameManager.Instance.GetEquipmentAt(itemIndx + ItemsButton.Length) != null) {
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
    /// Set whether or not the player is buying or selling
    /// </summary>
    /// <param name="isBuying">Flag for if the player is buying</param>
    public void SetIsBuying(bool isBuying) {
        this.isBuying = isBuying;
    }

    /// <summary>
    /// Get whether or not the player is buying
    /// </summary>
    /// <returns>Whether or not the player is buying</returns>
    public bool GetIsBuying() {
        return isBuying;
    }

    /// <summary>
    /// Set the current selected buttom to the top item
    /// </summary>
    public void SetItemsButton() {
        EventSystem.current.SetSelectedGameObject(ItemsButton[0].gameObject);
        updateItemsList();
        currItem = ItemsName[0].text;
        updateDescription();
    }

    /// <summary>
    /// Set whether the player is selling items or equipment
    /// </summary>
    /// <param name="sellingItem">Flag for whether player is selling items or equipments</param>
    public void SetSellingItem(bool sellingItem) {
        this.sellingItem = sellingItem;
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

    /// <summary>
    /// Update the description
    /// </summary>
    private void updateDescription() {
        Items item = null;
        if (isBuying || sellingItem) {
            item = GameManager.Instance.GetItemDetails(currItem);
        } else {
            item = GameManager.Instance.GetEquipmentDetails(currItem);
        }
        Description.text = item.Description;
    }
}
