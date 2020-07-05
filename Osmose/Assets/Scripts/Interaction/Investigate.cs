using System.Collections.Generic;
using UnityEngine;

public class Investigate : MonoBehaviour {
    public Clue Clue;
    public int ClueNumber;
    [SerializeField] private SceneName sceneToUnlock; // scene to unlock

    [Header("Dialogue")]
    public string[] preObtainDialogue; // lines of generic dialogue
    public string[] postObtainDialogue; // lines of dialogue for before/after specific event

    private bool canActivate = false;

    void Start() {
        // if there is no post obtaining dialogue, destroy this game object
        //if (haveInvestigated() && postObtainDialogue.Length > 0) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Interact") && canShowDialogue()) {
            string[] dialogue = getDialogue();
            if (!haveInvestigated()) {
                // did not investigate yet
                CluesManager.Instance.ObtainedClue(ClueNumber);
                GameManager.Instance.AddClue(Clue);
                if (sceneToUnlock != null) {
                    EventManager.Instance.AddEvent(sceneToUnlock.GetSceneName());
                }
            }
            Dialogue.Instance.ShowDialogue(dialogue, false);
        }
    }

    /// <summary>
    /// Determine whether or not to show dialogue
    /// </summary>
    /// <returns>True if can show dialogue, false otherwise</returns>
    private bool canShowDialogue() {
        bool haveDialogue = haveInvestigated() ? postObtainDialogue.Length > 0 : true;
        return haveDialogue && canActivate && GameManager.Instance.CanStartDialogue() &&  !Dialogue.Instance.dBox.activeSelf;
    }

    /// <summary>
    /// Get the correct dialogue to show and append getting item if there's an item obtained
    /// </summary>
    /// <returns>Correct dialogue to show</returns>
    private string[] getDialogue() {
        List<string> dialogue;
        if (haveInvestigated()) {
            dialogue = new List<string>(this.postObtainDialogue);
        } else {
            dialogue = new List<string>(this.preObtainDialogue);
        }

        return dialogue.ToArray();
    }

    /// <summary>
    /// Return whether or not have been investigated
    /// </summary>
    /// <returns>True if have investigated, false otherwise</returns>
    private bool haveInvestigated() {
        return CluesManager.Instance.DidObtainClue(ClueNumber);
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
