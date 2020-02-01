using System.Collections.Generic;
using UnityEngine;

public class Investigate : MonoBehaviour {
    public string EventName; // name of specific event for specific dialogue

    [Header("Dialogue")]
    public string[] preEventDialogue; // lines of generic dialogue
    public string[] postEventDialogue; // lines of dialogue for before/after specific event

    private bool canActivate = false;

    [Header("Item")]
    public Items Item;
    public int ItemAmount;
    public int PickupNumber;

    // Update is called once per frame
    void Update() {
        if (canActivate && GameManager.Instance.CanStartDialogue() && Input.GetButtonDown("Interact") && !Dialogue.Instance.dBox.activeSelf) {
            string[] dialogue = getDialogue();
            if (!haveInvestigated()) {
                // did not investigate yet
                EventManager.Instance.AddEvent(EventName);

                if (Item != null) {
                    if (Item.IsItem) {
                        // add item
                        GameManager.Instance.AddItem(Item.ItemName, ItemAmount);
                    } else if (Item.IsWeapon || Item.IsArmor) {
                        // add equipment
                        GameManager.Instance.AddEquipment(Item.ItemName, ItemAmount);
                    } else {
                        // add key item
                        GameManager.Instance.AddKeyItem(Item.ItemName);
                    }
                }
            }
            Dialogue.Instance.ShowDialogue(dialogue, false);
        }
    }

    /// <summary>
    /// Get the correct dialogue to show and append getting item if there's an item obtained
    /// </summary>
    /// <returns>Correct dialogue to show</returns>
    private string[] getDialogue() {
        List<string> dialogue;
        if (haveInvestigated()) {
            dialogue = new List<string>(this.postEventDialogue);
        } else {
            dialogue = new List<string>(this.preEventDialogue);
            if (Item != null) {
                // there's an item that is obtained
                dialogue.Add("You got " + ItemAmount + " " + Item.ItemName + "!");
            }
        }

        return dialogue.ToArray();
    }

    /// <summary>
    /// Return whether or not have been investigated
    /// </summary>
    /// <returns>True if have investigated, false otherwise</returns>
    private bool haveInvestigated() {
        bool obtainedItem = Item != null && ObtainItemManager.Instance.DidPickUpItem(PickupNumber);
        bool eventHappened = EventName.Length > 0 && EventManager.Instance.DidEventHappened(EventName);
        return obtainedItem || eventHappened;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = false;
        }
    }
}
