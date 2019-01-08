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
    void Start()
    {
        if (this.CompareTag("NPC")) {
            // if this is an npc, npc is talking
            personTalking = this.name;
        } else {
            // else this the player's thoughts/observation
            GameObject player = GameObject.FindWithTag("Player");
            personTalking = player.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivate && Input.GetButtonDown("Interact") && !Dialogue.Instance.dBox.activeSelf) {
            Dialogue.Instance.SetName(personTalking);
            if (!this.specificEvent.Equals("") && CutSceneHandler.didEventHappened(specificEvent)) {
                // there is a specified event AND event happened
                Dialogue.Instance.ShowDialogue(this.postEventDialogue);
            } else {
                // event did not happened or there is no specified event
                Dialogue.Instance.ShowDialogue(this.preEventDialogue);
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
