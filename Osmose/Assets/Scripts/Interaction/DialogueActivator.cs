using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour
{

    public string specificEvent; // name of specific event for specific dialogue

    public string[] preEventDialogue; // lines of generic dialogue

    public string[] postEventDialogue; // lines of dialogue for before/after specific event

    private string personTalking; // name of person talking in dialogue

    private bool canActivate = false;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        if (canActivate && GameManager.Instance.CanStartDialogue() && Input.GetButtonDown("Interact") && !Dialogue.Instance.dBox.activeSelf) {
            if (!this.specificEvent.Equals("") && EventManager.DidEventHappened(specificEvent)) {
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
