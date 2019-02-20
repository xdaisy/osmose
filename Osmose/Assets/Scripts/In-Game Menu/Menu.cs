using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public static Menu Instance;
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

    [Header("Select Panel")]
    public SelectMenu SelectMenuUI;
    public GameObject SelectMenu;
    public CanvasGroup SelectPanel;
    public Button[] SelectCharacters;

    private bool equipWeapon;

    private string currCharacter;

    private string previousHud;
    private string currentHud;

    // for using items
    private string clickedItem;
    private bool usingItem;

    private bool usingSkill;

    // constants to keep track of hud names
    private const string MAIN = "Main";
    private const string ITEMS = "Items";
    private const string ITEM_TYPE = "ItemType";
    private const string ITEM_LIST = "ItemList";
    private const string ITEM_DESCRIPTION = "ItemDescriptionPanel";
    private const string SKILLS = "Skills";
    private const string EQUIPMENT = "Equipment";
    private const string EQUIPPED_PANEL = "CharacterEquipment";
    private const string EQUIPMENT_PANEL = "EquipmentPanel";
    private const string STATS = "Stats";
    private const string SELECT = "SelectMenu";

    private void Awake() {
        // don't destroy object on load if menu don't exist
        if (Instance == null) {
            Instance = this;
        } else {
            // if another menu exists, destroy game object
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        previousHud = MAIN;
        currentHud = MAIN;
        equipWeapon = true;
        currCharacter = "";
        clickedItem = "";
        usingItem = false;
        usingSkill = false;

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
                // exit item list panel
                ItemList.interactable = false;
                ItemType.interactable = true;
                currentHud = previousHud;
                previousHud = MAIN;
                ItemMenuUI.ExitItemList();
            }
            if (currentHud == ITEM_DESCRIPTION) {
                // exit item description panel
                DescriptionPanel.interactable = false;
                ItemList.interactable = true;

                currentHud = previousHud;
                previousHud = ITEM_TYPE;
                clickedItem = "";

                ItemMenuUI.ExitDescriptionPanel();
            }
            if (currentHud == EQUIPPED_PANEL) {
                // exit equipped panel
                currentHud = previousHud;
                previousHud = MAIN;
                EquippedPanel.interactable = false;
                EquipmentCharacters.interactable = true;

                Button[] characters = EquipmentCharacters.GetComponentsInChildren<Button>();
                for (int i = 0; i < characters.Length; i++) {
                    string charName = characters[i].GetComponentInChildren<Text>().text;
                    if (charName == currCharacter) {
                        EventSystem.current.SetSelectedGameObject(characters[i].gameObject);
                        break;
                    }
                }
            }
            if (currentHud == EQUIPMENT_PANEL) {
                // exit equipment panel
                currentHud = previousHud;
                previousHud = EQUIPMENT;

                EquipmentMenuUI.ExitEquipments();
                EquipmentPanel.interactable = false;
                EquippedPanel.interactable = true;

                Button[] buttons = EquippedPanel.GetComponentsInChildren<Button>();
                EventSystem.current.SetSelectedGameObject(null);
                if (equipWeapon) {
                    EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
                } else {
                    EventSystem.current.SetSelectedGameObject(buttons[1].gameObject);
                }
            }
            if (currentHud == SELECT) {
                // exit character select panel
                if (previousHud == ITEM_DESCRIPTION) {
                    // if was on item description
                    currentHud = ITEM_DESCRIPTION;
                    previousHud = ITEM_LIST;

                    DescriptionPanel.interactable = true;
                    usingItem = false;
                    ItemMenuUI.ExitSelectMenu();

                    SelectMenu.SetActive(false);
                    SelectPanel.interactable = false;
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
                if (currentHud == MAIN) {
                    EventSystem.current.SetSelectedGameObject(MainButtons[0]);
                } else {
                    foreach (GameObject mainButton in MainButtons) {
                        if (mainButton.name == currentHud) {
                            EventSystem.current.SetSelectedGameObject(mainButton);
                            break;
                        }
                    }
                }
                currentHud = MAIN;
                break;
            case 1:
                currentHud = ITEM_TYPE;
                EventSystem.current.SetSelectedGameObject(ItemFirstHighlighted);
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

    // close all the menus
    public void closeAllMenu() {
        foreach (GameObject menu in MenuHud) {
            menu.SetActive(false);
        }
    }

    // select which type of item to go look at
    public void ChooseWhichItem(int itemType) {
        ItemType.interactable = false;
        ItemList.interactable = true;
        EventSystem.current.SetSelectedGameObject(ItemList.GetComponentInChildren<Button>().gameObject);
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

    // open panel to either use or discard item
    public void OpenDescriptionPanel(int item) {
        ItemList.interactable = false;
        DescriptionPanel.interactable = true;

        previousHud = currentHud;
        currentHud = ITEM_DESCRIPTION;

        ItemMenuUI.OpenDescriptionPanel(item);
    }

    // go to character select panel
    public void UseItem() {
        clickedItem = ItemMenuUI.GetClickedItem();
        usingItem = true;

        previousHud = currentHud;
        currentHud = SELECT;

        // set description panel to not interactable
        DescriptionPanel.interactable = false;

        SelectMenu.SetActive(true);
        SelectPanel.interactable = true;
        EventSystem.current.SetSelectedGameObject(SelectCharacters[0].gameObject);
    }

    // choose which character to heal
    public void SelectCharacter(int indx) {
        string charName = PartyName[indx].text;
        
        if (usingItem) {
            // using item
            Items item = GameManager.Instance.GetItemDetails(clickedItem);

            if (item.AffectHP) {
                // heal hp
                int currHP = GameManager.Instance.Party.GetCharacterCurrentHP(charName);
                int maxHP = GameManager.Instance.Party.GetCharacterMaxHP(charName);

                if (currHP < maxHP) {
                    // if not at max hp, can heal hp
                    item.Use(charName);
                }
            }
            if (item.AffectSP) {
                // heal sp
                int currSp = GameManager.Instance.Party.GetCharacterCurrentSP(charName);
                int maxSp = GameManager.Instance.Party.GetCharacterMaxSP(charName);

                if (currSp < maxSp) {
                    // if not at max sp, can heal sp
                    item.Use(charName);
                }
            }

            int itemAmt = GameManager.Instance.GetAmountOfItem(item.ItemName);
            if (itemAmt == 0) {
                // if used last of this item, go back to item list
                SelectMenu.SetActive(false);
                SelectPanel.interactable = false;

                ItemList.interactable = true;
                ItemMenuUI.SetNewItem();

                previousHud = ITEM_TYPE;
                currentHud = ITEM_LIST;

                usingItem = false;
                clickedItem = "";
            }
            // else, stay on this panel
        }
        if (usingSkill) {
            // using skill
        }
    }

    public void Discard() {
        clickedItem = ItemMenuUI.GetClickedItem();
        GameManager.Instance.RemoveItem(clickedItem, 1);
        int itemAmt = GameManager.Instance.GetAmountOfItem(clickedItem);
        if (itemAmt == 0) {
            // if used last of this item, go back to item list
            DescriptionPanel.interactable = false;

            ItemList.interactable = true;
            ItemMenuUI.SetNewItem();

            previousHud = ITEM_TYPE;
            currentHud = ITEM_LIST;
            clickedItem = "";
        }
    }

    // open up equipped panel
    public void SelectWhichCharacterEqpmt(int character) {
        previousHud = currentHud;
        currentHud = EQUIPPED_PANEL;
        currCharacter = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;

        EquipmentCharacters.interactable = false;
        EquippedPanel.interactable = true;
        EventSystem.current.SetSelectedGameObject(EquipmentFirstEquipped);
        EquipmentMenuUI.ShowCharacterEquipment(character);
    }

    // open up the equipment panel
    public void SelectWhichEquipment(bool equipWeapon) {
        previousHud = currentHud;
        currentHud = EQUIPMENT_PANEL;
        this.equipWeapon = equipWeapon;
        EquippedPanel.interactable = false;
        EquipmentPanel.interactable = true;
        EquipmentMenuUI.ShowEquipments(equipWeapon);
    }

    // equip equipment
    public void Equip(int indx) {
        string eqmtName = EquipmentMenuUI.GetEquipment(indx);
        string charName = EquipmentMenuUI.GetCurrentCharacter();
        Items equipment = GameManager.Instance.GetEquipmentDetails(eqmtName);

        equipment.Use(charName);
        EquipmentMenuUI.UpdateAfterEquip();
    }

    // open the game menu
    public void OpenGameMenu() {
        OpenMenu(0);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MainButtons[0]);
        GameManager.Instance.GameMenuOpen = true;
    }

    // close the game menu
    public void CloseGameMenu() {
        closeAllMenu();
        previousHud = MAIN;
        currentHud = MAIN;
        equipWeapon = true;
        currCharacter = "";
        clickedItem = "";
        usingItem = false;
        usingSkill = false;
        GameManager.Instance.GameMenuOpen = false;
    }
}
