﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemHud : MonoBehaviour {
    public CanvasGroup Hud;

    [Header("Item UI")]
    public Text[] Items;
    public Text[] ItemsAmount;

    [Header("Description")]
    public Text Description;

    private string currItem;
    private int itemIndx;

    private int clickedItem;

    private void Awake() {
        currItem = "";
        itemIndx = 0;
        clickedItem = 0;
    }

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        if (Hud.interactable && Input.GetButtonDown("Vertical")) {
            Text selectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<Text>();
            if (Input.GetButtonDown("Vertical")) {
                float buttonInput = Input.GetAxisRaw("Vertical");
                if (buttonInput < -0.5f) {
                    // if scrolling down
                    if (selectedItem.text == currItem) {
                        // if at last item text
                        int i = itemIndx + Items.Length;
                        Items item = GameManager.Instance.GetItemAt(i);
                        if (item != null) {
                            // if not at end of items list
                            itemIndx++;
                            updateItems();
                        }
                    }
                }

                if (buttonInput > 0.5f && selectedItem.text == currItem && itemIndx > 0) {
                    // if scrolling up and not at beginning of items list
                    itemIndx--;
                    updateItems();
                }
            }

            // update the current selected item
            currItem = selectedItem.text;
            updateDescription();
        }
    }

    // open the item hud
    public void OpenItemHud() {
        updateItems();
        EventSystem.current.SetSelectedGameObject(Items[0].gameObject);
        currItem = Items[0].text;
        updateDescription();
    }

    // exit the item hud
    public void ExitItemHud() {
        currItem = "";
        itemIndx = 0;
        clickedItem = 0;
        Description.text = "";
    }

    // return the name of the clicked item
    public string GetClickedItem(int item) {
        clickedItem = item;
        return Items[item].text;
    }

    // when exit from select hud, set the last clicked item as the current highlighted one
    public void SetLastClickedItem() {
        EventSystem.current.SetSelectedGameObject(Items[clickedItem].gameObject);
        clickedItem = -1;
    }

    // update which items are shown
    private void updateItems() {
        for (int i = 0; i < Items.Length; i++) {
            Text itemText = Items[i];
            Text itemAmount = ItemsAmount[i];
            itemText.gameObject.SetActive(true);
            itemAmount.gameObject.SetActive(true);
            Items item = GameManager.Instance.GetItemAt(i + itemIndx);
            if (item == null) {
                // if there is no item at this slot, disable it
                itemText.gameObject.SetActive(false);
                itemAmount.gameObject.SetActive(false);
                continue;
            }
            itemText.text = item.ItemName;
            itemAmount.text = "" + GameManager.Instance.GetAmountOfItem(item.ItemName);
        }
    }

    // update the description of the item the cursor is currently on
    private void updateDescription() {
        Items item = GameManager.Instance.GetItemDetails(currItem);
        if (item != null) {
            Description.text = item.Description;
        }
    }
}
