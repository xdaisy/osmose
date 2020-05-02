using System.Collections.Generic;
using UnityEngine;

public class Investigate : MonoBehaviour {
    public string EventName; // name of specific event for specific dialogue
    [SerializeField] private SceneName scenetounlock; // scene to unlock

    [Header("Dialogue")]
    public string[] preEventDialogue; // lines of generic dialogue
    public string[] postEventDialogue; // lines of dialogue for before/after specific event

    private bool canActivate = false;

    // Update is called once per frame
    void Update() {
        if (canActivate && GameManager.Instance.CanStartDialogue() && Input.GetButtonDown("Interact") && !Dialogue.Instance.dBox.activeSelf) {
            string[] dialogue = getDialogue();
            if (!haveInvestigated()) {
                // did not investigate yet
                EventManager.Instance.AddEvent(EventName);
                EventManager.Instance.AddEvent(scenetounlock.GetSceneName());
            }
            Dialogue.Instance.ShowDialogue(dialogue, false);
        }
    }

    /// <summary>
    /// Get the correct dialogue to show and append getting item if there's an item obtained
    /// </summary>
    /// <returns>Correct dialogue to show</returns>
    private string[] getDialogue() {
        List<string> dialogue;
        if (haveInvestigated()) {
            dialogue = new List<string>(this.postEventDialogue);
        } else {
            dialogue = new List<string>(this.preEventDialogue);
        }

        return dialogue.ToArray();
    }

    /// <summary>
    /// Return whether or not have been investigated
    /// </summary>
    /// <returns>True if have investigated, false otherwise</returns>
    private bool haveInvestigated() {
        return EventName.Length > 0 && EventManager.Instance.DidEventHappened(EventName);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = false;
        }
    }
}
