using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectHud : MonoBehaviour
{
    public Text[] SelectChoices; // what can be selected on the select hud

    public void OpenSelectHud(string[] selection) {
        for (int i = 0; i < SelectChoices.Length; i++) {
            Text selectText = SelectChoices[i];
            Button selectButton = selectText.GetComponent<Button>();
            if (i >= selection.Length) {
                // if no more selectables, turn "off" button
                selectText.text = "";
                selectButton.interactable = false;
                continue;
            }
            selectText.text = selection[i];
            selectButton.interactable = true;
        }

        EventSystem.current.SetSelectedGameObject(SelectChoices[0].gameObject);
    }
    
    public void ExitSelectHud() {
        // deselects current hightlighted button
        Button currHighlightedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        EventSystem.current.SetSelectedGameObject(null);
        currHighlightedButton.interactable = false;
    }
}
