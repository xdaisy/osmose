using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    public GameObject dBox; // dialogue box
    public Text dText; // dialogue text
    public Text dName; // name of dialogue 

    public static Dialogue instance; // dialogue manager instance

    private string[] dialogueLines; // lines of dialogue
    private int currentLine; // current line of dialogue

    private bool justStarted; // keep track if the dialogue just got started

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (dBox.activeSelf && Input.GetButtonUp("Interact")) {
            if (!justStarted) {
                currentLine++;
                if (currentLine >= dialogueLines.Length) {
                    dBox.SetActive(false);

                    currentLine = 0;

                    GameManager.Instance.DialogActive = false;
                }
                dText.text = dialogueLines[currentLine];
            } else {
                justStarted = false;
            }
        }
	}

    // start the dialogue
    public void ShowDialogue(string[] lines) {
        dBox.SetActive(true);
        dialogueLines = lines;
        currentLine = 0;
        dText.text = dialogueLines[currentLine];
        GameManager.Instance.DialogActive = true;
        justStarted = true;
    }

    public bool GetDialogueActive() {
        return dBox.activeSelf;
    }

    public void SetName(string name) {
        this.dName.text = name;
    }
}
