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
    public CanvasGroup MainButtonsHud;

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

    [Header("Skills Menu")]
    public SkillMenu SkillMenuUI;
    public CanvasGroup SkillsPanel;

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

    [Header("Save Menu")]
    public SaveMenu SaveMenuUI;

    private bool equipWeapon;

    private string currCharacter;

    private string previousHud;
    private string currentHud;

    // for switching characters
    private int charToSwitch;

    // for using items
    private string itemToUse;
    private bool usingItem;

    // for using skills
    private Skill skillToUse;
    private bool usingSkill;

    // constants to keep track of hud names
    private const string MAIN = "Main";
    private const string PARTY = "PartyHud";
    private const string ITEMS = "Items";
    private const string ITEM_TYPE = "ItemType";
    private const string ITEM_LIST = "ItemList";
    private const string ITEM_DESCRIPTION = "ItemDescriptionPanel";
    private const string SKILLS = "Skills";
    private const string CHAR_SELECT_PANEL = "CharacterSelectPanel";
    private const string SKILLS_LIST = "SkillsList";
    private const string EQUIPMENT = "Equipment";
    private const string EQUIPPED_PANEL = "CharacterEquipment";
    private const string EQUIPMENT_PANEL = "EquipmentPanel";
    private const string STATS = "Stats";
    private const string SELECT = "SelectMenu";
    private const string SAVE = "Save";

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
        charToSwitch = -1;
        equipWeapon = true;
        currCharacter = "";
        itemToUse = "";
        usingItem = false;
        skillToUse = null;
        usingSkill = false;

        updatePartyStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (previousHud == MAIN && currentHud != MAIN && currentHud != PARTY) {
                // go back to main menu
                if (currentHud == ITEM_TYPE) {
                    currentHud = ITEMS;
                }
                if (currentHud == CHAR_SELECT_PANEL) {
                    currentHud = SKILLS;
                    SkillMenuUI.CloseSkillMenu();
                }

                OpenMenu(0);
            }
            if (currentHud == PARTY) {
                currentHud = previousHud;
                MainButtonsHud.interactable = true;
                charToSwitch = -1;
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
                itemToUse = "";

                ItemMenuUI.ExitDescriptionPanel();
            }
            if (currentHud == SKILLS_LIST) {
                // exit skills panel
                currentHud = previousHud;
                previousHud = MAIN;

                SkillMenuUI.CloseSkillsPanel();
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
                    currentHud = previousHud;
                    previousHud = ITEM_LIST;

                    usingItem = false;
                    itemToUse = "";

                    DescriptionPanel.interactable = true;
                    ItemMenuUI.ExitSelectMenu();

                    SelectMenu.SetActive(false);
                    SelectPanel.interactable = false;
                }

                if (previousHud == SKILLS_LIST) {
                    // if was on skills list
                    currentHud = previousHud;
                    previousHud = CHAR_SELECT_PANEL;

                    usingSkill = false;
                    skillToUse = null;

                    SkillsPanel.interactable = true;
                    usingSkill = false;
                    SkillMenuUI.ExitSelectMenu();

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
            PartyLevel[i].text = "Level: " + GameManager.Instance.Party.GetCharLvl(partyMember);
            PartyHP[i].text = "HP: " + GameManager.Instance.Party.GetCharCurrHP(partyMember) + "/" + GameManager.Instance.Party.GetCharMaxHP(partyMember);
            PartySP[i].text = "SP: " + GameManager.Instance.Party.GetCharCurrSP(partyMember) + "/" + GameManager.Instance.Party.GetCharMaxSP(partyMember);

            int currExp = GameManager.Instance.Party.GetCharCurrEXP(partyMember);
            int expToNextLvl = GameManager.Instance.Party.GetCharEXPtoNextLvl(partyMember);

            PartyExpToNextLvl[i].value = ((float) currExp) / expToNextLvl;
            PartyEXP[i].text = "" + (expToNextLvl - currExp);
        }
    }

    // click on button to open menu
    public void OpenMenu(int menu) {
        previousHud = currentHud;
        // close all menus
        closeAllMenu();

        // open menu
        MenuHud[menu].SetActive(true);

        // load ui for opened menu
        switch (menu) {
            case 0:
                // open main menu
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
                // open item menu
                currentHud = ITEM_TYPE;
                EventSystem.current.SetSelectedGameObject(ItemFirstHighlighted);
                break;
            case 2:
                // open skills menu
                currentHud = CHAR_SELECT_PANEL;
                SkillMenuUI.OpenSkillMenu();
                break;
            case 3:
                // open equipment menu
                currentHud = EQUIPMENT;
                EquipmentMenuUI.OpenEquipmentMenu();
                break;
            case 4:
                // open stats menu
                currentHud = STATS;
                StatsMenuUI.OpenStatsMenu();
                break;
            case 5:
                // open save menu
                currentHud = SAVE;
                SaveMenuUI.OpenSaveMenu();
                break;
        }
    }

    // close all the menus
    public void closeAllMenu() {
        foreach (GameObject menu in MenuHud) {
            menu.SetActive(false);
        }
    }

    public void SwitchCharacters(int character) {
        if (charToSwitch == -1) {
            charToSwitch = character;
            previousHud = currentHud;
            currentHud = PARTY;
            MainButtonsHud.interactable = false;
        } else {
            List<string> party = GameManager.Instance.Party.GetCurrentParty();
            string temp = party[charToSwitch];
            party[charToSwitch] = party[character];
            party[character] = temp;
            GameManager.Instance.Party.ChangeMembers(party);
            charToSwitch = -1;
            MainButtonsHud.interactable = true;
            currentHud = previousHud;
            updatePartyStats();
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
        itemToUse = ItemMenuUI.GetClickedItem();
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
            Items item = GameManager.Instance.GetItemDetails(itemToUse);

            if (item.AffectHP) {
                // heal hp
                int currHP = GameManager.Instance.Party.GetCharCurrHP(charName);
                int maxHP = GameManager.Instance.Party.GetCharMaxHP(charName);

                if (currHP < maxHP) {
                    // if not at max hp, can heal hp
                    item.Use(charName);
                }
            }
            if (item.AffectSP) {
                // heal sp
                int currSp = GameManager.Instance.Party.GetCharCurrSP(charName);
                int maxSp = GameManager.Instance.Party.GetCharMaxSP(charName);

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
                itemToUse = "";
            }
            // else, stay on this panel
        }
        if (usingSkill) {
            // using skill
            string charWithSkill = SkillMenuUI.GetCurrentCharacter();
            int currSp = GameManager.Instance.Party.GetCharCurrSP(charWithSkill);
            if (currSp > skillToUse.Cost) {
                // can only use skill if have enough sp
                int currHP = GameManager.Instance.Party.GetCharCurrHP(charName);
                int maxHP = GameManager.Instance.Party.GetCharMaxHP(charName);

                if (currHP < maxHP) {
                    skillToUse.UseSkill(charWithSkill, charName);
                }
            }
        }
    }

    public void Discard() {
        itemToUse = ItemMenuUI.GetClickedItem();
        GameManager.Instance.RemoveItem(itemToUse, 1);
        int itemAmt = GameManager.Instance.GetAmountOfItem(itemToUse);
        if (itemAmt == 0) {
            // if used last of this item, go back to item list
            DescriptionPanel.interactable = false;

            ItemList.interactable = true;
            ItemMenuUI.SetNewItem();

            previousHud = ITEM_TYPE;
            currentHud = ITEM_LIST;
            itemToUse = "";
        }
    }

    // select which character to look at skills
    public void SelectSkillsChar() {
        previousHud = currentHud;
        currentHud = SKILLS_LIST;

        SkillMenuUI.OpenSkillsPanel();
    }

    public void UseSkill(int choice) {
        string currSkill = SkillMenuUI.GetClickedSkill(choice);
        string currChar = SkillMenuUI.GetCurrentCharacter();
        skillToUse = GameManager.Instance.Party.GetCharSkill(currChar, currSkill);

        if (skillToUse.IsHeal) {
            // only go to select screen if skill is healing skill
            usingSkill = true;

            previousHud = currentHud;
            currentHud = SELECT;

            // set skills panel to not interactable
            SkillsPanel.interactable = false;

            SelectMenu.SetActive(true);
            SelectPanel.interactable = true;
            EventSystem.current.SetSelectedGameObject(SelectCharacters[0].gameObject);
        } else {
            skillToUse = null;
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

    // save the game
    public void Save(int file) {
        StartCoroutine(SaveCo(file));
    }

    private IEnumerator SaveCo(int file) {
        // save
        SaveFileManager.Save(file);
        // wait 1 sec
        yield return new WaitForSeconds(1f);
        SaveMenuUI.UpdateSaveMenu();
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
        itemToUse = "";
        usingItem = false;
        usingSkill = false;
        GameManager.Instance.GameMenuOpen = false;
    }
}
