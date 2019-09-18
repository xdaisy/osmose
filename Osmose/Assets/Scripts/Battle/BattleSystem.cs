using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleSystem : MonoBehaviour {
    [Header("HUDs")]
    public CanvasGroup MainHud;
    public CanvasGroup SkillHud;
    public CanvasGroup ItemHud;
    public CanvasGroup SelectHud;
    public GameObject TextImage;
    public Text TextHud;
    public GameObject DescriptionPanel;
    public Button[] Commands;

    [Header("Hud UI Objects")]
    public MainHud MainHudUI;
    public SelectHud SelectHudUI;
    public PartyHud PartyHudUI;
    public ItemHud ItemHudUI;
    public SkillHud SkillHudUI;
    public PartyUI[] PartyMemUI;

    [Header("Character Pos")]
    public Transform[] CharPos;
    public Image CharTurnImage;
    public Image ShiftImage;

    [Header("Enemy Spawning")]
    public EnemyHandler EnemiesHandler;

    [Header("Effects")]
    public DamageEffect DamageNumber;

    private TurnQueue turnOrder; // keep track of whose turn is it for this round

    private bool playerTurn; // whether or not the it is the player's turn
    private bool enemyTurn;
    private string charTurn; // keep track of whose turn is it
    private Queue<string> textToShow; // shows player's action

    private List<int> hostilityMeter; // array of which character for enemy to attack

    private List<Enemy> enemies;
    private int earnedExp;
    private int earnedMoney;

    // info about party members
    private List<string> party;
    private int numAliveChar; // keep track of how many members of the party are alive

    private bool arenShifted; // keep track if Aren is shifted or not
    private bool arenShiftOnce; // keep track of if decided to unshift so restore sp

    private bool escaped; // keeps track if the player escaped

    private bool attacking, usingItem, usingSkill; // boolean for keeping track of what the player is doing
    private string itemToUse; // string that keep track of which item was clicked
    private Skill skillToUse; // string that keep track of which skill was clicked

    private const string ATTACK_BUTTON = "AttackButton";
    private const string ITEMS_BUTTON = "ItemsButton";
    private const string SKILLS_BUTTON = "SkillsButton";

    private bool endedBattle;

    // for loading back to previous scene

    private void Awake() {
        turnOrder = new TurnQueue();
        itemToUse = null;
        hostilityMeter = new List<int>();
    }

    // Use this for initialization
    void Start() {
        GameManager.Instance.InBattle = true;

        // set up ui
        party = GameManager.Instance.Party.GetCurrentParty();
        for (int i = 0; i < party.Count; i++) {
            hostilityMeter.Add(i);
        }
        numAliveChar = party.Count;

        playerTurn = false;
        enemyTurn = false;
        textToShow = new Queue<string>();
        earnedExp = 0;
        earnedMoney = 0;
        escaped = false;
        attacking = false;
        usingItem = false;
        usingSkill = false;

        TextImage.SetActive(true);
        TextHud.gameObject.SetActive(true);
        TextHud.text = "Enemy appeared!";

        endedBattle = false;

        Enemy[] en = GameObject.FindObjectsOfType<Enemy>();
        
        if (en.Length > 0) {
            // if there are enemies on the scene, don't spawn enemies
            enemies = new List<Enemy>(en);
        } else {
            // if there are not enemies on the scene, spawn enemies
            enemies = EnemiesHandler.GetEnemies();
        }
        turnOrder = BattleLogic.DetermineTurnOrder(enemies);
    }

    // Update is called once per frame
    void Update() {
        // if the battle is still going on and the round is over, determine next turn order
        if (turnOrder.Count < 1) {
            turnOrder = BattleLogic.DetermineTurnOrder(enemies);
        }

        if (escaped) {
            // escaped!
            textToShow.Enqueue("You escaped!");
            showText();

            if (endedBattle && Input.GetButtonDown("Interact")) {
                // load back to previous scene
                GameManager.Instance.InBattle = false;
                ExitBattle battleExit = FindObjectOfType<ExitBattle>();
                battleExit.EndBattle();
            }
            endedBattle = true;
        } else if (enemies.Count < 1) {
            // all enemies defeated, battle won!
            
            if (!endedBattle) {
                textToShow.Enqueue("The monsters ran away! You earned " + earnedExp + " exp and " + earnedMoney + " money!");
                GameManager.Instance.Party.ResetStatsModifier();
                List<string> leveledUpChar = GameManager.Instance.Party.GainExperience(earnedExp);
                string levelUp = "";
                foreach (string name in leveledUpChar) {
                    levelUp += name + " leveled up!\n";
                }
                if (levelUp.Length > 0) {
                    textToShow.Enqueue(levelUp);
                }
                GameManager.Instance.GainMoney(earnedMoney);
            }
            //showText();

            if (endedBattle && textToShow.Count < 1 && Input.GetButtonDown("Interact")) {
                // load back to previous scene
                GameManager.Instance.InBattle = false;
                // need to load back into scene
                ExitBattle battleExit = FindObjectOfType<ExitBattle>();
                battleExit.EndBattle();
            }
            endedBattle = true;
        } else if (isPartyDead()) {
            // all characters dead, battle is over
            textToShow.Enqueue("You were defeated...");
            showText();
            if (endedBattle && textToShow.Count < 1 && Input.GetButtonDown("Interact")) {
                GameManager.Instance.InBattle = false;
                // load back to last town
                ExitBattle battleExit = FindObjectOfType<ExitBattle>();
                battleExit.DefeatedInBattle();
            }
            endedBattle = true;
        }

        if (textToShow.Count > 0 && (!TextHud.IsActive() || Input.GetButtonDown("Interact"))) {
            // if player have finished their command, show text
            showText();
        } else if (!playerTurn && !enemyTurn && !TextHud.IsActive()) {
            // next turn
            nextTurn();

            if (party.Contains(charTurn) && GameManager.Instance.Party.IsAlive(charTurn)) {
                setCurrTurnImage();
                playerTurn = true;
                // if curr character's turn is aren && is shifted && magic < 25%, attack
                if (charTurn == Constants.AREN && arenShifted && GameManager.Instance.GetMagicMeter() < 0.25) {
                    int choice = UnityEngine.Random.Range(0, enemies.Count); // randomly attack an enemy
                    int damage = BattleLogic.RegularAttack(charTurn, enemies[choice]);
                    displayDamageToEnemies(damage, choice);

                } else {

                    // if the next to go is a party member and character is alive, is player's turn
                    MainHud.gameObject.SetActive(true);
                    MainHud.interactable = true;

                    if (charTurn == Constants.AREN) {
                        MainHudUI.ArenShifted(arenShifted);
                    } else {
                        MainHudUI.SetItemsActive();
                    }

                    // set the initial selected button
                    Button attackButton = null;
                    foreach (Button command in Commands) {
                        if (command.name == ATTACK_BUTTON) {
                            attackButton = command;
                        }
                    }
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(attackButton.gameObject);

                    GameManager.Instance.Party.SetDefending(charTurn, false); // character not defending at start of turn
                }
            } else if (party.Contains(charTurn) && GameManager.Instance.Party.IsAlive(charTurn)) {
                // if character is in party but is dead, do nothing
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
                StartCoroutine(enemyTurnCo(enemy));
                enemyTurn = true;
            }
        } else if (TextHud.IsActive() && textToShow.Count < 1 && !endedBattle && (Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel"))) {
            // stop displaying text (one one screen of text per move)
            TextImage.SetActive(false);
            TextHud.gameObject.SetActive(false);
        } else if (Input.GetButtonDown("Cancel")) {
            // if on a different menu and hit cancel, go back to previous menu

            if (SkillHud.interactable) {
                // selecting skill and cancel, go back to main menu on the skill button
                SkillHud.interactable = false;
                SkillHud.gameObject.SetActive(false);
                DescriptionPanel.SetActive(false);
                SkillHudUI.ExitSkillHud();

                MainHud.interactable = true;
                MainHud.gameObject.SetActive(true);

                setSelectedButton(SKILLS_BUTTON);
            }

            if (ItemHud.interactable) {
                // selecting item and cancel, go back to main menu on item button
                ItemHud.interactable = false;
                ItemHud.gameObject.SetActive(false);
                DescriptionPanel.gameObject.SetActive(false);
                ItemHudUI.ExitItemHud();

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
                    attacking = false;
                    MainHud.interactable = true;
                    setSelectedButton(ATTACK_BUTTON);
                } else if (usingItem) {
                    // select the item that was previously on
                    usingItem = false;
                    ItemHud.interactable = true;
                    ItemHudUI.SetLastClickedItem();
                } else if (usingSkill) {
                    // select the skill that was previously on
                    usingSkill = false;
                    SkillHud.interactable = true;
                    SkillHudUI.SetLastClickedSkill();
                }
            }
        }
    }

    // select attack and show the selection of enemies to attack
    public void SelectAttack() {
        MainHud.interactable = false;
        SelectHud.gameObject.SetActive(true);
        SelectHud.interactable = true;

        attacking = true;

        SelectHudUI.OpenSelectHud(enemies.ToArray());
    }

    public void Select(int choice) {
        if (attacking) {
            Enemy enemy = enemies[choice];

            int damage = BattleLogic.RegularAttack(charTurn, enemy);

            // show damage
            displayDamageToEnemies(damage, choice);

            MainHud.gameObject.SetActive(false); // set main hud invisible

            enemy.Highlight(false);

            SelectHud.interactable = false;
            SelectHud.gameObject.SetActive(false);
            SelectHudUI.ExitSelectHud();

            attacking = false;
        } else if (usingItem) {
            Items item = GameManager.Instance.GetItemDetails(itemToUse);
            string charName = party[choice];
            int currHP = GameManager.Instance.Party.GetCharCurrHP(charName);
            int maxHP = GameManager.Instance.Party.GetCharMaxHP(charName);
            int currSP = GameManager.Instance.Party.GetCharCurrSP(charName);
            int maxSP = GameManager.Instance.Party.GetCharMaxSP(charName);

            if ((item.AffectHP && currHP != maxHP) || (item.AffectSP && currSP != maxSP)) {
                // only use if can recover
                int amountHealed = BattleLogic.UseHealingItem(charName, item);

                ItemHud.gameObject.SetActive(false);
                ItemHudUI.ExitItemHud();

                // show amount healed
                StartCoroutine(showDamage(CharPos[choice].position, amountHealed, false));

                // close select hud
                SelectHud.interactable = false;
                SelectHud.gameObject.SetActive(false);
                SelectHudUI.ExitSelectHud();

                DescriptionPanel.SetActive(false);

                // reset
                usingItem = false;
                itemToUse = "";
            }
        } else if (usingSkill) {
            if (skillToUse.IsPhyAttk || skillToUse.IsMagAttk) {
                // if attacking enemy
                Enemy enemy = enemies[choice];

                int damage = BattleLogic.UseAttackSkill(charTurn, enemy, skillToUse);

                displayDamageToEnemies(damage, choice);

                // close skill hud
                SkillHud.gameObject.SetActive(false);
                SkillHud.interactable = false;
                DescriptionPanel.SetActive(false);
                SkillHudUI.ExitSkillHud();

                // close select hud
                SelectHud.interactable = false;
                SelectHud.gameObject.SetActive(false);
                SelectHudUI.ExitSelectHud();

                usingSkill = false;
                skillToUse = null;
            } else if (skillToUse.IsHeal) {
                // heal
                string charName = party[choice];
                if (skillToUse.UseOnSelf) {
                    // if use on self, don't get from party
                    charName = charTurn;
                }
                int currHP = GameManager.Instance.Party.GetCharCurrHP(charName);
                int maxHP = GameManager.Instance.Party.GetCharMaxHP(charName);

                if (currHP < maxHP) {
                    // only use if can recover hp
                    int amountHealed = BattleLogic.UseHealingSkill(charTurn, charName, skillToUse);

                    // close skill hud
                    SkillHud.gameObject.SetActive(false);
                    SkillHud.interactable = false;
                    DescriptionPanel.SetActive(false);
                    SkillHudUI.ExitSkillHud();

                    // show amount healed
                    StartCoroutine(showDamage(CharPos[choice].position, amountHealed, false));

                    // close select hud
                    SelectHud.interactable = false;
                    SelectHud.gameObject.SetActive(false);
                    SelectHudUI.ExitSelectHud();

                    DescriptionPanel.SetActive(false);

                    // reset
                    usingSkill = false;
                    skillToUse = null;
                }
            }
        }
    }

    public void SelectSkills() {
        // turn off main hud
        MainHud.interactable = false;
        MainHud.gameObject.SetActive(false);

        // turn on skill hud
        SkillHud.gameObject.SetActive(true);
        SkillHud.interactable = true;
        DescriptionPanel.SetActive(true);

        if (charTurn == Constants.AREN) {
            SkillHudUI.OpenSkillsHud(charTurn, arenShifted);
        } else {
            SkillHudUI.OpenSkillsHud(charTurn);
        }
    }

    public void UseSkill(int skill) {
        skillToUse = SkillHudUI.GetClickedSkill(skill);
        bool endTurn = false;

        if (skillToUse is TauntSkill) {
            for (int i = 0; i < party.Count; i++) {
                if (party[i] == Constants.NAOISE) {
                    hostilityMeter.Add(i);
                    hostilityMeter.Add(i);
                    break;
                }
            }
            endTurn = true;
            playerTurn = false;
        } else if ((GameManager.Instance.Party.GetCharCurrSP(charTurn) > skillToUse.Cost && skillToUse is ShiftSkill) || skillToUse is UnshiftSkill) {
            // aren shifting
            ShiftImage.gameObject.SetActive(skillToUse is ShiftSkill);
            arenShifted = skillToUse is ShiftSkill;
            float magicMeter = GameManager.Instance.GetMagicMeter();
            skillToUse.UseSkill(charTurn);
            SkillHudUI.OpenSkillsHud(charTurn, arenShifted);
            MainHudUI.ArenShifted(arenShifted);
            if (magicMeter < 0.75) {
                // if magic meter < 75%, player must end turn
                endTurn = true;
                playerTurn = false;
            }
            skillToUse = null;
        } else if (GameManager.Instance.Party.GetCharCurrSP(charTurn) > skillToUse.Cost) {
            // regular skill
            // can only use if have enough sp to use

            usingSkill = true;

            if (skillToUse.IsPhyAttk || skillToUse.IsMagAttk) {
                // use on enemy
                SkillHud.interactable = false;
                SelectHud.gameObject.SetActive(true);
                SelectHud.interactable = true;

                SelectHudUI.OpenSelectHud(enemies.ToArray());
            } else if (skillToUse.UseOnSelf) {
                // use skill on self
                int currHP = GameManager.Instance.Party.GetCharCurrHP(charTurn);
                int maxHP = GameManager.Instance.Party.GetCharMaxHP(charTurn);
                if (skillToUse.IsHeal && currHP < maxHP) {
                    int amountHealed = BattleLogic.UseHealingSkill(charTurn, skillToUse);
                    int pos = 0;
                    for (int i = 0; i < party.Count; i++) {
                        if (party[i] == charTurn) {
                            pos = i;
                        }
                    }
                    StartCoroutine(showDamage(CharPos[pos].position, amountHealed, false));
                    endTurn = true;
                }
            } else if (skillToUse.IsHeal) {
                // use healing skill on party if not use on self
                SkillHud.interactable = false;
                SelectHud.gameObject.SetActive(true);
                SelectHud.interactable = true;

                int numActPartyUI = PartyHudUI.GetNumActiveUI();

                PartyUI[] activeParty = new PartyUI[numActPartyUI];
                for (int i = 0; i < numActPartyUI; i++) {
                    activeParty[i] = PartyMemUI[i];
                }

                SelectHudUI.OpenSelectHud(activeParty, party.ToArray());
            }
        } else {
            skillToUse = null;
        }
        if (endTurn) {
            SkillHud.gameObject.SetActive(false);
            SkillHud.interactable = false;
            DescriptionPanel.SetActive(false);
            SkillHudUI.ExitSkillHud();
            CharTurnImage.gameObject.SetActive(false);
        }
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

        CharTurnImage.gameObject.SetActive(false);
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

    private void showText() {
        MainHud.gameObject.SetActive(false);

        TextImage.SetActive(true);
        TextHud.gameObject.SetActive(true);
        TextHud.text = textToShow.Dequeue();
    }

    private void displayDamageToEnemies(int damage, int choice) {
        Enemy enemy = enemies[choice];

        // show damage
        if (enemy.CurrentHP > 0) {
            // if enemy didn't die
            StartCoroutine(showDamage(enemy.transform.position, damage, true));
        } else {
            // enemy died
            StartCoroutine(showDamage(enemy.transform.position, damage));
            earnedExp += enemy.Exp;
            earnedMoney += enemy.Money;
            StartCoroutine(enemyDied(enemy, choice));
        }
    }

    // show amount damaged/recovered if enemy didn't die
    private IEnumerator showDamage(Vector3 pos, int amount, bool isAttack) {
        Instantiate(DamageNumber).SetDamage(pos, amount, isAttack);
        yield return new WaitForSeconds(DamageNumber.Duration);
        CharTurnImage.gameObject.SetActive(false);
        playerTurn = false;
    }

    // show damage when an enemy dies
    private IEnumerator showDamage(Vector3 pos, int amount) {
        Instantiate(DamageNumber).SetDamage(pos, amount, true);
        yield return new WaitForSeconds(DamageNumber.Duration);
    }

    private void nextTurn() {
        charTurn = turnOrder.Dequeue();
    }

    private IEnumerator enemyTurnCo(Enemy enemy) {
        enemy.DoMove();
        yield return new WaitForSeconds(0.5f);
        enemyMove(enemy);
        yield return new WaitForSeconds(0.5f);
        enemyTurn = false;
    }

    private void enemyMove(Enemy enemy) {
        EnemyTurn enemyTurn = BattleLogic.EnemyTurn(enemy, hostilityMeter);

        if (enemyTurn.Defend) {
            textToShow.Enqueue(enemy.EnemyName + " defended");
        }
        if (enemyTurn.Attack) {
            Instantiate(DamageNumber).SetDamage(CharPos[hostilityMeter[enemyTurn.Target]].transform.position, enemyTurn.Amount, true);
            string partyMember = party[hostilityMeter[enemyTurn.Target]];

            if (!GameManager.Instance.Party.IsAlive(partyMember)) {
                numAliveChar--;
            }
        }
    }

    // wait 1 sec when enemy die
    private IEnumerator enemyDied(Enemy enemy, int choice) {
        yield return new WaitForSeconds(1f);
        enemies.RemoveAt(choice);
        turnOrder.RemoveFromQueue(enemy.EnemyName);
        playerTurn = false;
    }

    private void setSelectedButton(string name) {
        EventSystem.current.SetSelectedGameObject(null);
        foreach (Button command in Commands) {
            if (command.name == name) {
                EventSystem.current.SetSelectedGameObject(command.gameObject);
                break;
            }
        }
    }

    // return whether or not all the party members are dead
    private bool isPartyDead() {
        return numAliveChar < 1;
    }

    private void setCurrTurnImage() {
        for (int i = 0; i < party.Count; i++) {
            if (party[i] == charTurn) {
                CharTurnImage.gameObject.SetActive(true);
                CharTurnImage.transform.position = new Vector3(CharTurnImage.rectTransform.position.x, CharPos[i].position.y, CharTurnImage.rectTransform.position.z);
                break;
            }
        }
    }
}
