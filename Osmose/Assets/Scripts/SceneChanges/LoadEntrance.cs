using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadEntrance : MonoBehaviour {
    private string transitionFromArea = "Continue";

    // Use this for initialization
    void Start() {
        if (transitionFromArea == PlayerControls.Instance.PreviousAreaName) {
            GameManager.Instance.FadingBetweenAreas = false;
        }
        UIFade.Instance.FadeFromBlack();
    }
}
