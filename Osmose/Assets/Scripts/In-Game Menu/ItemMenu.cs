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
    }

    private void showItems() {
        // update which item is being shown
        for (int i = 0; i < Items.Length; i++) {
            Items item = GameManager.Instance.GetItemAt(i + itemIndx);
            if (item == null) {
                Items[i].gameObject.SetActive(false);
                continue;
            }
            Items[i].text = item.ItemName;
        }
    }
}
