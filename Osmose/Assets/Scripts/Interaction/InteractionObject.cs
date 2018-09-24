using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour {

    public bool talks; // if true, object here can talk to player

    public string message; // message this object will give to player

    private Dialogue dMang; // dialogue manager that will send the message to

    public string[] dialogueLines; // lines of dialogue

    // Use this for initialization
    void Start() {
        dMang = FindObjectOfType<Dialogue>();
    }

    // Update is called once per frame
    void Update() {

    }

    void DoInteraction() {

    }

    // talk if this object has a message
    public void Talk() {
        //dialogue.showText(message);
        //Debug.Log(message);
        if (!dMang.dialogueActive) {
            // shows if dialogue already not showing
            dMang.dialogueLines = this.dialogueLines;
            dMang.currentLine = 0;
            dMang.ShowDialogue();
        }
    }
}
