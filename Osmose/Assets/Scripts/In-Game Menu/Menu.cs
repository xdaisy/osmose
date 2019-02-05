﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    private EventSystem eventSystem;
    public GameObject[] MenuHud;
    public GameObject[] MainButtons;

    [Header("Party stats")]
    public GameObject[] PartyStatHud;
    public Text[] PartyName;
    public Text[] PartyLevel;
    public Text[] PartyHP;
    public Text[] PartySP;
    public Slider[] PartyExpToNextLvl;
    public Text[] PartyEXP;

    [Header("Items Menu")]
    public ItemMenu ItemMenuUI;
    public GameObject ItemFirstHighlighted;
    public CanvasGroup ItemType;
    public CanvasGroup ItemList;
    public CanvasGroup DescriptionPanel;

    [Header("Stats Menu")]
    public StatsMenu StatsMenuUI;

    [Header("Equipment Menu")]
    public EquipmentMenu EquipmentMenuUI;
    public CanvasGroup EquipmentCharacters;
    public CanvasGroup EquippedPanel; // show current equipment
    public GameObject EquipmentFirstEquipped; // first highlighted button when go to equipped panel
    public CanvasGroup EquipmentPanel; // show equipments in inventory

    private bool equipWeapon ;

    private string currCharacter;

    private string previousHud;
    private string currentHud;

    // constants to keep track of hud names
    private const string MAIN = "Main";
    private const string ITEMS = "Items";
    private const string ITEM_TYPE = "ItemType";
    private const string ITEM_LIST = "ItemList";
    private const string SKILLS = "Skills";
    private const string EQUIPMENT = "Equipment";
    private const string CHARACTER_EQUIPMENT = "CharacterEquipment";
    private const string EQUIPMENT_PANEL = "EquipmentPanel";
    private const string STATS = "Stats";

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameMenuOpen = true;
        previousHud = MAIN;
        currentHud = MAIN;
        equipWeapon = true;
        currCharacter = "";

        eventSystem = EventSystem.current;
        updatePartyStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (previousHud == MAIN && currentHud != MAIN) {
                // go back to main menu
                if (currentHud == ITEM_TYPE) {
                    currentHud = ITEMS;
                }

                OpenMenu(0);
            }
            if (currentHud == ITEM_LIST) {
                ItemList.interactable = false;
                ItemType.interactable = true;
                currentHud = previousHud;
                previousHud = MAIN;
                ItemMenuUI.ExitItemList();
            }
            if (currentHud == CHARACTER_EQUIPMENT) {
                currentHud = previousHud;
                previousHud = MAIN;
                EquippedPanel.interactable = false;
                EquipmentCharacters.interactable = true;

                Button[] characters = EquipmentCharacters.GetComponentsInChildren<Button>();
                for (int i = 0; i < characters.Length; i++) {
                    string charName = characters[i].GetComponentInChildren<Text>().text;
                    if (charName == currCharacter) {
                        eventSystem.SetSelectedGameObject(characters[i].gameObject);
                        break;
                    }
                }
            }
            if (currentHud == EQUIPMENT_PANEL) {
                currentHud = previousHud;
                previousHud = EQUIPMENT;
                EquipmentPanel.interactable = false;
                EquippedPanel.interactable = true;

                Button[] buttons = EquippedPanel.GetComponentsInChildren<Button>();
                if (equipWeapon) {
                    eventSystem.SetSelectedGameObject(buttons[0].gameObject);
                } else {
                    eventSystem.SetSelectedGameObject(buttons[1].gameObject);
                }
            }
        }
    }

    private void updatePartyStats() {
        List<string> currentParty = GameManager.Instance.Party.GetCurrentParty();

        for (int i = 0; i < PartyStatHud.Length; i++) {
            if (i >= currentParty.Count) {
                PartyStatHud[i].SetActive(false); // if don't have all the party members, hide some of the hud
                continue;
            }

            string partyMember = currentParty[i];
            PartyName[i].text = partyMember;
            PartyLevel[i].text = "Level: " + GameManager.Instance.Party.GetCharacterLevel(partyMember);
            PartyHP[i].text = "HP: " + GameManager.Instance.Party.GetCharacterCurrentHP(partyMember) + "/" + GameManager.Instance.Party.GetCharacterMaxHP(partyMember);
            PartySP[i].text = "SP: " + GameManager.Instance.Party.GetCharacterCurrentSP(partyMember) + "/" + GameManager.Instance.Party.GetCharacterMaxSP(partyMember);

            int currExp = GameManager.Instance.Party.GetCharacterCurrentEXP(partyMember);
            int expToNextLvl = GameManager.Instance.Party.GetCharacterEXPtoNextLvl(partyMember);

            PartyExpToNextLvl[i].value = ((float) currExp) / expToNextLvl;
            PartyEXP[i].text = "" + (expToNextLvl - currExp);
        }
    }

    public void OpenMenu(int menu) {
        previousHud = currentHud;
        closeAllMenu();

        MenuHud[menu].SetActive(true);
        switch(menu) {
            case 0:
                foreach(GameObject mainButton in MainButtons) {
                    if (mainButton.name == currentHud) {
                        eventSystem.SetSelectedGameObject(mainButton);
                        break;
                    }
                }
                currentHud = MAIN;
                break;
            case 1:
                currentHud = ITEM_TYPE;
                eventSystem.SetSelectedGameObject(ItemFirstHighlighted);
                break;
            case 2:
                currentHud = SKILLS;
                break;
            case 3:
                currentHud = EQUIPMENT;
                EquipmentMenuUI.OpenEquipmentMenu();
                break;
            case 4:
                currentHud = STATS;
                StatsMenuUI.OpenStatsMenu();
                break;
        }
    }

    public void closeAllMenu() {
        foreach (GameObject menu in MenuHud) {
            menu.SetActive(false);
        }
    }

    public void ChooseWhichItem(int itemType) {
        ItemType.interactable = false;
        ItemList.interactable = true;
        eventSystem.SetSelectedGameObject(ItemList.GetComponentInChildren<Button>().gameObject);
        previousHud = currentHud;
        currentHud = ITEM_LIST;
        switch (itemType) {
            case 0:
                // items
                ItemMenuUI.SetItemType(itemType);
                break;
            case 1:
                // equipments
                ItemMenuUI.SetItemType(itemType);
                break;
            case 2:
                // key
                ItemMenuUI.SetItemType(itemType);
                break;
        }
    }

    public void SelectWhichCharacterEqpmt(int character) {
        previousHud = currentHud;
        currentHud = CHARACTER_EQUIPMENT;
        currCharacter = eventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text;

        eventSystem.SetSelectedGameObject(EquipmentFirstEquipped);
        EquipmentCharacters.interactable = false;
        EquippedPanel.interactable = true;
        EquipmentMenuUI.ShowCharacterEquipment(character);
    }

    public void SelectWhichEquipment(bool equipWeapon) {
        previousHud = currentHud;
        currentHud = EQUIPMENT_PANEL;
        this.equipWeapon = equipWeapon;
        EquippedPanel.interactable = false;
        EquipmentPanel.interactable = true;
        EquipmentMenuUI.ShowEquipments(equipWeapon);
    }
}
