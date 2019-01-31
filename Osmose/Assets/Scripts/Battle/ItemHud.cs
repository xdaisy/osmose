using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemHud : MonoBehaviour
{
    private EventSystem eventSystem;

    public CanvasGroup Hud;

    // For showing Item HUD
    [Header("Item HUD")]
    public Button[] ItemButtons;
    public Text ItemName;
    public Text ItemDescription;
    public Text ItemAmount;

    private Button currentItem;
    private int itemIndx = 0; // keeps track of the index of the items array we're looking at in Game Manager, use to know what page the items hud will be on

    private void Awake() {
        eventSystem = EventSystem.current;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Hud.interactable) {
            // if hud is interactable
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                // only update if item hud is active and the item list is interactable
                if (currentItem.name == eventSystem.currentSelectedGameObject.name) {
                    // if was at the is on the last item, scroll down
                    int i = itemIndx + ItemButtons.Length;
                    Items item = GameManager.Instance.GetItemAt(itemIndx + ItemButtons.Length);
                    if (item != null) {
                        // more items, so scroll right
                        itemIndx += 2;
                        showItems();
                    }
                }
            }
        }
    }

    private void showItems() {
        // update which item is being shown
        for (int i = 0; i < ItemButtons.Length; i++) {
            Text itemText = ItemButtons[i].GetComponentInChildren<Text>();
            itemText.GetComponent<Button>().interactable = true;
            Items item = GameManager.Instance.GetItemAt(i + itemIndx);
            if (item == null) {
                // if there is no item at this slot, disable it
                ItemButtons[i].gameObject.SetActive(false);
                continue;
            }
            itemText.text = item.ItemName;
        }
    }
}
