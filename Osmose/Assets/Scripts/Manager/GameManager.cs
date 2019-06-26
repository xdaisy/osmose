using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Party")]
    public PartyStats Party;
    public Sprite ArenSprite;
    public Sprite ReySprite;
    public Sprite NaoiseSprite;

    [Header("Game Status")]
    public bool GameMenuOpen, DialogActive, FadingBetweenAreas, InBattle, InCutscene, InShop;
    public string CurrentScene;
    public string LastTown; // last town/non-battle area player was last in
    public bool IsBattleMap;

    [Header("Currency")]
    public int Wallet;

    [Header("Items Held")]
    public List<string> ItemsHeld; // keep track of which usable item the player has
    public List<int> NumOfItems; // keep track of how many of each item is held

    [Header("Equipment Held")]
    public List<string> EquipmentHeld; // keep track of which equipment the player has
    public List<int> NumOfEquipment; // keep track of how many of each equipment is held

    [Header("Key Items Held")]
    public List<string> KeyItemsHeld; // keep track of which key item the player has, can only have one of each key item

    [Header("References")]
    public Items[] ReferenceItems; // reference to prefab of the items
    public Items[] ReferenceEquipment; // refernce to prefab of equipment
    public Items[] ReferenceKeyItems; // reference to prefab of key items

    private float magicMeter = 1f;
    private float playTime = 0f;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        Party = new PartyStats();
    }

    // Update is called once per frame
    void Update() {
        if (GameMenuOpen || DialogActive || FadingBetweenAreas || InBattle || InCutscene || InShop) {
            PlayerControls.Instance.SetCanMove(false);
        } else {
            PlayerControls.Instance.SetCanMove(true);
        }
    }

    // return if can open the menu
    public bool CanOpenMenu() {
        return !DialogActive && !FadingBetweenAreas && !InBattle && !InCutscene && !InShop;
    }

    // return if can open shop menu
    public bool CanOpenShop() {
        return !DialogActive && !FadingBetweenAreas && !InBattle && !InCutscene && !GameMenuOpen;
    }

    public bool CanStartDialogue() {
        return !InShop && !FadingBetweenAreas && !InBattle && !InCutscene && !GameMenuOpen;
    }

    // get money
    public void GainMoney(int amount) {
        Wallet += amount;
        Wallet = Math.Min(Wallet, 999999); // can't go above 999999
    }

    // lose money
    public void UseMoney(int amount) {
        Wallet -= amount;
        Wallet = Math.Max(Wallet, 0); // can't go below 0
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

    public void AddItem(string itemName, int amount) {
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
            if (ItemsHeld.Contains(itemName)) {
                // if have item
                for (int i = 0; i < ItemsHeld.Count; i++) {
                    if (ItemsHeld[i] == itemName) {
                        NumOfItems[i] += amount; // increment number of itemName held
                        break;
                    }
                }
            } else {
                // if don't have, add it
                ItemsHeld.Add(itemName);
                NumOfItems.Add(amount);
            }
        } else {
            Debug.LogError(itemName + " do not exist"); // log error
        }
    }

    public void RemoveItem(string itemName, int amount) {
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
                    NumOfItems[i] -= amount; // decrement number of itemName held, if added should be 0
                    if (NumOfItems[i] <= 0) {
                        // remove the item if carry 0 of them
                        ItemsHeld.RemoveAt(i);
                        NumOfItems.RemoveAt(i);
                    }
                    break;
                }
            }
        } else {
            Debug.LogError(itemName + " do not exist"); // log error
        }
    }

    // return the number of items in inventory
    public int GetNumItems() {
        return ItemsHeld.Count;
    }

    // Equipment Methods

    public Items GetEquipmentAt(int index) {
        if (index >= EquipmentHeld.Count) {
            return null;
        }
        return GetEquipmentDetails(EquipmentHeld[index]);
    }

    public Items GetNthEquipment(int n, bool isWeapon, string charName) {
        int count = 0;
        for (int i = 0; i < EquipmentHeld.Count; i++) {
            Items item = GetEquipmentAt(i);
            if (isWeapon && item.IsWeapon) {
                // if look for weapon and found weapon, increment count
                count = (charName == "Aren" && item.ArenEquipment) || (charName == "Rey" && item.ReyEquipment) || (charName == "Naoise" && item.NaoiseEquipment) ? count + 1 : count;
            } else if (!isWeapon && item.IsArmor) {
                // if look for armor and found armor, increment count
                count++;
            }

            if (count == n) {
                return item;
            }
        }
        return null;
    }

    public int GetAmountOfEquipment(string equipmentName) {
        for (int i = 0; i < EquipmentHeld.Count; i++) {
            if (EquipmentHeld[i] == equipmentName) {
                return NumOfEquipment[i];
            }
        }
        return 0;
    }

    public void AddEquipment(string equipmentName, int amount) {
        // checks if item exists
        bool itemExists = false;
        foreach (Items item in ReferenceEquipment) {
            if (item.ItemName == equipmentName) {
                itemExists = true;
                break;
            }
        }

        if (itemExists) {
            // add item if the item is a real item
            if (EquipmentHeld.Contains(equipmentName)) {
                // if have equipment, find it and increment count
                for (int i = 0; i < EquipmentHeld.Count; i++) {
                    if (EquipmentHeld[i] == equipmentName) {
                        NumOfEquipment[i] += amount; // increment number of itemName held, if added should be 0
                        break;
                    }
                }
            } else {
                EquipmentHeld.Add(equipmentName);
                NumOfEquipment.Add(amount);
            }
            
        } else {
            Debug.LogError(equipmentName + " do not exist"); // log error
        }
    }

    public void RemoveEquipment(string equipmentName, int amount) {
        // checks if item exists
        bool itemExists = false;
        foreach (Items item in ReferenceEquipment) {
            if (item.ItemName == equipmentName) {
                itemExists = true;
                break;
            }
        }

        if (itemExists) {
            // add item if the item is a real item
            if (EquipmentHeld.Contains(equipmentName)) {
                // get rid of if item has it
                for (int i = 0; i < EquipmentHeld.Count; i++) {
                    if (EquipmentHeld[i] == equipmentName) {
                        NumOfEquipment[i] -= amount; // decrement number of itemName held, if added should be 0
                        if (NumOfEquipment[i] <= 0) {
                            // remove the item if carry 0 of them
                            EquipmentHeld.RemoveAt(i);
                            NumOfEquipment.RemoveAt(i);
                        }
                        break;
                    }
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

    // retun the number of equipment in inventory
    public int GetNumEquipment() {
        return EquipmentHeld.Count;
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
        foreach (Items item in ReferenceKeyItems) {
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

    /// <summary>
    /// Get the character's sprite
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Sprite of the character, null if the character is not one of the three main characters</returns>
    public Sprite GetCharSprite(string name) {
        if (name == Constants.AREN) {
            return this.ArenSprite;
        } else if (name == Constants.REY) {
            return this.ReySprite;
        } else if (name == Constants.NAOISE) {
            return this.NaoiseSprite;
        }
        return null;
    }

    /// <summary>
    /// Get the magic meter
    /// </summary>
    /// <returns>Magic meter</returns>
    public float GetMagicMeter() {
        return magicMeter;
    }

    /// <summary>
    /// Set the magic meter
    /// </summary>
    /// <param name="magic">Current amount of the magic meter</param>
    public void SetMagicMeter(float magic) {
        magicMeter = magic;
        // cannot be under 0f
        magicMeter = Mathf.Max(magicMeter, 0f);
        // cannot be over 1f
        magicMeter = Mathf.Min(magicMeter, 1f);
    }

    /// <summary>
    /// Get the total play time
    /// </summary>
    /// <returns>The total play time</returns>
    public float GetPlayTime() {
        return playTime;
    }

    /// <summary>
    /// Set the play time when loading save
    /// </summary>
    /// <param name="time">Play time of the save file</param>
    public void SetPlayTIme(float time) {
        this.playTime = time;
    }
}
