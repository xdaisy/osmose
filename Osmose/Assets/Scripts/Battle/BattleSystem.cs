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

    // keep track of whose turn is it for this round
    private Queue<string> turnOrder;

    private void Awake() {
        turnOrder = new Queue<string>();
    }

    // Use this for initialization
    void Start () {
        determineTurnOrder();
	}
	
	// Update is called once per frame
	void Update () {
        // if the battle is still going on and the round is over, determine next turn order
        if (turnOrder.Count < 1) {
            determineTurnOrder();
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
        Debug.Log("Enemy Attacked");
    }

    private void determineTurnOrder() {
        string[] turn = new string[200];
        List<string> party = PartyStats.GetCurrentParty();

        foreach(string name in party) {
            float speed = PartyStats.GetCharacterSpeed(name);
            int idx = (int) speed;
            if (turn[idx] != null) {
                // put character in the next available index if speed match
                for (int i = idx + 1; i < turn.Length; i++) {
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
                for (int i = idx + 1; i < turn.Length; i++) {
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
