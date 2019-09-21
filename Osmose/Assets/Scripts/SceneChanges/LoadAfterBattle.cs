using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAfterBattle : MonoBehaviour {
    void Update() {
        if (PlayerControls.Instance.PreviousAreaName == Constants.BATTLE) {
            GameManager.Instance.FadingBetweenAreas = false;
            PlayerControls.Instance.PreviousAreaName = "";
        }
    }
}
