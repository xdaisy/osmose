using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    public GameObject dBox; // dialogue box
    public Text dText; // dialogue text

    public bool dialogueActive; // check if dialogue box is visible

    public string[] dialogueLines; // lines of dialogue
    public int currentLine; // current line of dialogue

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (dialogueActive && Input.GetButtonDown("Interact")) {
            currentLine++;
        }
        if (currentLine >= dialogueLines.Length) {
            dBox.SetActive(false);
            dialogueActive = false;

            currentLine = 0;
        }
        dText.text = dialogueLines[currentLine];
	}

    public void showText(string dialogue) {
        dialogueActive = true;
        dBox.SetActive(true);
        dText.text = dialogue;
    }

    public void ShowDialogue() {
        dialogueActive = true;
        dBox.SetActive(true);
    }
}
