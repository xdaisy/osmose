using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuySellPanel : MonoBehaviour {
    public CanvasGroup BuySellGroup;
    public ItemsPanel ItemsPanelUI;

    private string command;
    // Start is called before the first frame update
    void Start() {
        command = "";
    }

    // Update is called once per frame
    void Update() {
        if (BuySellGroup.interactable) {
            Text buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
            if (command != buttonText.text) {
                command = buttonText.text;
                switch (command) {
                    case "Buy":
                        ItemsPanelUI.UpdateList(true, false, false);
                        break;
                    case "Sell":
                        ItemsPanelUI.UpdateList(false, true, false);
                        break;
                }
            }
        }
    }
}
