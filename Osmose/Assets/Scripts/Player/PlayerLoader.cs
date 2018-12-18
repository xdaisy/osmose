using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour {

    public GameObject Player;

    private void Awake() {
        if (PlayerControls.instance == null) {
            GameObject player = Instantiate(Player);
            player.name = player.name.Replace("(Clone)", ""); // remove (clone) from name
        }
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
