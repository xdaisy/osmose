using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectHud : MonoBehaviour
{
    public Text[] SelectChoices; // what can be selected on the select hud

    private Enemy[] enemyObjects; // list of enemy objects that will be highlighted
    private PartyMember[] partyObjects; // list of party member objects that will be highlighted
    private bool isSelectingEnemy; // keep track if selecting enemies or party members

    private string currSelected;
    private int currSelectedIndx;

    private void Start() {
        currSelected = "";
        currSelectedIndx = 0;
    }

    void Update() {
        Text currObject = EventSystem.current.currentSelectedGameObject.GetComponent<Text>();
        if (currSelected != currObject.text) {
            int selectedIndx = findCurrObject(currObject.text);

            if (isSelectingEnemy) {
                // dehighlight previous selected enemy
                Enemy prevSelectedEnemy = enemyObjects[currSelectedIndx];
                prevSelectedEnemy.Highlight(false);

                // highlight current selected enemy
                Enemy currSelectedEnemy = enemyObjects[selectedIndx];
                currSelectedEnemy.Highlight(true);
            }

            currSelected = currObject.text;
            currSelectedIndx = selectedIndx;
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
            Button selectButton = selectText.GetComponent<Button>();
            if (i >= selection.Length) {
                // if no more selectables, turn "off" button
                selectText.text = "";
                selectButton.interactable = false;
                continue;
            }
            // is seleecting enemy
            Enemy enemy = selection[i].GetComponent<Enemy>();
            selectText.text = enemy.EnemyName;
            selectButton.interactable = true;
        }

        this.isSelectingEnemy = true;
        this.enemyObjects = selection;
        selection[0].Highlight(true);
        EventSystem.current.SetSelectedGameObject(SelectChoices[0].gameObject);
    }

    public void OpenSelectHud(PartyMember[] selection) {
        for (int i = 0; i < SelectChoices.Length; i++) {
            Text selectText = SelectChoices[i];
            Button selectButton = selectText.GetComponent<Button>();
            if (i >= selection.Length) {
                // if no more selectables, turn "off" button
                selectText.text = "";
                selectButton.interactable = false;
                continue;
            }
            string name = "";
            if (isSelectingEnemy) {
                // is seleecting enemy
                Enemy enemy = selection[i].GetComponent<Enemy>();
                name = enemy.EnemyName;
            } else {
                // is selecting party member
            }
            selectText.text = name;
            selectButton.interactable = true;
        }

        this.isSelectingEnemy = false;
        this.partyObjects = selection;
        selection[0].Highlight(true);
        EventSystem.current.SetSelectedGameObject(SelectChoices[0].gameObject);
    }
    
    public void ExitSelectHud() {
        // deselects current hightlighted button
        Button currHighlightedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        EventSystem.current.SetSelectedGameObject(null);
        currHighlightedButton.interactable = false;

        // dehighlight current selected object
        if (isSelectingEnemy) {
            enemyObjects[currSelectedIndx].Highlight(false);
        }
        currSelected = "";
        currSelectedIndx = 0;
    }
}
