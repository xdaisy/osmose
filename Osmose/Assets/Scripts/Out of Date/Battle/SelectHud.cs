using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectHud : MonoBehaviour
{
    public Text[] SelectChoices; // what can be selected on the select hud

    private Enemy[] enemyObjects; // list of enemy objects that will be highlighted
    private PartyUI[] partyObjects; // list of party member objects that will be highlighted
    private bool isSelectingEnemy; // keep track if selecting enemies or party members

    private string currSelected;
    private int currSelectedIndx;

    private void Start() {
        currSelected = "";
        currSelectedIndx = 0;
    }

    void Update() {
        if (Input.GetButtonDown("Vertical")) {
            Text currObject = EventSystem.current.currentSelectedGameObject.GetComponent<Text>();
            if (currSelected != currObject.text) {
                int selectedIndx = findCurrObject(currObject.text);

                if (isSelectingEnemy) {
                    // is selecting enemy
                    // dehighlight previous selected enemy
                    Enemy prevSelectedEnemy = enemyObjects[currSelectedIndx];
                    prevSelectedEnemy.Highlight(false);

                    // highlight current selected enemy
                    Enemy currSelectedEnemy = enemyObjects[selectedIndx];
                    currSelectedEnemy.Highlight(true);
                } else {
                    // is selecting party
                    // dehighlight previous selected party ui
                    PartyUI prevPartyUI = partyObjects[currSelectedIndx];
                    prevPartyUI.Highlight(false);

                    // high current selected party ui
                    PartyUI currSelectedPartyUI = partyObjects[selectedIndx];
                    currSelectedPartyUI.Highlight(true);
                }

                currSelected = currObject.text;
                currSelectedIndx = selectedIndx;
            }
        }
    }

    private int findCurrObject(string name) {
        for (int i = 0; i < SelectChoices.Length; i++) {
            if (SelectChoices[i].text == name) {
                return i;
            }
        }
        return -1;
    }

    public void OpenSelectHud(Enemy[] selection) {
        for (int i = 0; i < SelectChoices.Length; i++) {
            Text selectText = SelectChoices[i];
            if (i >= selection.Length) {
                // if no more selectables, turn "off" button
                selectText.gameObject.SetActive(false);
                continue;
            }
            // is seleecting enemy
            Enemy enemy = selection[i].GetComponent<Enemy>();
            selectText.text = enemy.GetName();
            selectText.gameObject.SetActive(true);
        }

        this.isSelectingEnemy = true;
        this.enemyObjects = selection;
        selection[0].Highlight(true);
        EventSystem.current.SetSelectedGameObject(SelectChoices[0].gameObject);
    }

    public void OpenSelectHud(PartyUI[] selection, string[] currParty) {
        for (int i = 0; i < SelectChoices.Length; i++) {
            Text selectText = SelectChoices[i];
            Button selectButton = selectText.GetComponent<Button>();
            if (i >= selection.Length) {
                // if no more selectables, turn "off" button
                selectText.gameObject.SetActive(false);
                continue;
            }
            selectText.gameObject.SetActive(true);
            selectText.text = currParty[i];
            selectButton.interactable = GameManager.Instance.Party.IsAlive(currParty[i]);
        }

        this.isSelectingEnemy = false;
        this.partyObjects = selection;
        selection[0].Highlight(true);
        EventSystem.current.SetSelectedGameObject(SelectChoices[0].gameObject);
    }
    
    public void ExitSelectHud() {
        // deselects current hightlighted button
        EventSystem.current.SetSelectedGameObject(null);

        // dehighlight current selected object
        if (isSelectingEnemy) {
            // is selecting party
            enemyObjects[currSelectedIndx].Highlight(false);
        } else {
            // is selecting party
            partyObjects[currSelectedIndx].Highlight(false);
        }
        currSelected = "";
        currSelectedIndx = 0;
        isSelectingEnemy = false;
    }
}
