using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHud : MonoBehaviour {
    public Button ItemsButton;

    public void SetItemsActive() {
        ItemsButton.interactable = true;
    }

    public void ArenShifted(bool isShifted) {
        ItemsButton.interactable = !isShifted;
    }
}
