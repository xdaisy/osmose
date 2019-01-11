using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BattleSystem : MonoBehaviour {
    private EventSystem eventSystem; // event system for battle

    // All the HUDS
    public CanvasGroup MainHud;
    public CanvasGroup SkillHud;
    public CanvasGroup ItemHud;
    public CanvasGroup PartyHud;
    public CanvasGroup EnemyHud;
    public Text TextHud;

    private string previousHud; // keep track of the previous hud

    // For showing party members
    public GameObject[] PartyMembers;
    public Text[] PartyNames;
    public Text[] PartyHP;
    private int numPartyMembers;

    // For showing Item HUD
    public Button[] ItemButtons;
    public Text ItemName;
    public Text ItemDescription;
    public Text ItemAmount;
    public GameObject PreviousPageButton;
    public GameObject NextPageButton;

    private int itemStartOn = 0; // keeps track of the index of the items array we're looking at in Game Manager, use to know what page the items hud will be on

    private Items itemToUse; // keep track of which item selected to be used

    // keep track of whose turn is it for this round
    private Queue<string> turnOrder;

    private bool playerDeciding; // whether or not the player is deciding their move
    private string nextToGo; // keep track of whose turn is it
    private string textToShow; // shows player's action

    private int earnedExp;
    private int earnedMoney;

    private int numEnemiesRemaining;

    private bool escaped; // keeps track if the player escaped

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
        for (int i = 0; i < PartyMembers.Length; i++) {
            if (i >= party.Count) {
                // if have less members in current party than total party members, do not show the other
                PartyMembers[i].SetActive(false);
                continue;
            }
            string partyMember = party[i];
            PartyNames[i].text = partyMember;
            PartyHP[i].text = "" + GameManager.Instance.Party.GetCharacterCurrentHP(partyMember) 
                + "/" + GameManager.Instance.Party.GetCharacterMaxHP(partyMember);
        }

        determineTurnOrder();
        playerDeciding = false;
        textToShow = "";
        earnedExp = 0;
        earnedMoney = 0;
        numEnemiesRemaining = int.MaxValue;
        escaped = false;
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
        } else if (numEnemiesRemaining < 1) {
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
            playerDeciding = false;
        } else if (!playerDeciding && !TextHud.IsActive()) {
            nextToGo = turnOrder.Dequeue();

            if (GameManager.Instance.Party.IsInParty(nextToGo)) {
                // if the next to go is a party member, is player's turn
                MainHud.gameObject.SetActive(true);
                MainHud.interactable = true;

                // set the initial selected button
                Button[] battleCommands = MainHud.GetComponentsInChildren<Button>();
                Button attackButton = null;
                foreach (Button command in battleCommands) {
                    if (command.name == "AttackButton") {
                        attackButton = command;
                    }
                }
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(attackButton.gameObject);

                GameManager.Instance.Party.SetDefending(nextToGo, false); // character not defending at start of turn
                playerDeciding = true;
            } else {
                // enemy's turn
                MainHud.gameObject.SetActive(false);
                MainHud.interactable = false;

                GameObject enemy = GameObject.Find(nextToGo);
                Enemy enemyStat = enemy.GetComponent<Enemy>();
                enemyTurn(enemyStat);
            }
        } else if (TextHud.IsActive() && (Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel"))) {
            // stop displaying text (one one screen of text per damage)
            TextHud.gameObject.SetActive(false);
        } else if (Input.GetButtonDown("Cancel")) {
            // if on a different menu and hit cancel, go back to previous menu
            if (EnemyHud.interactable) {
                // if selecting enemy and cancel, go back to main menu on the attack button
                EnemyHud.interactable = false;
                MainHud.interactable = true;

                setSelectedButton("AttackButton");
            }

            if (SkillHud.interactable) {
                // selecting skill and cancel, go back to main menu on the skill button
                SkillHud.interactable = false;
                SkillHud.gameObject.SetActive(false);

                MainHud.interactable = true;
                MainHud.gameObject.SetActive(true);

                setSelectedButton("SkillsButton");
            }

            if (ItemHud.interactable) {
                // selecting item and cancel, go back to main menu on item button
                ItemHud.interactable = false;
                ItemHud.gameObject.SetActive(false);

                MainHud.interactable = true;
                MainHud.gameObject.SetActive(true);

                setSelectedButton("ItemsButton");
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
                textToShow = "Enemy defended";
                break;
            default:
                int member = UnityEngine.Random.Range(0, numPartyMembers);
                string partyMember = PartyNames[member].text;

                int damage = enemy.Attack - GameManager.Instance.Party.GetCharacterDefense(partyMember);
                if (GameManager.Instance.Party.IsDefending(partyMember)) {
                    damage /= 2;
                }

                GameManager.Instance.Party.DealtDamage(partyMember, damage);

                textToShow = partyMember + " took " + damage + " damage!";

                updateHP(member);
                break;
        }
    }

    private void updateHP(int member) {
        string partyMember = PartyNames[member].text;
        PartyHP[member].text = "" + GameManager.Instance.Party.GetCharacterCurrentHP(partyMember)
                    + "/" + GameManager.Instance.Party.GetCharacterMaxHP(partyMember);
    }

    /// <summary>
    /// Go from the Main Battle HUD to the Select Enemy HUD
    /// </summary>
    public void SelectAttack() {
        MainHud.interactable = false;
        EnemyHud.interactable = true;

        previousHud = "main";

        // TODO: Find way to get list of the Enemies and have the far left one selected 
        Button enemy = EnemyHud.GetComponentInChildren<Button>();
        eventSystem.SetSelectedGameObject(enemy.gameObject);
    }

    public void SelectEnemy() {
        EnemyHud.interactable = false;

        previousHud = "enemy";
        /// get enemy who was clicked
        GameObject lastClicked = eventSystem.currentSelectedGameObject;

        // get enemy object of the enenmy who was clicked
        Enemy enemy = lastClicked.GetComponent<Enemy>();

        // calculate the damage
        int damage = GameManager.Instance.Party.GetCharacterAttack(nextToGo) - enemy.Defense;

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
            textToShow = lastClicked.name + " took " + damage + " damage!";
        } else {
            earnedExp += enemy.Exp;
            earnedMoney += enemy.Money;
            numEnemiesRemaining--;
            textToShow = "Defeated " + lastClicked.name + "!";
        }

        if (numEnemiesRemaining < 1) {
            textToShow = "";
        }
    }

    public void ClickSkills() {
        // turn off main hud
        MainHud.interactable = false;
        MainHud.gameObject.SetActive(false);

        // turn on skill hud
        float currSp = GameManager.Instance.Party.GetCharacterCurrentSp(nextToGo);
        float maxSp = GameManager.Instance.Party.GetCharacterMaxSp(nextToGo);

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
                
                if (NextPageButton.activeSelf) {
                    // if found "", if next page button is active, turn it inactive
                    // find the first ""
                    // means no more items
                    NextPageButton.SetActive(false);
                }
            } else {
                // got item
                Text itemButtonText = itemButton.GetComponentInChildren<Text>();
                itemButtonText.text = item.ItemName;
            }
        }
        if (itemStartOn <= 0) {
            // if at beginning of item list, can't go back a page
            PreviousPageButton.SetActive(false);
        } else {
            // else, can go back a page
            PreviousPageButton.SetActive(true);
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

        GameManager.Instance.Party.SetDefending(nextToGo, true);

        playerDeciding = false;
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

        playerDeciding = false;
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
        } else if (itemToUse.AffectSP && GameManager.Instance.Party.GetCharacterCurrentSp(character) < GameManager.Instance.Party.GetCharacterMaxSp(character)) {
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

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        numEnemiesRemaining = enemies.Length;
        foreach(GameObject enemy in enemies) {
            string name = enemy.name;

            Enemy enemyStat = enemy.GetComponent<Enemy>();
            int idx = (int) enemyStat.Speed;

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
