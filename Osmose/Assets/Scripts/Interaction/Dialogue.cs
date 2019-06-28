using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {
    public GameObject ParentObject;

    public GameObject dBox; // dialogue box
    public Text dText; // dialogue text
    public Text dName; // name of dialogue 

    public static Dialogue Instance; // dialogue manager instance

    private string[] dialogueLines; // lines of dialogue
    private int currentLine; // current line of dialogue

    private bool justStarted; // keep track if the dialogue just got started

    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(ParentObject);
        }
        DontDestroyOnLoad(ParentObject);
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
                showText();
            } else {
                justStarted = false;
            }
        }
	}

    // start the dialogue
    public void ShowDialogue(string[] lines, bool triggered) {
        dBox.SetActive(true);
        dialogueLines = lines;
        currentLine = 0;
        showText();
        GameManager.Instance.DialogActive = true;
        // if triggered bc cutscene, want to set to false
        // if triggered by interacting, want to set to true
        justStarted = !triggered;
    }

    private void showText() {
        string line = dialogueLines[currentLine];
        if (line.Contains("-")) {
            // if someone is talking, will have <name>-<text>
            string[] lineSplit = line.Split('-');
            dName.text = lineSplit[0];
            line = lineSplit[1];
        } else {
            // else, no one is talking. have text showing
            dName.text = "";
        }
        dText.text = line;
    }

    public bool GetDialogueActive() {
        return dBox.activeSelf;
    }
}
