using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [Header("UI References")]
    public ShopActivator ShopKeep;
    public CanvasGroup[] ShopPanels;
    public Button BuyButton;
    public Button SellButton;
    public Text MoneyOwned;

    [Header("Panel UI")]
    public ItemsPanel ItemsPanelUI;

    [Header("Shop's Items")]
    public Items[] ItemsForSell;

    private const int BUY_SELL_PANEL = 0;
    private const int ITEM_TYPE_PANEL = 1;
    private const int ITEM_LIST_PANEL = 2;

    // Start is called before the first frame update
    void Start()
    {
        ItemsPanelUI.SetItemsList(ItemsForSell);
        MoneyOwned.text = "" + GameManager.Instance.Wallet;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (ShopPanels[BUY_SELL_PANEL].interactable) {
                ExitShop();
            } else if (ShopPanels[ITEM_TYPE_PANEL].interactable) {

            } else if (ShopPanels[ITEM_LIST_PANEL].interactable) {
                ShopPanels[ITEM_LIST_PANEL].interactable = false;
                ShopPanels[BUY_SELL_PANEL].interactable = true;
                if (ItemsPanelUI.GetIsBuying()) {
                    EventSystem.current.SetSelectedGameObject(BuyButton.gameObject);
                } else {
                    EventSystem.current.SetSelectedGameObject(SellButton.gameObject);
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
    /// Set up buying/selling in shop
    /// </summary>
    public void SelectShopCommand(bool isBuying) {
        ShopPanels[BUY_SELL_PANEL].interactable = false;
        ShopPanels[ITEM_LIST_PANEL].interactable = true;
        ItemsPanelUI.SetIsBuying(isBuying);
        ItemsPanelUI.SetItemsButton();
    }
}
