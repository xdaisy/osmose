using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    public GameObject dBox; // dialogue box
    public Text dText; // dialogue text
    public Text dName; // name of dialogue 

    private bool dialogueActive; // check if dialogue box is visible

    private string[] dialogueLines; // lines of dialogue
    private int currentLine; // current line of dialogue

    private static bool dialogueManagerExist;

    private PlayerControls player; // controls of player

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerControls>();

        //if (!dialogueManagerExist) {
        //    dialogueManagerExist = true;
        //    DontDestroyOnLoad(transform.gameObject);
        //} else {
        //    Destroy(gameObject);
        //}
	}
	
	// Update is called once per frame
	void Update () {
	}

    // start the dialogue
    public void ShowDialogue() {
        dialogueActive = true;
        dBox.SetActive(true);
        dText.text = dialogueLines[currentLine];
        player.SetCanMove(false); // make player not be able to move
    }

    // progress the dialogue
    public void ShowNextLine() {
        if (dialogueActive) {
            currentLine++;
        }
        if (currentLine >= dialogueLines.Length) {
            dBox.SetActive(false);
            dialogueActive = false;

            currentLine = 0;

            player.SetCanMove(true); // allow player to move again
        }
        dText.text = dialogueLines[currentLine];
    }

    public void setDialogueLines(string[] dialogueLines) {
        this.dialogueLines = dialogueLines;
    }

    public bool getDialogueActive() {
        return this.dialogueActive;
    }

    public void setCurrentLine(int currentLine) {
        this.currentLine = currentLine;
    }

    public void setName(string name) {
        this.dName.text = name;
    }
}
