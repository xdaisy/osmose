using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemsTypePanel : MonoBehaviour {
    public CanvasGroup ItemsTypeGroup;
    public ItemsPanel ItemsPanelUI;

    private string command;

    // Start is called before the first frame update
    void Start() {
        command = "";
    }

    // Update is called once per frame
    void Update() {
        if (ItemsTypeGroup.interactable) {
            Text buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
            if (command != buttonText.text) {
                command = buttonText.text;
                switch (command) {
                    case "All":
                        ItemsPanelUI.UpdateList(false, true, false);
                        break;
                    case "Items":
                        ItemsPanelUI.UpdateList(false, false, true);
                        break;
                    case "Equipment":
                        ItemsPanelUI.UpdateList(false, false, false);
                        break;
                }
            }
        }
    }
}
