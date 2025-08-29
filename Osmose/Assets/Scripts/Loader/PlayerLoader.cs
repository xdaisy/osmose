using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour {

    public GameObject Player;

    private void Awake() {
        if (PlayerControls.Instance == null) {
            GameObject player = Instantiate(Player);
            player.name = player.name.Replace("(Clone)", ""); // remove (clone) from name
        }
    }
}
