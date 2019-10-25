using UnityEngine;

public class LoadAfterBattle : MonoBehaviour {
    void Update() {
        if (PlayerControls.Instance.PreviousAreaName == Constants.BATTLE ||
            PlayerControls.Instance.PreviousAreaName == Constants.MAP) {
            GameManager.Instance.FadingBetweenAreas = false;
            PlayerControls.Instance.PreviousAreaName = "";
        }
    }
}
