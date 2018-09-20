using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    public GameObject dBox; // dialogue box
    public Text dText; // dialogue text

    public bool dialogueActive; // check if dialogue box is visible

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (dialogueActive && Input.GetButtonDown("Interact")) {
            dBox.SetActive(false);
            dialogueActive = false;
        }
	}

    public void showText(string dialogue) {
        dialogueActive = true;
        dBox.SetActive(true);
        dText.text = dialogue;
    }
}
