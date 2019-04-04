using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [Header("Generic")]
    public ShopActivator ShopKeep;
    public CanvasGroup[] ShopPanels;
    public Text MoneyOwned;
    public Text Description;

    [Header("Buy/Sell Panel")]
    public Button BuyButton;
    public Button SellButton;

    [Header("Item Types Panel")]
    public GameObject ItemType;
    public Button[] ItemTypes;

    [Header("Panel UI")]
    public ItemsPanel ItemsPanelUI;

    [Header("Shop's Items")]
    public Items[] ItemsForSell;

    private int currItemType;

    private const int BUY_SELL_PANEL = 0;
    private const int ITEM_TYPE_PANEL = 1;
    private const int ITEM_LIST_PANEL = 2;

    // Start is called before the first frame update
    void Start()
    {
        ItemsPanelUI.SetItemsList(ItemsForSell);
        MoneyOwned.text = "" + GameManager.Instance.Wallet;
        currItemType = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (ShopPanels[BUY_SELL_PANEL].interactable) {
                ExitShop();
            } else if (ShopPanels[ITEM_TYPE_PANEL].interactable) {
                ShopPanels[ITEM_TYPE_PANEL].interactable = false;
                ShopPanels[BUY_SELL_PANEL].interactable = true;
                EventSystem.current.SetSelectedGameObject(SellButton.gameObject);
                Description.text = "How may I help you?";
            } else if (ShopPanels[ITEM_LIST_PANEL].interactable) {
                if (ItemsPanelUI.GetIsBuying()) {
                    ShopPanels[ITEM_LIST_PANEL].interactable = false;
                    ShopPanels[BUY_SELL_PANEL].interactable = true;
                    EventSystem.current.SetSelectedGameObject(BuyButton.gameObject);
                    Description.text = "How may I help you?";
                } else {
                    ShopPanels[ITEM_LIST_PANEL].interactable = false;
                    ShopPanels[ITEM_TYPE_PANEL].interactable = true;
                    EventSystem.current.SetSelectedGameObject(ItemTypes[currItemType].gameObject);
                    Description.text = "What would you like to sell?";
                }
            }
        }
    }

    /// <summary>
    /// Open the shop menu
    /// </summary>
    public void OpenShopMenu() {
        GameManager.Instance.InShop = true;
        this.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(BuyButton.gameObject);

        Description.text = "Hi, how may I help you?";
    }

    /// <summary>
    /// Close the shop menu
    /// </summary>
    public void ExitShop() {
        GameManager.Instance.InShop = false;
        this.gameObject.SetActive(false);
        ShopKeep.CloseShop();
    }

    /// <summary>
    /// Set up buying in shop
    /// </summary>
    public void SelectBuy() {
        ShopPanels[BUY_SELL_PANEL].interactable = false;
        ShopPanels[ITEM_LIST_PANEL].interactable = true;
        ItemType.SetActive(false);
        ItemsPanelUI.SetIsBuying(true);
        ItemsPanelUI.SetItemsButton();
    }

    /// <summary>
    /// Set up selling in shop
    /// </summary>
    public void SelectSell() {
        ShopPanels[BUY_SELL_PANEL].interactable = false;
        ShopPanels[ITEM_TYPE_PANEL].interactable = true;
        Description.text = "What would you like to sell?";
        ItemType.SetActive(true);
        EventSystem.current.SetSelectedGameObject(ItemTypes[0].gameObject);
    }

    /// <summary>
    /// Choose whether or not selling Items or Equipment
    /// </summary>
    /// <param name="sellingItem"></param>
    public void SelectItemType(bool sellingItem) {
        ShopPanels[ITEM_TYPE_PANEL].interactable = false;
        ShopPanels[ITEM_LIST_PANEL].interactable = true;
        ItemsPanelUI.SetIsBuying(false);
        ItemsPanelUI.SetSellingItem(sellingItem);
        ItemsPanelUI.SetItemsButton();
        if (sellingItem) {
            currItemType = 1;
        } else {
            currItemType = 2;
        }
    }
}
