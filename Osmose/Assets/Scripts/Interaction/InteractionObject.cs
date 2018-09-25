using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour {

    public bool talks; // if true, object here can talk to player

    private Dialogue dMang; // dialogue manager that will send the message to

    public string[] dialogueLines; // lines of dialogue

    public string personTalkingName; // name of person talking in dialogue

    // Use this for initialization
    void Start() {
        dMang = FindObjectOfType<Dialogue>();
    }

    // Update is called once per frame
    void Update() {

    }

    // talk if this object has a message
    public void Talk() {
        //dialogue.showText(message);
        //Debug.Log(message);
        if (!dMang.getDialogueActive()) {
            // shows if dialogue already not showing
            //dMang.dialogueLines = this.dialogueLines;
            dMang.setDialogueLines(this.dialogueLines);
            dMang.setCurrentLine(0);
            dMang.setName(personTalkingName);
            dMang.ShowDialogue();
        } else {
            // if dialogue is showing, go to next line
            dMang.ShowNextLine();
        }
    }
}
