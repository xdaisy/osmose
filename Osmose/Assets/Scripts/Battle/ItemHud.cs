using System.Collections;
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

    // Start is called before the first frame update
    void Start() {
        currItem = "";
        itemIndx = 0;
    }

    // Update is called once per frame
    void Update() {
        if (Hud.interactable) {
            Text selectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<Text>();
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
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

            if (Input.GetKeyDown(KeyCode.UpArrow) && itemIndx > 0) {
                // if scrolling up and not at beginning of items list
                itemIndx--;
                updateItems();
            }

            // update the current selected item
            currItem = selectedItem.text;
            updateDescription();
        }
    }

    private void updateItems() {
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

    private void updateDescription() {
        Items item = GameManager.Instance.GetItemDetails(currItem);
        Description.text = item.Description;
    }
}
