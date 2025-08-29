using System.Collections.Generic;
using UnityEngine;

public class ActivatePuzzle : MonoBehaviour {
    public string[] InitialDialogue;
    public string[] YesDialogue;
    public string[] NoDialogue;
    public string[] CannotDialogue;
    public int NumClues;
    public SceneName sceneToLoad;

    private bool canActivate = false;

    // Update is called once per frame
    void Update() {
        if (canActivate && GameManager.Instance.CanStartDialogue() && Input.GetButtonDown("Interact") && !Dialogue.Instance.dBox.activeSelf) {
            bool canDoPuzzle = GameManager.Instance.GetNumCurrentClues() == NumClues;
            List<Clue> clues = CluesManager.Instance.GetAllClues();
            bool[] updatedClues = CluesManager.Instance.GetUpdatedClues();
            for (int i = 0; i < clues.Count; i++) {
                if (updatedClues.Length < i) {
                    continue;
                }

                Clue clue = clues[i];
                if (clue.GetCanUpdate()) {
                    if (!updatedClues[i]) {
                        canDoPuzzle = false;
                        break;
                    }
                }
            }
            
            Dialogue.Instance.ActivatePuzzleDialogue(InitialDialogue, YesDialogue, NoDialogue, CannotDialogue, canDoPuzzle, sceneToLoad.GetSceneName());
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
