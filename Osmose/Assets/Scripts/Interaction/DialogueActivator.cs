﻿using UnityEngine;

/// <summary>
/// Class that handles when player is talking to an NPC
/// </summary>
public class DialogueActivator : MonoBehaviour {
    public string specificEvent; // name of specific event for specific dialogue

    public string[] preEventDialogue; // lines of generic dialogue

    public string[] postEventDialogue; // lines of dialogue for before/after specific event

    private bool canActivate = false;

    // Update is called once per frame
    void Update() {
        if (canActivate && GameManager.Instance.CanStartDialogue() && Input.GetButtonDown("Interact") && !Dialogue.Instance.dBox.activeSelf) {
            if (!this.specificEvent.Equals("") && EventManager.Instance.DidEventHappened(specificEvent)) {
                // there is a specified event AND event happened
                Dialogue.Instance.ShowDialogue(this.postEventDialogue, false);
            } else {
                // event did not happened or there is no specified event
                Dialogue.Instance.ShowDialogue(this.preEventDialogue, false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = false;
        }
    }
}
