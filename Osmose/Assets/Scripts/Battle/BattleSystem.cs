using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleSystem : MonoBehaviour {

    public EventSystem eventSystem;
    public CanvasGroup MainHud;
    public CanvasGroup PartyHud;
    public CanvasGroup EnemyHud;
    public Text TextHud;

    // keep track of whose turn is it for this round
    private Queue<string> turnOrder;

    private bool playerDeciding;

    private void Awake() {
        turnOrder = new Queue<string>();
    }

    // Use this for initialization
    void Start () {
        determineTurnOrder();
        playerDeciding = false;

        TextHud.gameObject.SetActive(true);
        TextHud.text = "Enemy appeared!";
	}
	
	// Update is called once per frame
	void Update () {
        // if the battle is still going on and the round is over, determine next turn order
        if (turnOrder.Count < 1) {
            determineTurnOrder();
        }

        if (!playerDeciding && !TextHud.IsActive()) {
            string nextToGo = turnOrder.Dequeue();

            if (PartyStats.IsInParty(nextToGo)) {
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

                playerDeciding = true;
            } else {
                // enemy's turn
                MainHud.gameObject.SetActive(false);
                MainHud.interactable = false;

                TextHud.gameObject.SetActive(true);
                TextHud.text = "Enemy's turn";
            }
        } else if (TextHud.IsActive() && (Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel"))) {
            // stop displaying text (one one screen of text per damage)
            TextHud.gameObject.SetActive(false);
        } else if (Input.GetButtonDown("Cancel")) {
            // if on a different menu and hit cancel, go back to previous menu
            if (EnemyHud.gameObject.activeSelf) {
                // if selecting enemy and cancel, go back to main menu on the attack button
                EnemyHud.interactable = false;
                MainHud.interactable = true;

                Button[] battleCommands = MainHud.GetComponentsInChildren<Button>();
                Button attackButton = null;
                foreach (Button command in battleCommands) {
                    if (command.name == "AttackButton") {
                        attackButton = command;
                    }
                }
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(attackButton.gameObject);
            }
        }
	}

    /// <summary>
    /// Go from the Main Battle HUD to the Select Enemy HUD
    /// </summary>
    public void SelectAttack() {
        MainHud.interactable = false;
        EnemyHud.interactable = true;

        // TODO: Find way to get list of the Enemies and have the far left one selected 
        Button enemy = EnemyHud.GetComponentInChildren<Button>();
        eventSystem.SetSelectedGameObject(enemy.gameObject);
    }

    public void SelectEnemy() {
        EnemyHud.interactable = false;
        /// get enemy who was clicked
        GameObject lastClicked = eventSystem.currentSelectedGameObject;

        // get enemy object of the enenmy who was clicked
        Enemy enemy = lastClicked.GetComponent<Enemy>();

        playerDeciding = false;
    }

    private void determineTurnOrder() {
        string[] turn = new string[200];
        List<string> party = PartyStats.GetCurrentParty();

        foreach(string name in party) {
            float speed = PartyStats.GetCharacterSpeed(name);
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
}
