using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour {

    public string transitionFromArea;

    public bool IsBattleMap;

    // Use this for initialization
    void Start() {
        if (transitionFromArea == PlayerControls.instance.previousAreaName) {
            // set player to entrance's position
            PlayerControls.instance.transform.position = transform.position;
            PlayerControls.instance.IsBattleMap = IsBattleMap;
            PlayerControls.instance.SetCanMove(true);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
