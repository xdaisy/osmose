using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigate : MonoBehaviour {
    public string Eventname; // name of specific event for specific dialogue

    [Header("Dialogue")]
    public string[] preEventDialogue; // lines of generic dialogue
    public string[] postEventDialogue; // lines of dialogue for before/after specific event

    private bool canActivate = false;

    [Header("Item")]
    public Items Item;
    public int ItemAmount;
    public int PickupNumber;

    private bool investigated;

    // Start is called before the first frame update
    void Start() {
        if (
            ObtainItemManager.Instance.DidPickUpItem(PickupNumber) || EventManager.Instance.DidEventHappened(Eventname)
            ) {
            investigated = true;
        } else {
            investigated = false;
        }
    }

    // Update is called once per frame
    void Update() {
        if (canActivate && GameManager.Instance.CanStartDialogue() && Input.GetButtonDown("Interact") && !Dialogue.Instance.dBox.activeSelf) {
            if (!investigated) {
                // did not investigate yet
                EventManager.Instance.AddEvent(Eventname);
                investigated = true;

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
            Dialogue.Instance.ShowDialogue(getDialogue(), false);
        }
    }

    /// <summary>
    /// Get the correct dialogue to show and append getting item if there's an item obtained
    /// </summary>
    /// <returns>Correct dialogue to show</returns>
    private string[] getDialogue() {
        List<string> dialogue;
        if (investigated) {
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
}
