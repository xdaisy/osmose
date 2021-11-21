using System.Collections.Generic;
using UnityEngine;

public class HintSystem : MonoBehaviour {
    [SerializeField] private SceneName lockedByScene;
    [SerializeField] private Hint[] hints;
    [SerializeField] private string[] preInvestigationDialogue;
    [SerializeField] private string[] obtainAllCluesDialogue;

    private bool canActivate = false;

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Interact") && canShowDialogue()) {
            string[] dialogue = getDialogue();
            Dialogue.Instance.ShowDialogue(dialogue, false);
        }
    }
    
    /// <summary>
    /// Get the dialogue based on which clues have been obtained
    /// </summary>
    /// <returns>Dialogue to show</returns>
    private string[] getDialogue() {
        List<string> dialogue = null;

        if (lockedByScene != null && !EventManager.Instance.DidEventHappened(lockedByScene.GetSceneName())) {
            // if investigation hasn't started
            dialogue = new List<string>(preInvestigationDialogue);
        } else {
            foreach(Hint hint in hints) {
                if (!CluesManager.Instance.DidObtainClue(hint.GetClue().GetClueNumber())) {
                    // if haven't obtain clue
                    dialogue = new List<string>(hint.GetDialogue());
                    break;
                } else if (hint.getCanUpdate() && !CluesManager.Instance.DidUpdateClue(hint.GetClue().GetClueNumber())) {
                    // clue has been obtained but not updated
                    dialogue = new List<string>(hint.GetUpdateDialogue());
                    break;
                }
            }
            if (dialogue == null) {
                // if all clues have been obtained
                dialogue = new List<string>(obtainAllCluesDialogue);
            }
        }

        return dialogue.ToArray();
    }

    /// <summary>
    /// Determine whether or not to show dialogue
    /// </summary>
    /// <returns>True if can show dialogue, false otherwise</returns>
    private bool canShowDialogue() {
        string[] dialogue = getDialogue();
        return dialogue.Length > 0 && canActivate && GameManager.Instance.CanStartDialogue() && !Dialogue.Instance.dBox.activeSelf; ;
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
