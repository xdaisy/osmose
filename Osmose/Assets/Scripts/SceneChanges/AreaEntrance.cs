using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour {
    public string transitionFromArea;

    public bool IsBattleMap;

    // Use this for initialization
    void Start() {
        if (transitionFromArea == PlayerControls.Instance.PreviousAreaName) {
            // set player to entrance's position
            PlayerControls.Instance.SetPosition(transform.position);
            GameManager.Instance.IsBattleMap = IsBattleMap;
            if (!IsBattleMap) {
                GameManager.Instance.LastTown = GameManager.Instance.CurrentScene;
            }
            GameManager.Instance.FadingBetweenAreas = false;
        }
        UIFade.Instance.FadeFromBlack();
    }
}
