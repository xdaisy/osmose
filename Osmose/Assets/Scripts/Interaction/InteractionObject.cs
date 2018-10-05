using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour {

    public bool talks; // if true, object here can talk to player

    public string specificEvent; // name of specific event for specific dialogue

    private Dialogue dMang; // dialogue manager that will send the message to

    public string[] preEventDialogue; // lines of generic dialogue

    public string[] postEventDialogue; // lines of dialogue for before/after specific event

    private string personTalking; // name of person talking in dialogue

    // Use this for initialization
    void Start() {
        dMang = FindObjectOfType<Dialogue>();
        if (this.CompareTag("NPC")) {
            // if this is an npc, npc is talking
            Debug.Log("npc " + this.name);
            personTalking = this.name;
        } else {
            // else this the player's thoughts/observation
            GameObject player = GameObject.FindWithTag("Player");
            personTalking = player.name;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    // talk if this object has a message
    public void Talk() {
        if (!dMang.getDialogueActive()) {
            // shows if dialogue already not showing

            if (!this.specificEvent.Equals("") && CutSceneHandler.didEventHappened(specificEvent)) {
                // there is a specified event AND event happened
                dMang.setDialogueLines(this.postEventDialogue);
            } else {
                // event did not happened or there is no specified event
                dMang.setDialogueLines(this.preEventDialogue);
            }

            dMang.setCurrentLine(0);
            dMang.setName(personTalking);
            dMang.ShowDialogue();
        } else {
            // if dialogue is showing, go to next line

            dMang.ShowNextLine();
        }
    }
}
