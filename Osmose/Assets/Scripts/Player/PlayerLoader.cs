using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour {

    public GameObject Player;

	// Use this for initialization
	void Start () {
		if(PlayerControls.instance == null) {
            GameObject player = Instantiate(Player);
            player.name = player.name.Replace("(Clone)", ""); // remove (clone) from name
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
