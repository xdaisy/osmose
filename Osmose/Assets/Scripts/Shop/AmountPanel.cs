using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmountPanel : MonoBehaviour {
    public Text Amount;
    public Text TotalAmount;

    private int amount = 0;
    private Items currItem = null;
    private bool isBuying = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
                    if (isBuying && moneyOwned >= (amount + 1) * currItem.Value) {
                        amount++;
                        updateAmount();
                    }
                    if (!isBuying && currItem.IsItem && GameManager.Instance.GetAmountOfItem(currItem.ItemName) > amount) {
                        amount++;
                        updateAmount();
                    }
                    if (!isBuying && (currItem.IsWeapon || currItem.IsArmor) && GameManager.Instance.GetAmountOfEquipment(currItem.ItemName) > amount) {
                        amount++;
                        updateAmount();
                    }
                }
            }
        }
    }

    public void OpenAmountPanel(Items item, bool isBuying) {
        currItem = item;
        this.isBuying = isBuying;
        updateAmount();
    }

    public void CloseAmountPanel() {
        amount = 0;
        TotalAmount.text = "0";
        currItem = null;
        isBuying = true;
    }

    private void updateAmount() {
        Amount.text = "" + amount;
        if (isBuying) {
            TotalAmount.text = "" + (amount * currItem.Value);
        } else {
            TotalAmount.text = "" + (amount * Mathf.Max(Mathf.RoundToInt(currItem.Value * 0.5f), 1));
        }
    }
}
