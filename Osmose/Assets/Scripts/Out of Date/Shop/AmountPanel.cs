using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class AmountPanel : MonoBehaviour {
    public Text Amount;
    public Text TotalAmount;
    public Text MoneyOwned;

    private int amount = 0;
    private Items currItem = null;
    private bool isBuying = true;

    private void Awake() {
        MoneyOwned.text = "" + GameManager.Instance.Wallet;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update() {
        if (this.gameObject.activeSelf) {
            // if this is visible
            if (Input.GetButtonDown("Vertical")) {
                float buttonInput = Input.GetAxisRaw("Vertical");

                if (buttonInput < -0.5f && amount > 0) {
                    // decrease amount
                    amount--;
                    updateAmount();
                }
                if (buttonInput > 0.5f) {
                    // increase amount
                    int moneyOwned = GameManager.Instance.Wallet;
                    int owned = 0;
                    if (currItem.IsItem) {
                        owned = GameManager.Instance.GetAmountOfItem(currItem.ItemName);
                    } else {
                        owned = GameManager.Instance.GetAmountOfEquipment(currItem.ItemName);
                    }
                    if (isBuying && moneyOwned >= (amount + 1) * currItem.Value && owned + amount + 1 <= 99) {
                        amount++;
                        updateAmount();
                    }
                    if (!isBuying && amount < owned) {
                        amount++;
                        updateAmount();
                    }
                }
            }
            if (Input.GetButtonDown("Horizontal")) {
                float buttonInput = Input.GetAxisRaw("Horizontal");

                if (buttonInput < -0.5f && amount > 0) {
                    // - 10
                    amount -= 10;
                    amount = Math.Max(amount, 0); // min = 0
                    updateAmount();
                }
                if (buttonInput > 0.5f) {
                    // + 10
                    int moneyOwned = GameManager.Instance.Wallet;
                    int owned = 0;
                    int cost = 0;

                    if (currItem.IsItem) {
                        owned = GameManager.Instance.GetAmountOfItem(currItem.ItemName);
                    } else {
                        owned = GameManager.Instance.GetAmountOfEquipment(currItem.ItemName);
                    }

                    int add = 0;

                    if (isBuying) {
                        // calculate if add 10 or add some other amount
                        add = 99 - (owned + amount) >= 10 ? 10 : 99 - (owned + amount);
                        add = Math.Max(add, 0); // make sure doesn't go below 0
                        cost = (amount + add) * currItem.Value;
                        while (cost > moneyOwned && add > 0) {
                            add--;
                            cost = (amount + add) * currItem.Value;
                        }
                    } else {
                        add = owned - amount >= 10 ? 10 : owned - amount;
                    }

                    if (add > 0) {
                        amount += add;
                        updateAmount();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Set up which Item and whether or not it is being bought
    /// </summary>
    /// <param name="item">Item that is being bought or sold</param>
    /// <param name="isBuying">Flag for whether or not player is buying</param>
    public void OpenAmountPanel(Items item, bool isBuying) {
        currItem = item;
        this.isBuying = isBuying;
        updateAmount();
        EventSystem.current.SetSelectedGameObject(Amount.gameObject);
    }

    /// <summary>
    /// Set variables to default when exiting Amount Panel
    /// </summary>
    public void CloseAmountPanel() {
        amount = 0;
        TotalAmount.text = "0";
        currItem = null;
        isBuying = true;
    }

    /// <summary>
    /// Buy or Sell the amount of selected item
    /// </summary>
    public void BuySell() {
        int totalAmount = Int32.Parse(TotalAmount.text);

        if (isBuying) {
            ShopLogic.BuyItem(currItem, amount, totalAmount);
        } else {
            ShopLogic.SellItem(currItem, amount, totalAmount);
        }
        amount = 0;

        updateAmount();
    }

    private void updateAmount() {
        Amount.text = "" + amount;
        if (isBuying) {
            TotalAmount.text = "" + (amount * currItem.Value);
        } else {
            TotalAmount.text = "" + (amount * Mathf.Max(Mathf.RoundToInt(currItem.Value * 0.5f), 1));
        }
        MoneyOwned.text = "" + GameManager.Instance.Wallet;
    }
}
