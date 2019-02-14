using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BattleSystem : MonoBehaviour {
    private EventSystem eventSystem; // event system for battle

    

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
    public PartyHud PartyHudUI;
    public ItemHud ItemHudUI;
    public PartyUI[] PartyMemUI;

    private Queue<string> turnOrder; // keep track of whose turn is it for this round

    private bool playerTurn; // whether or not the it is the player's turn
    private string charTurn; // keep track of whose turn is it
    private string textToShow; // shows player's action

    private List<Enemy> enemies;
    private int earnedExp;
    private int earnedMoney;

    // info about party members
    private List<string> party;
    private int numAliveChar; // keep track of how many members of the party are alive

    private bool escaped; // keeps track if the player escaped

    private bool attacking, usingItem, usingSkill; // boolean for keeping track of what the player is doing
    private string itemToUse; // string that keep track of which item was clicked

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
        party = GameManager.Instance.Party.GetCurrentParty();
        numAliveChar = party.Count;

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
            // next turn
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
            // stop displaying text (one one screen of text per move)
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
                DescriptionPanel.gameObject.SetActive(false);

                MainHud.interactable = true;
                MainHud.gameObject.SetActive(true);

                setSelectedButton(ITEMS_BUTTON);
            }

            if (SelectHud.interactable) {
                // close select hud
                SelectHud.interactable = false;
                SelectHud.gameObject.SetActive(false);

                SelectHudUI.ExitSelectHud();

                if (attacking) {
                    // select the attack button
                    MainHud.interactable = true;
                    setSelectedButton(ATTACK_BUTTON);
                } else if (usingItem) {
                    // select the item that was previously on
                    ItemHud.interactable = true;
                    ItemHudUI.SetLastClickedItem();
                } else if (usingSkill) {
                    // select the skill that was previously on
                }
            }
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
                int member = UnityEngine.Random.Range(0, numAliveChar);
                string partyMember = party[member];

                int damage = enemy.Attack - GameManager.Instance.Party.GetCharacterDefense(partyMember);
                if (GameManager.Instance.Party.IsDefending(partyMember)) {
                    damage /= 2;
                }

                GameManager.Instance.Party.DealtDamage(partyMember, damage);

                PartyHudUI.UpdateHP();

                textToShow = enemy.EnemyName + " attacked! " + partyMember + " took " + damage + " damage!";
                
                break;
        }
    }

    // select attack and show the selection of enemies to attack
    public void SelectAttack() {
        MainHud.interactable = false;
        SelectHud.gameObject.SetActive(true);
        SelectHud.interactable = true;
        
        attacking = true;

        // TODO: Find way to get list of the Enemies and have the far left one selected 
        SelectHudUI.OpenSelectHud(enemies.ToArray());
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

            enemy.Highlight(false);

            SelectHud.interactable = false;
            SelectHud.gameObject.SetActive(false);

            attacking = false;
        } else if (usingItem) {
            Items item = GameManager.Instance.GetItemDetails(itemToUse);
            string charName = party[choice];
            int currHP = GameManager.Instance.Party.GetCharacterCurrentHP(charName);
            int maxHP = GameManager.Instance.Party.GetCharacterMaxHP(charName);
            int currSP = GameManager.Instance.Party.GetCharacterCurrentSP(charName);
            int maxSP = GameManager.Instance.Party.GetCharacterMaxSP(charName);

            if ((item.AffectHP && currHP != maxHP) || (item.AffectSP && currSP != maxSP)) {
                // only use if can recover

                // heal
                item.Use(charName);

                ItemHud.gameObject.SetActive(false);
                // set the text to show
                textToShow = charName + " recovered ";
                if (item.AffectHP) {
                    textToShow += (GameManager.Instance.Party.GetCharacterCurrentHP(charName) - currHP) + " HP!";
                } else if (item.AffectSP) {
                    textToShow += (GameManager.Instance.Party.GetCharacterCurrentSP(charName) - currSP) + " SP!";
                }

                // update hp
                PartyHudUI.UpdateHP();

                // close select hud
                SelectHud.interactable = false;
                SelectHud.gameObject.SetActive(false);

                DescriptionPanel.SetActive(false);

                // reset
                usingItem = false;
                itemToUse = "";
            }
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

        SkillHud.gameObject.SetActive(true);
        SkillHud.interactable = true;
        Text spText = SkillHud.GetComponentInChildren<Text>();
        spText.text = "SP: " + currSp + "/" + maxSp;
    }

    // going to item hud
    public void SelectItems() {
        // turn off main hud
        MainHud.interactable = false;
        MainHud.gameObject.SetActive(false);

        // turn on item hud and description panel
        ItemHud.gameObject.SetActive(true);
        ItemHud.interactable = true;
        DescriptionPanel.gameObject.SetActive(true);

        ItemHudUI.OpenItemHud();
    }

    // get name of clicked item
    public void UseItem(int item) {
        itemToUse = ItemHudUI.GetClickedItem(item);
        ItemHud.interactable = false;
        SelectHud.gameObject.SetActive(true);
        SelectHud.interactable = true;

        usingItem = true;

        int numActPartyUI = PartyHudUI.GetNumActiveUI();

        PartyUI[] activeParty = new PartyUI[numActPartyUI];
        for(int i = 0; i < numActPartyUI; i++) {
            activeParty[i] = PartyMemUI[i];
        }

        SelectHudUI.OpenSelectHud(activeParty, party.ToArray());
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
