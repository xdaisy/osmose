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

    private void Awake() {
        itemsList = null;
        currItem = "";
        itemIndx = 0;
        isBuying = true;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SetItemsButton() {
        EventSystem.current.SetSelectedGameObject(ItemsButton[0].gameObject);
        updateItemsList();
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
            } else {
                // get item from inventory
                item = GameManager.Instance.GetItemAt(i + itemIndx);
                if (item == null) {
                    // if there aren't enough items in inventory to fill list, set text inactive
                    ItemsButton[i].gameObject.SetActive(false);
                    ItemsCost[i].gameObject.SetActive(false);
                    ItemsOwned[i].gameObject.SetActive(false);
                    continue;
                }
            }
            ItemsName[i].gameObject.SetActive(true);
            ItemsCost[i].gameObject.SetActive(true);
            ItemsOwned[i].gameObject.SetActive(true);

            ItemsName[i].text = item.ItemName;
            if (isBuying) {
                ItemsCost[i].text = "" + item.Value;
            } else {
                ItemsCost[i].text = "" + Mathf.RoundToInt(item.Value * 0.5f);
            }
            ItemsOwned[i].text = "" + GameManager.Instance.GetAmountOfItem(item.ItemName);
        }
    }
}
