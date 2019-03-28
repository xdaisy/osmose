using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    public ShopActivator ShopKeep;
    public CanvasGroup[] ShopPanels;
    public Button BuyButton;

    private const int BUY_SELL_PANEL = 0;
    private const int ITEM_TYPE_PANEL = 1;
    private const int ITEM_LIST_PANEL = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
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
}
