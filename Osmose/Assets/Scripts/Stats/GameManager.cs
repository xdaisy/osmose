using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PartyStats Party;

    public bool GameMenuOpen, DialogActive, FadingBetweenAreas;

    public string[] ItemsHeld; // keep track of which usable item the player has
    public int[] NumOfItems; // keep track of how many of each item held
    public Items[] ReferenceItems; // reference to prefab of the items

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            Party = new PartyStats();
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMenuOpen || DialogActive || FadingBetweenAreas) {
            PlayerControls.Instance.SetCanMove(false);
        } else {
            PlayerControls.Instance.SetCanMove(true);
        }
    }

    public Items GetItemAt(int index) {
        if (index >= ItemsHeld.Length) {
            return null;
        }
        return GetItemDetails(ItemsHeld[index]);
    }

    public int GetAmountOfItem(string itemName) {
        for (int i = 0; i < ItemsHeld.Length; i++) {
            if (ItemsHeld[i] == "") {
                break;
            }

            if (ItemsHeld[i] == itemName) {
                return NumOfItems[i];
            }
        }
        return 0;
    }

    public Items GetItemDetails(string itemName) {
        foreach(Items item in ReferenceItems) {
            if (item.ItemName == itemName) {
                return item;
            }
        }

        return null;
    }

    /// <summary>
    /// Move the indexes with items to the front of the array
    /// </summary>
    public void SortItems() {
        bool itemAfterSpace = true;

        while (itemAfterSpace) {
            itemAfterSpace = false;
            for (int i = 0; i < ItemsHeld.Length - 1; i++) {
                if (ItemsHeld[i] == "") {
                    ItemsHeld[i] = ItemsHeld[i + 1];
                    ItemsHeld[i + 1] = "";

                    NumOfItems[i] = NumOfItems[i + 1];
                    NumOfItems[i + 1] = 0;

                    if (ItemsHeld[i] != "") {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemName) {
        // checks if item exists
        bool itemExists = false;
        foreach (Items item in ReferenceItems) {
            if (item.ItemName == itemName) {
                itemExists = true;
                break;
            }
        }

        if (itemExists) {
            // add item if the item is a real item
            for (int i = 0; i < ItemsHeld.Length; i++) {
                if (ItemsHeld[i] == itemName || ItemsHeld[i] == "") {
                    ItemsHeld[i] = itemName; // set item in error
                    NumOfItems[i]++; // increment number of itemName held, if added should be 0
                    break;
                }
            }

            SortItems();
        } else {
            Debug.LogError(itemName + " do not exist"); // log error
        }
    }

    public void RemoveItem(string itemName) {
        // checks if item exists
        bool itemExists = false;
        foreach (Items item in ReferenceItems) {
            if (item.ItemName == itemName) {
                itemExists = true;
                break;
            }
        }

        if (itemExists) {
            // add item if the item is a real item
            for (int i = 0; i < ItemsHeld.Length; i++) {
                if (ItemsHeld[i] == itemName) {
                    NumOfItems[i]--; // decrement number of itemName held, if added should be 0
                    if (NumOfItems[i] <= 0) {
                        // remove the item if carry 0 of them
                        ItemsHeld[i] = "";
                    }
                    break;
                } else if (ItemsHeld[i] == "") {
                    // if didn't find it and got to end of array, break
                    break;
                }
            }
            SortItems();
        } else {
            Debug.LogError(itemName + " do not exist"); // log error
        }
    }
}
