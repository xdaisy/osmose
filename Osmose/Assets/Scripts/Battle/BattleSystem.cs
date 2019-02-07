using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BattleSystem : MonoBehaviour {
    private EventSystem eventSystem; // event system for battle

    // All the HUDS
    public CanvasGroup PartyHud;

    private string previousHud; // keep track of the previous hud

    // For showing party members
    [Header("Party")]
    public Text[] PartyNames;
    public Text[] PartyHP;
    private int numPartyMembers;

    // For showing Item HUD
    [Header("Item HUD")]
    public Button[] ItemButtons;
    public Text ItemName;
    public Text ItemDescription;
    public Text ItemAmount;

    private int itemStartOn = 0; // keeps track of the index of the items array we're looking at in Game Manager, use to know what page the items hud will be on

    private Items itemToUse; // keep track of which item selected to be used

    

    // ###############################
    // keep track of what is going to be in refactored script
    [Header("HUDs")]
    public CanvasGroup MainHud;
    public CanvasGroup SkillHud;
    public CanvasGroup ItemHud;
    public CanvasGroup SelectHud;
    public Text TextHud;
    public GameObject DescriptionPanel;

    [Header("Hud UI Objects")]
    public SelectHud SelectHudUI;

    private Queue<string> turnOrder; // keep track of whose turn is it for this round

    private bool playerTurn; // whether or not the it is the player's turn
    private string charTurn; // keep track of whose turn is it
    private string textToShow; // shows player's action

    private int earnedExp;
    private int earnedMoney;

    private List<Enemy> enemies;

    private bool escaped; // keeps track if the player escaped

    private bool attacking, usingItem, usingSkill; // boolean for keeping track of what the player is doing

    private const string ATTACK_BUTTON = "AttackButton";
    private const string ITEMS_BUTTON = "ItemsButton";
    private const string SKILLS_BUTTON = "SkillsButton";

    private void Awake() {
        turnOrder = new Queue<string>();
        itemToUse = null;
    }

    // Use this for initialization
    void Start () {
        GameManager.Instance.InBattle = true;

        eventSystem = EventSystem.current;
        // set up ui
        List<string> party = GameManager.Instance.Party.GetCurrentParty();
        numPartyMembers = party.Count;
        for (int i = 0; i < PartyNames.Length; i++) {
            if (i >= party.Count) {
                // if have less members in current party than total party members, do not show the other
                PartyNames[i].text = "";
                PartyHP[i].text = "";
                continue;
            }
            string partyMember = party[i];
            PartyNames[i].text = partyMember;
            PartyHP[i].text = "" + GameManager.Instance.Party.GetCharacterCurrentHP(partyMember) 
                + "/" + GameManager.Instance.Party.GetCharacterMaxHP(partyMember);
        }

        getEnemies();
        determineTurnOrder();
        playerTurn = false;
        textToShow = "";
        earnedExp = 0;
        earnedMoney = 0;
        escaped = false;
        attacking = false;
        usingItem = false;
        usingSkill = false;
        previousHud = "main";

        TextHud.gameObject.SetActive(true);
        TextHud.text = "Enemy appeared!";
	}
	
	// Update is called once per frame
	void Update () {
        // if the battle is still going on and the round is over, determine next turn order
        if (turnOrder.Count < 1) {
            determineTurnOrder();
        }
        
        if (escaped) {
            // escaped!
            TextHud.gameObject.SetActive(true);
            TextHud.text = "You escaped!";

            if (Input.GetButtonDown("Interact")) {
                // load back to previous scene
            }
        } else if (enemies.Count < 1) {
            // all enemies defeated, battle won!
            TextHud.gameObject.SetActive(true);
            TextHud.text = "You won! You earned " + earnedExp + " exp and " + earnedMoney + " money!";

            if (Input.GetButtonDown("Interact")) {
                // load back to previous scene
                GameManager.Instance.InBattle = false;
                PlayerControls.Instance.PreviousAreaName = "battle";
                // need to load back into scene
            }
        }

        if (textToShow.Length > 0) {
            // if player have finished their command, show text
            TextHud.gameObject.SetActive(true);
            TextHud.text = textToShow;
            textToShow = ""; // reset it to empty string
            playerTurn = false;
        } else if (!playerTurn && !TextHud.IsActive()) {
            charTurn = turnOrder.Dequeue();

            if (GameManager.Instance.Party.IsInParty(charTurn)) {
                // if the next to go is a party member, is player's turn
                MainHud.gameObject.SetActive(true);
                MainHud.interactable = true;

                // set the initial selected button
                Button[] battleCommands = MainHud.GetComponentsInChildren<Button>();
                Button attackButton = null;
                foreach (Button command in battleCommands) {
                    if (command.name == ATTACK_BUTTON) {
                        attackButton = command;
                    }
                }
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(attackButton.gameObject);

                GameManager.Instance.Party.SetDefending(charTurn, false); // character not defending at start of turn
                playerTurn = true;
            } else {
                // enemy's turn
                MainHud.gameObject.SetActive(false);
                MainHud.interactable = false;

                Enemy enemy = null;
                for (int i = 0; i < enemies.Count; i++) {
                    if (enemies[i].EnemyName == charTurn) {
                        enemy = enemies[i];
                    }
                }
                enemyTurn(enemy);
            }
        } else if (TextHud.IsActive() && (Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel"))) {
            // stop displaying text (one one screen of text per damage)
            TextHud.gameObject.SetActive(false);
        } else if (Input.GetButtonDown("Cancel")) {
            // if on a different menu and hit cancel, go back to previous menu

            if (SkillHud.interactable) {
                // selecting skill and cancel, go back to main menu on the skill button
                SkillHud.interactable = false;
                SkillHud.gameObject.SetActive(false);

                MainHud.interactable = true;
                MainHud.gameObject.SetActive(true);

                setSelectedButton(SKILLS_BUTTON);
            }

            if (ItemHud.interactable) {
                // selecting item and cancel, go back to main menu on item button
                ItemHud.interactable = false;
                ItemHud.gameObject.SetActive(false);

                MainHud.interactable = true;
                MainHud.gameObject.SetActive(true);

                setSelectedButton(ITEMS_BUTTON);
            }

            if (PartyHud.interactable) {
                switch(previousHud) {
                    case "item":
                        // party hud is now not interable
                        PartyHud.interactable = false;

                        // item hud is now interactable
                        // item hud is still visible, so don't have to set active
                        ItemHud.interactable = true;

                        string itemName = itemToUse.ItemName;
                        foreach (Button item in ItemButtons) {
                            if (item.GetComponentInChildren<Text>().text == itemName) {
                                eventSystem.SetSelectedGameObject(item.gameObject);
                                break;
                            }
                        }
                        break;
                    case "skill":
                        // party hud is now not interable
                        PartyHud.interactable = false;

                        // skill hud now interactable
                        // skill hud already visible, so don't have to set active
                        SkillHud.interactable = true;
                        break;
                    default:
                        break;
                }
            }

            if (SelectHud.interactable) {
                // close select hud
                SelectHud.interactable = false;
                SelectHud.gameObject.SetActive(false);

                if (attacking) {
                    // select the attack button
                    MainHud.interactable = true;
                    setSelectedButton(ATTACK_BUTTON);
                } else if (usingItem) {
                    // select the item that was previously on
                } else if (usingSkill) {
                    // select the skill that was previously on
                }
            }
        }

        if (ItemHud.gameObject.activeSelf) {
            // if item hud is active, make sure the correct description is on
            updateItemDescriptionPanel();
        }
	}

    private void enemyTurn(Enemy enemy) {
        enemy.IsDefending = false;

        int move = UnityEngine.Random.Range(0, 5);

        switch(move) {
            case 0:
                // enemy attack
                enemy.IsDefending = true;
                textToShow = enemy.EnemyName + " defended";
                break;
            default:
                int member = UnityEngine.Random.Range(0, numPartyMembers);
                string partyMember = PartyNames[member].text;

                int damage = enemy.Attack - GameManager.Instance.Party.GetCharacterDefense(partyMember);
                if (GameManager.Instance.Party.IsDefending(partyMember)) {
                    damage /= 2;
                }

                GameManager.Instance.Party.DealtDamage(partyMember, damage);

                textToShow = enemy.EnemyName + " attacked! " + partyMember + " took " + damage + " damage!";

                updateHP(member);
                break;
        }
    }

    private void updateHP(int member) {
        string partyMember = PartyNames[member].text;
        PartyHP[member].text = "" + GameManager.Instance.Party.GetCharacterCurrentHP(partyMember)
                    + "/" + GameManager.Instance.Party.GetCharacterMaxHP(partyMember);
    }

    // select attack and show the selection of enemies to attack
    public void SelectAttack() {
        MainHud.interactable = false;
        SelectHud.gameObject.SetActive(true);
        SelectHud.interactable = true;

        previousHud = "main";
        attacking = true;

        // TODO: Find way to get list of the Enemies and have the far left one selected 
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        string[] enemyNames = new string[enemies.Length];

        for (int i = 0; i < enemies.Length; i++) {
            enemyNames[i] = enemies[i].EnemyName;
        }

        SelectHudUI.OpenSelectHud(enemyNames);
    }

    public void Select(int choice) {
        if (attacking) {
            Enemy enemy = enemies[choice];

            // calculate the damage
            int damage = GameManager.Instance.Party.GetCharacterAttack(charTurn) - enemy.Defense;

            // if enemy is defending, reduce damage
            if (enemy.IsDefending) {
                damage = Mathf.RoundToInt(damage / 2);
            }

            // reduce enemy's hp
            enemy.CurrentHP -= damage;
            enemy.CurrentHP = Math.Max(enemy.CurrentHP, 0); // set so that 0 is the lowest amount it can go

            MainHud.gameObject.SetActive(false); // set main hud invisible

            // set the next displayed text
            if (enemy.CurrentHP > 0) {
                textToShow = enemy.EnemyName + " took " + damage + " damage!";
            } else {
                earnedExp += enemy.Exp;
                earnedMoney += enemy.Money;
                textToShow = "Defeated " + enemy.EnemyName + "!";
                enemies.RemoveAt(choice);
            }

            if (enemies.Count < 1) {
                textToShow = "";
            }

            SelectHud.interactable = false;
            SelectHud.gameObject.SetActive(false);
        } else if (usingItem) {

        } else if (usingSkill) {

        }
    }

    public void ClickSkills() {
        // turn off main hud
        MainHud.interactable = false;
        MainHud.gameObject.SetActive(false);

        // turn on skill hud
        float currSp = GameManager.Instance.Party.GetCharacterCurrentSP(charTurn);
        float maxSp = GameManager.Instance.Party.GetCharacterMaxSP(charTurn);

        previousHud = "main";

        SkillHud.gameObject.SetActive(true);
        SkillHud.interactable = true;
        Text spText = SkillHud.GetComponentInChildren<Text>();
        spText.text = "SP: " + currSp + "/" + maxSp;
    }

    public void ClickItems() {
        // turn off main hud
        MainHud.interactable = false;
        MainHud.gameObject.SetActive(false);

        // turn on item hud
        ItemHud.gameObject.SetActive(true);
        ItemHud.interactable = true;

        previousHud = "main";

        eventSystem.SetSelectedGameObject(null);
        Button item = ItemHud.GetComponentInChildren<Button>();
        eventSystem.SetSelectedGameObject(item.gameObject, null);

        showItems();
    }

    public void SelectItem(int itemIdx) {
        // get item info
        string itemName = ItemButtons[itemIdx].GetComponentInChildren<Text>().text;
        itemToUse = GameManager.Instance.GetItemDetails(itemName);

        // set item hud to not interactable
        ItemHud.interactable = false;

        // set party hud to interactable
        PartyHud.interactable = true;

        previousHud = "item";

        eventSystem.SetSelectedGameObject(null);
        Button firstHighlightedCharacter = PartyHud.GetComponentInChildren<Button>();
        eventSystem.SetSelectedGameObject(firstHighlightedCharacter.gameObject);
    }

    private void showItems() {
        for (int i = 0; i < ItemButtons.Length; i++) {
            // go through each item button
            int idx = i + itemStartOn; // get index for items array
            Button itemButton = ItemButtons[i];
            Items item = GameManager.Instance.GetItemAt(idx);
            if (item == null) {
                // if no item, set button off
                itemButton.gameObject.SetActive(false);
            } else {
                // got item
                Text itemButtonText = itemButton.GetComponentInChildren<Text>();
                itemButtonText.text = item.ItemName;
            }
        }

        updateItemDescriptionPanel();
    }

    private void updateItemDescriptionPanel() {
        GameObject currentSelected = eventSystem.currentSelectedGameObject;

        if (currentSelected.tag == "BattleItemButton") {
            // update if the button is a battle item button
            string currentSelectedItemName = currentSelected.GetComponentInChildren<Text>().text;

            if (currentSelectedItemName != ItemName.text) {
                // update if the current selected
                Items currentSelectedItem = GameManager.Instance.GetItemDetails(currentSelectedItemName);

                ItemName.text = currentSelectedItemName;
                ItemDescription.text = currentSelectedItem.Description;
                ItemAmount.text = "" + GameManager.Instance.GetAmountOfItem(currentSelectedItemName);
            }
        }
    }

    public void SelectDefend() {
        // turn off main hud
        MainHud.interactable = false;
        MainHud.gameObject.SetActive(false);

        GameManager.Instance.Party.SetDefending(charTurn, true);

        playerTurn = false;
    }

    public void SelectEscape() {
        // turn off main hud
        MainHud.interactable = false;
        MainHud.gameObject.SetActive(false);
        int escape = UnityEngine.Random.Range(0, 2);

        switch(escape) {
            case 0:
                escaped = true;
                break;
            default:
                escaped = false;
                break;
        }

        playerTurn = false;
    }

    public void SelectPartyMember(int partyMember) {
        string character = PartyNames[partyMember].text;

        if (itemToUse.AffectHP && GameManager.Instance.Party.GetCharacterCurrentHP(character) < GameManager.Instance.Party.GetCharacterMaxHP(character)) {
            // recover character hp if current hp is less than max
            itemToUse.Use(character);

            updateHP(partyMember);

            PartyHud.interactable = false;
            ItemHud.gameObject.SetActive(false);

            textToShow = character + " healed " + itemToUse.AmountToChange + " hp!";
        } else if (itemToUse.AffectSP && GameManager.Instance.Party.GetCharacterCurrentSP(character) < GameManager.Instance.Party.GetCharacterMaxSP(character)) {
            // recover character sp if current sp is less than max
            itemToUse.Use(character);

            PartyHud.interactable = false;
            ItemHud.gameObject.SetActive(false);

            textToShow = character + " healed " + itemToUse.AmountToChange + " sp!";
        } else {
            // character can't be healed
        }
    }

    private void determineTurnOrder() {
        string[] turn = new string[200];
        List<string> party = GameManager.Instance.Party.GetCurrentParty();

        foreach(string name in party) {
            float speed = GameManager.Instance.Party.GetCharacterSpeed(name);
            int idx = (int) speed;
            if (turn[idx] != null) {
                // put character in the next available index if speed match
                for (int i = idx - 1; i >= 0; i--) {
                    if (turn[i] == null) {
                        turn[i] = name;
                        break;
                    }
                }
            } else {
                turn[idx] = name;
            }
        }
        
        foreach(Enemy enemy in enemies) {
            string name = enemy.EnemyName;

            int idx = enemy.Speed;

            if (turn[idx] != null) {
                // put character in the next available index if speed match
                for (int i = idx - 1; i >= 0; i--) {
                    if (turn[i] == null) {
                        turn[i] = name;
                        break;
                    }
                }
            } else {
                turn[idx] = name;
            }
        }

        // put the one with the higher speed in the queue first
        for (int i = turn.Length - 1; i >= 0; i--) {
            if (turn[i] != null) {
                turnOrder.Enqueue(turn[i]);
            }
        }
    }

    // called in beginning of battle to get the list of enemies
    private void getEnemies() {
        enemies = new List<Enemy>();

        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemyList) {
            Enemy enemyStat = enemy.GetComponent<Enemy>();
            enemies.Add(enemyStat);
        }
    }

    private void setSelectedButton(string name) {
        Button[] battleCommands = MainHud.GetComponentsInChildren<Button>();
        Button button = null;
        foreach (Button command in battleCommands) {
            if (command.name == name) {
                button = command;
            }
        }
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(button.gameObject);
    }
}
