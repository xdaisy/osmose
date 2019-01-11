﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour {

    public string transitionFromArea;

    public bool IsBattleMap;

    // Use this for initialization
    void Start() {
        if (transitionFromArea == PlayerControls.Instance.PreviousAreaName) {
            // set player to entrance's position
            PlayerControls.Instance.transform.position = transform.position;
            PlayerControls.Instance.IsBattleMap = IsBattleMap;
            GameManager.Instance.FadingBetweenAreas = false;
        }
        UIFade.Instance.FadeFromBlack();
    }

    // Update is called once per frame
    void Update() {

    }
}