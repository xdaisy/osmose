using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PartyStats Party;

    public bool GameMenuOpen, DialogActive, FadingBetweenAreas, InBattle;

    public string CurrentScene;

    [Header("Items Held")]
    public List<string> ItemsHeld; // keep track of which usable item the player has
    public List<int> NumOfItems; // keep track of how many of each item is held

    [Header("Equipment Held")]
    public List<string> EquipmentHeld; // keep track of which equipment the player has
    public int[] NumOfEquipment; // keep track of how many of each equipment is held

    [Header("Key Items Held")]
    public List<string> KeyItemsHeld; // keep track of which key item the player has, can only have one of each key item

    [Header("References")]
    public Items[] ReferenceItems; // reference to prefab of the items
    public Items[] ReferenceEquipment; // refernce to prefab of equipment
    public Items[] ReferenceKeyItems; // reference to prefab of key items

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
        if (GameMenuOpen || DialogActive || FadingBetweenAreas || InBattle) {
            PlayerControls.Instance.SetCanMove(false);
        } else {
            PlayerControls.Instance.SetCanMove(true);
        }
    }

    public Items GetItemAt(int index) {
        if (index >= ItemsHeld.Count) {
            return null;
        }
        return GetItemDetails(ItemsHeld[index]);
    }

    public int GetAmountOfItem(string itemName) {
        for (int i = 0; i < ItemsHeld.Count; i++) {
            if (ItemsHeld[i] == "") {
                break;
            }

            if (ItemsHeld[i] == itemName) {
                return NumOfItems[i];
            }
        }
        return 0;
    }

    /// <summary>
    /// Get the Items detail for the itemName
    /// </summary>
    /// <param name="itemName">Name of the item</param>
    /// <returns>Items of the item</returns>
    public Items GetItemDetails(string itemName) {
        foreach(Items item in ReferenceItems) {
            if (item.ItemName == itemName) {
                return item;
            }
        }

        return null;
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
            for (int i = 0; i < ItemsHeld.Count; i++) {
                if (ItemsHeld[i] == itemName || ItemsHeld[i] == "") {
                    ItemsHeld[i] = itemName; // set item in error
                    NumOfItems[i]++; // increment number of itemName held, if added should be 0
                    break;
                }
            }
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
            for (int i = 0; i < ItemsHeld.Count; i++) {
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
        } else {
            Debug.LogError(itemName + " do not exist"); // log error
        }
    }

    // Equipment Methods

    public Items GetEquipmentAt(int index) {
        if (index >= EquipmentHeld.Count) {
            return null;
        }
        return GetEquipmentDetails(EquipmentHeld[index]);
    }

    public int GetAmountOfEquipment(string equipmentName) {
        for (int i = 0; i < EquipmentHeld.Count; i++) {
            if (EquipmentHeld[i] == "") {
                break;
            }

            if (EquipmentHeld[i] == equipmentName) {
                return NumOfEquipment[i];
            }
        }
        return 0;
    }

    public void AddEquipment(string equipmentName) {
        // checks if item exists
        bool itemExists = false;
        foreach (Items item in ReferenceItems) {
            if (item.ItemName == equipmentName) {
                itemExists = true;
                break;
            }
        }

        if (itemExists) {
            // add item if the item is a real item
            for (int i = 0; i < EquipmentHeld.Count; i++) {
                if (EquipmentHeld[i] == equipmentName || EquipmentHeld[i] == "") {
                    EquipmentHeld[i] = equipmentName; // set item in error
                    NumOfEquipment[i]++; // increment number of itemName held, if added should be 0
                    break;
                }
            }
        } else {
            Debug.LogError(equipmentName + " do not exist"); // log error
        }
    }

    public void RemoveEquipment(string equipmentName) {
        // checks if item exists
        bool itemExists = false;
        foreach (Items item in ReferenceItems) {
            if (item.ItemName == equipmentName) {
                itemExists = true;
                break;
            }
        }

        if (itemExists) {
            // add item if the item is a real item
            for (int i = 0; i < EquipmentHeld.Count; i++) {
                if (EquipmentHeld[i] == equipmentName) {
                    NumOfEquipment[i]--; // decrement number of itemName held, if added should be 0
                    if (NumOfEquipment[i] <= 0) {
                        // remove the item if carry 0 of them
                        EquipmentHeld[i] = "";
                    }
                    break;
                } else if (EquipmentHeld[i] == "") {
                    // if didn't find it and got to end of array, break
                    break;
                }
            }
        } else {
            Debug.LogError(equipmentName + " do not exist"); // log error
        }
    }

    /// <summary>
    /// Get the Items detail for the equipmentName
    /// </summary>
    /// <param name="equipmentName">Name of the equipment</param>
    /// <returns>Items of the equipments</returns>
    public Items GetEquipmentDetails(string equipmentName) {
        foreach (Items item in ReferenceEquipment) {
            if (item.ItemName == equipmentName) {
                return item;
            }
        }

        return null;
    }

    // Key Items Methods

    public Items GetKeyItemAt(int index) {
        if (index >= KeyItemsHeld.Count) {
            return null;
        }
        return GetKeyItemDetails(KeyItemsHeld[index]);
    }

    public void AddKeyItem(string keyItemName) {
        // checks if item exists
        bool itemExists = false;
        foreach (Items item in ReferenceItems) {
            if (item.ItemName == keyItemName) {
                itemExists = true;
                break;
            }
        }

        if (itemExists) {
            // add item if the item is a real item
            KeyItemsHeld.Add(keyItemName);
        } else {
            Debug.LogError(keyItemName + " do not exist"); // log error
        }
    }

    /// <summary>
    /// Get the Items detail for the keyItemName
    /// </summary>
    /// <param name="keyItemName">Name of the key item</param>
    /// <returns>Items of the key item</returns>
    public Items GetKeyItemDetails(string keyItemName) {
        foreach (Items item in ReferenceKeyItems) {
            if (item.ItemName == keyItemName) {
                return item;
            }
        }

        return null;
    }
}
