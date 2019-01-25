using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    private EventSystem eventSystem;
    public GameObject[] MenuHud;

    public ItemMenu ItemMenuUI;

    // Party stats HUD
    public GameObject[] PartyStatHud;
    public Text[] PartyName;
    public Text[] PartyLevel;
    public Text[] PartyHP;
    public Text[] PartySP;
    public Slider[] PartyExpToNextLvl;
    public Text[] PartyEXP;

    // Item HUD
    public GameObject ItemFirstHighlightedObject;
    public CanvasGroup ItemType;
    public CanvasGroup ItemList;
    public CanvasGroup DescriptionPanel;

    private string previousHud;
    private string currentHud;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameMenuOpen = true;
        previousHud = "Main";

        eventSystem = EventSystem.current;
        updatePartyStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (previousHud == "Main") {
                // go back to main menu
                OpenMenu(0);

                Button[] mainButtons = MenuHud[0].GetComponentsInChildren<Button>();
                for (int i = 0; i < mainButtons.Length; i++) {
                    if (mainButtons[i].name == previousHud) {
                        eventSystem.SetSelectedGameObject(mainButtons[i].gameObject);
                        break;
                    }
                }
            }
            if (previousHud == "ItemType") {
                ItemList.interactable = false;
                ItemType.interactable = true;
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
            PartySP[i].text = "SP: " + GameManager.Instance.Party.GetCharacterCurrentSp(partyMember) + "/" + GameManager.Instance.Party.GetCharacterMaxSp(partyMember);

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
                currentHud = "Main";
                break;
            case 1:
                currentHud = "ItemType";
                eventSystem.SetSelectedGameObject(ItemFirstHighlightedObject);
                break;
            case 2:
                currentHud = "Skills";
                break;
            case 3:
                currentHud = "Equipment";
                break;
            case 4:
                currentHud = "Stats";
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
                ItemMenuUI.SetItemType(ITEMTYPE.ITEMS);
                break;
            case 1:
                // equipments
                ItemMenuUI.SetItemType(ITEMTYPE.EQUIPMENT);
                break;
            case 2:
                // key
                ItemMenuUI.SetItemType(ITEMTYPE.KEY);
                break;
        }
    }
}
