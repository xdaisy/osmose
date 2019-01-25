using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMenu : MonoBehaviour {
    private EventSystem eventSystem;

    public CanvasGroup ItemList;

    public Text[] Items;

    public Image ItemImage;
    public Text ItemAmount;
    public Text Description;

    private Text currentItem;
    private int itemIndx;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        currentItem = Items[0]; // set to first item
        itemIndx = 0;
        showItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && ItemList.gameObject.activeSelf && ItemList.interactable) {
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
        if (Input.GetKeyDown(KeyCode.UpArrow) && ItemList.gameObject.activeSelf && ItemList.interactable) {
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
        currentItem = eventSystem.currentSelectedGameObject.GetComponent<Text>();
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
}
