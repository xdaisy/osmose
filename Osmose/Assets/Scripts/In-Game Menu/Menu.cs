using System.Collections;
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

    private string previousHud;
    private string currentHud;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameMenuOpen = true;
        previousHud = "Main";
        currentHud = "Main";

        eventSystem = EventSystem.current;
        updatePartyStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (previousHud == "Main" && currentHud != "Main") {
                // go back to main menu
                if (currentHud == "ItemType") {
                    currentHud = "Items";
                }

                OpenMenu(0);
            }
            if (previousHud == "ItemType") {
                ItemList.interactable = false;
                ItemType.interactable = true;
                previousHud = "Main";
                currentHud = "ItemType";
                ItemMenuUI.ExitItemList();
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
                currentHud = "Main";
                break;
            case 1:
                currentHud = "ItemType";
                eventSystem.SetSelectedGameObject(ItemFirstHighlighted);
                break;
            case 2:
                currentHud = "Skills";
                break;
            case 3:
                currentHud = "Equipment";
                EquipmentMenuUI.OpenEquipmentMenu();
                break;
            case 4:
                currentHud = "Stats";
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
        currentHud = "ItemList";
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
        EquipmentMenuUI.ShowCharacterEquipment(character);
        EquipmentCharacters.interactable = false;
        EquippedPanel.interactable = true;
        eventSystem.SetSelectedGameObject(EquipmentFirstEquipped);
    }

    public void SelectWhichEquipment(bool isWeapon) {
        EquippedPanel.interactable = false;
        EquipmentPanel.interactable = true;
    }
}
